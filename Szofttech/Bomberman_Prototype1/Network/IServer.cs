using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Network
{
    /// <summary>
    /// An Interface that every Server object has to implement
    /// </summary>
    public interface IServer
    {
        public TcpListener ServerSocket { get; }
        /// <summary>
        /// We store all our connected clients in a <see cref="List{IClient}"/>
        /// </summary>
        public List<IClient> Clients { get; }
        /// <summary>
        /// Method for accepting incoming client connections to the server
        /// </summary>
        /// <param name="maximum">The maximum ammount of players the server will accept</param>
        public void AcceptClientConnections(int maximum, bool isbr);
        /// <summary>
        /// Sends data to a client's GetFromServer property 
        /// </summary>
        /// <param name="client">The client we send the data to</param>
        /// <param name="data">The data we want to send</param>
        public virtual void SendDataToClient(IClient client, MessageType type, string data)
        {
            NetworkStream stream = client.GetFromServer.GetStream();
            Message sending = new Message(type, data, client.PlayerID);
            stream.Write(Encoding.Default.GetBytes(sending.ToString()));

            Debug.WriteLine($"Server:\n\tSent to: {client.PlayerID}\n\tData: {sending}\n");
        }
        /// <summary>
        /// We read data from a client's <see cref="IClient.SendToServer"/> property
        /// </summary>
        /// <param name="client">The client we recieve the data from</param>
        /// <returns>The retval data in as a <see cref="Message"/></returns>
        public virtual string ReadDataFromClient(IClient client, bool shouldWait = false)
        {
            NetworkStream stream = client.SendToServer.GetStream();
            StringBuilder retval = new StringBuilder();
            while (stream.DataAvailable || shouldWait)
            {
                byte[] buffer = new byte[1024];
                Debug.WriteLine(Encoding.Default.GetString(buffer));
                stream.Read(buffer, 0, buffer.Length);
                string read = Encoding.Default.GetString(buffer);
                read = read.Replace("\0", string.Empty);
                retval.Append(read);
                shouldWait = false;
            }
            return retval.ToString();
        }
        public virtual string ReadDataFromAllClients()
        {
            string retval = string.Empty;
            foreach (IClient client in Clients)
            {
                retval += ReadDataFromClient(client);
            }
            return retval;
        }
        /// <summary>
        /// We invoke the <see cref="SendDataToClient(IClient, MessageType, string)"/> method to every <see cref="IClient"/> stored in our <see cref="Clients"/> list
        /// </summary>
        /// <param name="data">The data as a <see cref="string"/></param>
        public virtual void SendDataToAllClients(MessageType type, string data)
        {
            foreach (IClient client in Clients)
            {
                SendDataToClient(client, type, data);
            }
        }
        public string GetLocalIPAddress();
    }
}
