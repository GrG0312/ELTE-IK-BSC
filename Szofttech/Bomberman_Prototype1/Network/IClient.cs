using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Network
{
    /// <summary>
    /// An Interface that every Client object has to implement
    /// </summary>
    public interface IClient
    {
        public int PlayerID { get; }
        /// <summary>
        /// The channel to send data to a <see cref="IServer"/>
        /// </summary>
        public TcpClient SendToServer { get; }
        /// <summary>
        /// The channel to recieve data from a <see cref="IServer"/>
        /// </summary>
        public TcpClient GetFromServer { get; }
        /// <summary>
        /// Connects both properties to the host specified by <paramref name="ip"/> and <paramref name="port"/>
        /// </summary>
        /// <param name="ip">The IP address (or name) of the Host</param>
        /// <param name="port">The port</param>
        public virtual void ConnectToServer(string ip, int port)
        {
            SendToServer.Connect(ip, port);
            GetFromServer.Connect(ip, port);
        }
        /// <summary>
        /// We send the <paramref name="data"/> via the <see cref="SendToServer"/> property
        /// </summary>
        /// <param name="data">The data we want to send as a <see cref="Message"/></param>
        public virtual void SendDataToServer(MessageType type, string data)
        {
            NetworkStream outStream = SendToServer.GetStream();
            Message sending = new Message(type, data, PlayerID);
            outStream.Write(Encoding.Default.GetBytes(sending.ToString()));

            Log($"Data sent: {sending}");
        }
        /// <summary>
        /// Recieves data from an <see cref="IServer"/> via the <see cref="GetFromServer"/> property
        /// </summary>
        /// <returns>The incoming data as a <see cref="Message"/></returns>
        public virtual string ReadDataFromServer(bool shouldWait = false)
        {
            NetworkStream stream = GetFromServer.GetStream();

            StringBuilder retval = new StringBuilder();
            while (stream.DataAvailable || shouldWait)
            {
                byte[] buffer = new byte[1024];
                stream.Read(buffer, 0, buffer.Length);
                string read = Encoding.Default.GetString(buffer);
                read = read.Replace("\0", string.Empty);
                retval.Append(read);
                shouldWait = false;
                Log($"Recieved: {retval}");
            }
            return retval.ToString();
        }
        public void Log(params string[] args)
        {
            string log = String.Join("\n\t", args);
            log = String.Join("\n\t", $"Client {PlayerID}:", log);
            Debug.WriteLine(log);
        }
    }
}
