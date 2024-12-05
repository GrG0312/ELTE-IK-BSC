using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bomberman_Prototype1.Persistence;
using Bomberman_Prototype1.Model;
using System.Diagnostics;
using System.Windows.Data;
using System.Security.AccessControl;

namespace Bomberman_Prototype1.Network
{
    public class IllegalDataFormatException : Exception { }
    public class BMServer : IServer
    {
        public const int PORT = 8888;

        private Thread listeningThread;
        private IServer thisAsInterface;
        private bool shouldEnd;
        private ManualResetEventSlim waiter = new ManualResetEventSlim();
        private bool shouldSleep;
        #region Properties
        public TcpListener ServerSocket { get; }
        public List<IClient> Clients { get; private set; }
        #endregion

        #region Events
        public event EventHandler<string>? PlayerConnected;
        public event EventHandler<(int, Direction)>? MovePlayer;
        public event EventHandler<(int, string)>? PlaceThing;
        #endregion

        public BMServer()
        {
            thisAsInterface = this;
            ServerSocket = TcpListener.Create(PORT);
            Clients = new List<IClient>();
            listeningThread = new Thread(ThreadTask);
            listeningThread.Name = "Server Listening Thread";
            shouldEnd = false;
            shouldSleep = false;
        }
        public void PauseRead()
        {
            shouldSleep = true;
            waiter.Reset();
        }
        public void ResumeRead()
        {
            shouldSleep = false;
            waiter.Set();
        }
        public string GetLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        public void AcceptClientConnections(int maximum, bool isbr)
        {
            ServerSocket.Start();
            while (Clients.Count < maximum)
            {
                TcpClient clientOutSocket = ServerSocket.AcceptTcpClient();
                TcpClient clientInSocket = ServerSocket.AcceptTcpClient();
                BMClient client = new BMClient(clientOutSocket, clientInSocket);
                client.InitializeProfile(ReadProfileDataFromClient(client));
                lock (this)
                {
                    client.PlayerID = Clients.Count + 1;
                    thisAsInterface.SendDataToClient(client, MessageType.PlayerID, $"{isbr}");
                    Clients.Add(client);
                }
                Application.Current.Dispatcher.Invoke( () => PlayerConnected?.Invoke(client, client.PlayerProfile!.Name));
            }

            waiter.Set();
            listeningThread.Start();
        }
        /// <summary>
        /// Reads in a <see cref="Message"/> and extracts the Profile data of a <see cref="IClient"/>
        /// </summary>
        /// <param name="client">Which <see cref="IClient"/> we want to get the data from</param>
        /// <returns>The extracted <see cref="Profile"/></returns>
        /// <exception cref="IllegalDataFormatException"></exception>
        private Profile ReadProfileDataFromClient(IClient client)
        {
            string read = thisAsInterface.ReadDataFromClient(client, true);
            Message? recieved = Message.ConvertToMessage(read);
            if (recieved == null || recieved.Type != MessageType.Profile)
            {
                throw new IllegalDataFormatException();
            }
            string[] dataChunks = recieved.Data.Split(';', StringSplitOptions.RemoveEmptyEntries);

            Profile profile = new Profile(
                dataChunks[0], 
                int.Parse(dataChunks[1]), 
                int.Parse(dataChunks[2]), 
                dataChunks[3],
                int.Parse(dataChunks[4]),
                int.Parse(dataChunks[5]),
                int.Parse(dataChunks[6])
            );
            return profile;
        }
        private void ThreadTask()
        {
            while (!shouldEnd)
            {
                if (waiter.IsSet && !shouldSleep)
                {
                    try
                    {
                        string read = thisAsInterface.ReadDataFromAllClients();
                        while (read != string.Empty)
                        {
                            Message? message = Message.ConvertToMessage(Message.ChopOffFirstMessage(ref read));
                            if (message != null)
                            {
                                Debug.WriteLine($"Server:\n\tRecieved from: {message.PlayerID}\n\tData: {message.Data}\n");
                                shouldEnd = HandlePostSetupMessage(message);
                            }
                        }
                    }
                    catch (IOException){ }
                    catch (ObjectDisposedException){ }
                }
            }
            Debug.WriteLine("Server: thread ended.");
        }
        private bool HandlePostSetupMessage(Message message)
        {
            string[] raw = message.Data.Split(';');
            switch (message.Type)
            {
                case MessageType.Move:
                    Direction dir = (Direction)Enum.Parse(typeof(Direction), raw[0]);
                    Application.Current.Dispatcher.Invoke( () => MovePlayer?.Invoke(this, (message.PlayerID, dir)));
                    break;
                case MessageType.Action:
                    Application.Current.Dispatcher.Invoke( () => PlaceThing?.Invoke(this, (message.PlayerID, raw[0])));
                    break;
                default:
                    return true;
            }
            return false;
        }
        public void Shutdown(bool automatic = false)
        {
            thisAsInterface.SendDataToAllClients(MessageType.Shutdown, $"{automatic}");
            lock (this)
            {
                shouldEnd = true;
            }
        }
        public string GetStatsFromClients()
        {
            waiter.Reset();
            string retval = string.Empty;
            foreach (IClient client in Clients)
            {
                string data = thisAsInterface.ReadDataFromClient(client,true);
                Message? msg = Message.ConvertToMessage(data);
                if (msg != null)
                {
                    if (msg.Type != MessageType.RoundResults)
                    {
                        throw new FormatException();
                    }
                    string[] raw = msg.Data.Split(";");
                    retval += msg.PlayerID + ":" + raw[0] + ":" + raw[1] + "@";
                }
            }
            waiter.Set();
            return retval;
        }
    }
}
