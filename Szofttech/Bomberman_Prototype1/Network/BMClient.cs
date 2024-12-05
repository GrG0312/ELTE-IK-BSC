using System.Windows;
using System.IO;
using System.Net.Sockets;
using Bomberman_Prototype1.Model.CustomEventArgs;
using Bomberman_Prototype1.Persistence;
using Bomberman_Prototype1.ViewModel;
using Bomberman_Prototype1.Model;
using Bomberman_Prototype1.Model.Entities.Powerups;
using System.Net;
using System.Diagnostics;
using Microsoft.Win32;
using System.Reflection;

namespace Bomberman_Prototype1.Network
{
    /// <summary>
    /// The class that we'll use as clients, implements the <see cref="IClient"/> interface
    /// <list mtype="bullet">
    ///     <item>
    ///         <term>'Hides' the interface methods with default implementation</term>
    ///         <description>
    ///             This is because of the issue of multiple inheritance. 
    ///             If we would implement multiple interfaces with the same methods, the compiler would not know which method should it execute.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>Creating a <see cref="BMClient"/> variable</term>
    ///         <description>
    ///             You can only access the methods defined in this class    
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             Casting as <see cref="IClient"/>
    ///         </term>
    ///         <description>
    ///             You can access every method with default implementations, and still use those methods from <see cref="BMClient"/> that don't have default implementation,
    ///             but are declared in <see cref="IClient"/>
    ///         </description>
    ///     </item>
    /// </list>
    /// </summary>
    public class BMClient : IClient
    {
        public const int PORT = 8888;

        private IClient thisAsInterface;
        private int pid;
        private Thread listeningThread;
        private bool shouldEnd;
        public int RoundsWon {  get; private set; }
        public int PlayedTime { get; private set; }
        public int PlayerID 
        { 
            get {  return pid; }
            set
            {
                if (pid == -1)
                {
                    pid = value;
                }
            }
        }
        public TcpClient SendToServer { get; }
        public TcpClient GetFromServer { get; }
        public Profile? PlayerProfile { get; private set; }

        public EventHandler<PlaceValueEventArgs<int>>? NewBoardEvent;
        public EventHandler<PlaceValueEventArgs<Field>>? NewWallEvent;
        public EventHandler<PlaceValueEventArgs<(Profile, int)>>? NewPlayerEvent;
        public EventHandler<(int, Direction)>? PlayerMoved;
        public EventHandler<PlaceValueEventArgs<Field>>? PlaceObstacle;
        public EventHandler<int>? PlaceBomb;
        public EventHandler<PlaceEventArgs>? ExplosionEvent;
        public EventHandler<PlaceValueEventArgs<Type>>? NewPowerupEvent;
        public EventHandler<PlaceValueEventArgs<int>>? RemovedUIElement;
        public EventHandler<string>? StatsCameInEvent;
        public EventHandler<bool>? SetBREvent;
        public EventHandler<PlaceValueEventArgs<Field>>? PlaceBlockerEvent;
        public EventHandler<(bool, int, string)>? GameIsOverEvent;
        public EventHandler<string>? ChangeTimeEvent;
        public EventHandler<PlaceValueEventArgs<(string,int)>>? NewMonsterEvent;
        public EventHandler<PlaceValueEventArgs<int>>? MonsterMovedEvent;

        public BMClient(TcpClient to, TcpClient from, Profile p = null!)
        {
            thisAsInterface = this;
            SendToServer = to;
            GetFromServer = from;
            PlayerProfile = p;
            pid = -1; //we get it after we connect to the server
            RoundsWon = 0;
            listeningThread = new Thread(ThreadTask);
        }
        public void InitializeProfile(Profile p)
        {
            PlayerProfile = p;
        }
        private void SendProfileDataToServer()
        {
            //output is: the;data;we;want;to;send;
            string output = string.Join(';', 
                PlayerProfile!.Name, 
                PlayerProfile.PlayedGames, 
                PlayerProfile.GamesWon, 
                PlayerProfile.SpritePath, 
                PlayerProfile.PlayTime.Hours, 
                PlayerProfile.PlayTime.Minutes, 
                PlayerProfile.PlayTime.Seconds
            );
            thisAsInterface.SendDataToServer(MessageType.Profile, output);
        }
        /// <summary>
        /// We connect to the Server, after connecting we send our Profile data and recieve the our ID
        /// </summary>
        public void ConnectToServer(string ipAddress)//This "hides" the default implementation
        {
            //in order to use the default implementations from the outside world, use object casting ((IClient)variable).ConnectToServer
            thisAsInterface.ConnectToServer(ipAddress, PORT);
            SendProfileDataToServer();
            GetPlayerIdFromServer();
            listeningThread.Start();
        }
        /// <summary>
        /// Reads a <see cref="Message"/> from a server, and if the <see cref="MessageType"/> is <see cref="MessageType.PlayerID"/>,
        /// then it sets the <see cref="PlayerID"/> property.
        /// </summary>
        /// <exception cref="IllegalDataFormatException"></exception>
        private void GetPlayerIdFromServer()
        {
            string recieved = thisAsInterface.ReadDataFromServer(true);
            Message? retval = Message.ConvertToMessage(recieved);
            if (retval == null || retval.Type != MessageType.PlayerID)
            {
                throw new IllegalDataFormatException();
            }
            PlayerID = retval.PlayerID;
            SetBREvent?.Invoke(this, bool.Parse(retval.Data));
            thisAsInterface.Log("Player ID has been set");
        }
        /// <summary>
        /// We will run this method on a different thread to listen for incoming messages
        /// </summary>
        private void ThreadTask()
        {
            Thread.Sleep(500);
            while (!shouldEnd)
            {
                try
                {
                    string read = thisAsInterface.ReadDataFromServer();
                    while (read != string.Empty)
                    {
                        Message? message = Message.ConvertToMessage(Message.ChopOffFirstMessage(ref read));
                        if (message != null)
                        {
                            shouldEnd = HandlePostSetupMessage(message);
                        }
                    }
                }
                catch (IOException) 
                {
                    thisAsInterface.Log("There has been an IOE");
                }
                catch (ObjectDisposedException) 
                {
                    thisAsInterface.Log("There has been an ODE");
                }
            }
            thisAsInterface.Log("Thread ended");
        }
        /// <summary>
        /// Handles the incoming messages which were cought by the <see cref="ThreadTask"/> method
        /// </summary>
        /// <param name="message">The <see cref="Message"/> we want to handle</param>
        private bool HandlePostSetupMessage(Message message)
        {
            string[] raw = message.Data.Split(';');
            int col, row;
            switch (message.Type)
            {
                case MessageType.Move:
                    thisAsInterface.Log("Got an order to move a Player");
                    int id = int.Parse(raw[0]);
                    Direction dir = (Direction)Enum.Parse(typeof(Direction), raw[1]);
                    Debug.WriteLine(dir);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PlayerMoved?.Invoke(this, (id, dir));
                    });
                    break;
                case MessageType.Action:
                    if (raw[0] == "bomb")
                    {
                        Application.Current.Dispatcher?.Invoke(() => PlaceBomb?.Invoke(this, int.Parse(raw[1])));
                    }
                    else
                    {
                        col = int.Parse(raw[1]);
                        row = int.Parse(raw[2]);
                        Field f = (Field)Enum.Parse(typeof(Field), raw[3]);
                        Application.Current.Dispatcher?.Invoke(() => PlaceObstacle?.Invoke(this, new PlaceValueEventArgs<Field>(col, row, f)));
                    }
                    break;
                case MessageType.UIElement:
                    thisAsInterface.Log("Got a UIElement", message.Data);
                    UIElementMessage(message.Data);
                    break;
                case MessageType.Shutdown:
                    bool automatic = bool.Parse(raw[0]);
                    if (automatic)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            thisAsInterface.SendDataToServer(MessageType.RoundResults, $"{RoundsWon}");
                            RoundsWon = 0;
                        });
                    }
                    return true;
                case MessageType.RoundResults:
                    int increase = int.Parse(raw[0]) == PlayerID ? 1 : 0;
                    int gameTime = int.Parse(raw[1]);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        RoundsWon += increase;
                        PlayedTime += gameTime;
                        thisAsInterface.SendDataToServer(MessageType.RoundResults, $"{PlayerProfile!.Name};{RoundsWon}");
                    });
                    break;
                case MessageType.StatDisplay:
                    thisAsInterface.Log(message.Data);
                    Application.Current.Dispatcher.Invoke( () =>
                    {
                        StatsCameInEvent?.Invoke(this, message.Data);
                    });
                    break;
                case MessageType.Shrink:
                    Field field = (Field)Enum.Parse(typeof(Field), raw[0]);
                    col = int.Parse(raw[1]);
                    row = int.Parse(raw[2]);
                    Application.Current.Dispatcher.Invoke(() => PlaceBlockerEvent?.Invoke(this, new PlaceValueEventArgs<Field>(col, row, field)));
                    break;
                case MessageType.Over:
                    int winnerId = int.Parse(raw[0]);
                    bool isDraw = bool.Parse(raw[1]);
                    bool didIWin = winnerId == PlayerID && !isDraw;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GameIsOverEvent?.Invoke(this, (didIWin, PlayedTime, raw[2]));
                    });
                    break;
                case MessageType.Timer:
                    string newTime = raw[0];
                    Application.Current.Dispatcher.Invoke(() => ChangeTimeEvent?.Invoke(this, newTime));
                    break;
                case MessageType.MonsterMoved:
                    col = int.Parse(raw[0]);
                    row = int.Parse(raw[1]);
                    int mid = int.Parse(raw[2]);
                    Application.Current.Dispatcher.Invoke(() => MonsterMovedEvent?.Invoke(this, new PlaceValueEventArgs<int>(col,row,mid)));
                    break;
                default:
                    thisAsInterface.Log("Not supported Post-Setup message");
                    return true;
            }
            return false;
        }
        private void UIElementMessage(string data)
        {
            string[] raw = data.Split(';');
            UIElementType mtype = (UIElementType)Enum.Parse(typeof(UIElementType), raw[0]);
            switch (mtype)
            {
                case UIElementType.Board:
                    int cols = int.Parse(raw[1]);
                    int rows = int.Parse(raw[2]);
                    int itype = int.Parse(raw[3]);
                    Application.Current.Dispatcher.Invoke( () => NewBoardEvent?.Invoke(this, new PlaceValueEventArgs<int>(cols, rows, itype)));
                    break;
                case UIElementType.Wall:
                    Field field = (Field)Enum.Parse(typeof(Field), raw[1]);
                    int col = int.Parse(raw[2]);
                    int row = int.Parse(raw[3]);
                    Application.Current.Dispatcher.Invoke(() => NewWallEvent?.Invoke(this, new PlaceValueEventArgs<Field>(col, row, field)));
                    break;
                case UIElementType.Player:
                    col = int.Parse(raw[1]);
                    row = int.Parse(raw[2]);
                    Profile p = new Profile(raw[3], int.Parse(raw[4]), int.Parse(raw[5]), raw[6]);
                    int id = int.Parse(raw[7]);
                    Application.Current.Dispatcher.Invoke(() => NewPlayerEvent?.Invoke(this, new PlaceValueEventArgs<(Profile, int)>(col, row, (p, id))));
                    break;
                case UIElementType.Powerup:
                    col = int.Parse(raw[1]);
                    row = int.Parse(raw[2]);
                    string url = raw[3];
                    Assembly a = Assembly.Load("Model");
                    Type ttype = a.GetType(raw[4])!;
                    Application.Current.Dispatcher.Invoke(() => NewPowerupEvent?.Invoke(this, new PlaceValueEventArgs<Type>(col, row, ttype)));
                    break;
                case UIElementType.Explosion:
                    col = int.Parse(raw[1]);
                    row = int.Parse(raw[2]);
                    Application.Current.Dispatcher.Invoke(() => ExplosionEvent?.Invoke(this, new PlaceEventArgs(col, row)));
                    break;
                case UIElementType.RemovedElement:
                    col = int.Parse(raw[1]);
                    row = int.Parse(raw[2]);
                    id = int.Parse(raw[3]);
                    Application.Current.Dispatcher.Invoke(() => RemovedUIElement?.Invoke(this, new PlaceValueEventArgs<int>(col, row, id)));
                    break;
                case UIElementType.Monster:
                    col = int.Parse(raw[1]);
                    row = int.Parse(raw[2]);
                    Application.Current.Dispatcher.Invoke(() => NewMonsterEvent?.Invoke(this, new PlaceValueEventArgs<(string,int)>(col, row, (raw[3], int.Parse(raw[4])))));
                    break;
                default:
                    thisAsInterface.Log("Unhandled UIElement");
                    break;
            }
        }
    }
}
