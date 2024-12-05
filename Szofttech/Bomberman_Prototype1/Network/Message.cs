using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1.Network
{
    public enum MessageType 
    { 
        Profile, 
        PlayerID,
        Move, 
        Action, 
        UIElement,
        Shutdown,
        RoundResults,
        StatDisplay,
        Shrink,
        Over,
        Timer,
        MonsterMoved
    }
    public enum UIElementType 
    {
        Board, 
        Wall,
        Player,
        Monster,
        Explosion,
        Powerup,
        RemovedElement,
    }
    public class Message
    {
        public MessageType Type { get; }
        public string Data { get; }
        public int PlayerID { get; }

        public Message(MessageType type, string data, int id)
        {
            Type = type;
            Data = data;
            PlayerID = id;
        }
        /// <summary>
        /// We convert the data in the instance into a string
        /// </summary>
        /// <returns><see cref="MessageType"/>?PlayerID?Data;As;A;String</returns>
        public override string ToString()
        {
            //~MessageType?PlayerID?The;data;as;a;string~
            string msg = String.Join('?', Type.ToString(), Data, PlayerID);
            return "~" + msg + "~";
        }
        public static Message? ConvertToMessage(string recieved)
        {
            recieved = recieved.Replace("\0", string.Empty);
            recieved = recieved.Trim('~');
            string[] raw = recieved.Split('?');//Message is separated by ? characters
            if (recieved != string.Empty)
            {
                return new Message((MessageType)Enum.Parse(typeof(MessageType), raw[0]), raw[1], int.Parse(raw[2]));
            }
            return null;
        }
        public static string ChopOffFirstMessage(ref string read, string divider = "~")
        {
            if (!String.IsNullOrWhiteSpace(read))
            {
                read = read.TrimStart('~');
                int charLocation = read.IndexOf(divider, StringComparison.Ordinal);
                if (charLocation > 0)
                {
                    string retval = read.Substring(0, charLocation);
                    if (charLocation == read.Length - 1)
                    {
                        read = string.Empty;
                    } else
                    {
                        read = read.Substring(charLocation, read.Length - charLocation);
                    }
                    return retval;
                }
            }
            return read;
        }
    }
}
