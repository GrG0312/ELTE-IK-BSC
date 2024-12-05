using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bomberman_Prototype1.Model;
using Bomberman_Prototype1.Model.Entities;
using Bomberman_Prototype1.Model.CustomEventArgs;
using Bomberman_Prototype1.Model.Entities.Powerups;
using Bomberman_Prototype1.Persistence;
using System.Net.Sockets;
using System.Diagnostics;
using System.Windows.Controls.Ribbon;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using Bomberman_Prototype1.Network;
using Bomberman_Prototype1.Model.Entities.Monsters;
using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Threading;

namespace Bomberman_Prototype1.ViewModel
{
    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isGhost = System.Convert.ToBoolean(value);
            if (isGhost)
                return 0.5;
            else { return 1.0; }
        }
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public struct Params
    {
        public int PlayerNum { get; private set; }
        public int RoundNum { get; private set; }
        public int MapID { get; private set; }
        public bool IsBtr { get; internal set; }
        public Params(int pn, int rn, int m, bool isbr)
        {
            PlayerNum = pn;
            RoundNum = rn;
            MapID = m;
            IsBtr = isbr;
        }
    }
    public struct Result
    {
        private bool isWinner;
        public int PlayerID { get; private set; }
        public string Name { get; private set; }
        public int Wins { get; private set; }
        public string WritePlayerID {  get; private set; }
        public string WriteName {  get; private set; }
        public string WriteWins {  get; private set; }
        public string IsVisible { get { if (isWinner) { return "Visible"; } else { return "Hidden"; } } }
        public Result(int p, string n, int w)
        {
            PlayerID = p;
            WritePlayerID = "PlayerID: " + p.ToString();
            Name = n;
            WriteName = "Name: " + n;
            Wins = w;
            WriteWins = "Wins: "+w.ToString();
            isWinner = false;
        }
        public Result SetIsWinner(bool isWinner)
        {
            return new Result(PlayerID, Name, Wins)
            {
                isWinner = isWinner
            };
        }
    }
    public class ViewModelMain : ViewModelBase
    {

        #region Fields
        private int hostRoundsWon;
        private int fullPlayedTime;

        //Board
        private int _cellsize = 70;
        private ImageSource _backgroundImage = null!;
        private SolidColorBrush _backgroundColor = null!;

        private int? _mapNumber = null;//do we really need this
        /// <summary>
        /// Referenciák a <see cref="ViewModelMain"/>-nek, hogy jó dimenzióba helyezzük le az elemeket
        /// </summary>
        private List<Wall> references = new List<Wall>();
        private GameModel? gameModel;
        private string? ip;
        private Params latest;
        public enum ViewState { MainMenu, JoinGame, HostGame, SettingUpGame, InGame, BrowsingProfiles, CreateProfile, EndScreen, RoundScreen };

        #region Networking
        private BMServer? server;
        private BMClient? client;
        #endregion

        #endregion

        #region Properties
        public ImageSource BackgroundImage{ get { return _backgroundImage; } set { _backgroundImage = value; OnPropertyChanged(); } }
        public CompositeCollection Elements { get; set; }
        public CompositeCollection Results { get; set; }
        public String GameTime { get { return TimeSpan.FromSeconds(gameModel!.GameTime).ToString("g"); } }
        public String? BattleRoyalTime { get; private set; }
        public string IsBattleRoyal { get { if (gameModel!.IsBattleRoyale) { return "Visible"; } else { return "Hidden"; }; } }
        public string IsNotBattleRoyal { get { if (IsBattleRoyal == "Hidden") { return "Visible"; } else { return "Hidden"; }; } } //kell hogy szép legyen

        //View-ViewModel
        public Profile? CurrentProfile { get; set; }
        public string? CurrentPictureForNewProfile {  get; set; }
        public string IPAddress 
        {
            get { return ip!; }
            set
            {
                if (ip != value)
                {
                    ip = value;
                    OnPropertyChanged(nameof(IPAddress));
                }
            }
        }
        #region Commands
        public DelegateCommand CharacterMoveCommand { get; private set; }
        public DelegateCommand PlaceBombCommand { get; private set; }
        public DelegateCommand PutObstacleCommand { get; private set; }
        public DelegateCommand NewGameParamsCommand { get; private set; }
        public DelegateCommand JoinGameCommand { get; private set; }

        #region Profile Navigation and Selection
        public DelegateCommand BrowseProfilesCommand { get; private set; }
        public DelegateCommand SelectProfileCommand { get; private set; }
        public DelegateCommand CancelSelectCommand { get; private set; }
        public DelegateCommand NextProfileCommand { get; private set; }
        public DelegateCommand NextPictureCommand { get; private set; }
        public DelegateCommand CreateProfileCommand { get; private set; }
        public DelegateCommand CreateNewProfileCommand { get; private set; }
        public DelegateCommand CancelCreateCommand { get; private set; }
        #endregion

        #endregion

        #endregion

        #region Events
        public EventHandler<ViewState>? ChangeDisplayedWindow;
        #region Profilok
        public EventHandler<string>? SelectedOrCanceledProfile;
        public EventHandler<string>? CreateOrCanceledProfile;
        public EventHandler<string>? LoadNext;
        #endregion
        public EventHandler<string>? PlayerJoinedLobby;
        #endregion

        #region Constructor(s)
        public ViewModelMain(GameModel model)
        {
            hostRoundsWon = 0;
            fullPlayedTime = 0;

            Elements = new CompositeCollection();
            Results = new CompositeCollection();

            //set up the model
            gameModel = model;
            gameModel.NewGame += NewGameEventHandler;
            gameModel.ExplosionHere += ExplosionDrawEventHandler;
            gameModel.GameAdvanced += Model_AdvancedTimeEventHandler;
            gameModel.PutWall += Model_PutWall;//battleroyal shrink megjelenítés
            gameModel.EntityRemoved += Model_EntityDied;//bármilyen entity ha meghal, el kell tüntetni de csak látványilag
            gameModel.BombPlaced += BombPlacedEventHandler;
            gameModel.PlayerMoved += Model_OnPlayerMoved;
            gameModel.PutObstacle += PlaceBlocker;
            gameModel.GameOver += Model_GameOverEventHandler;

            //set up the commands
            NewGameParamsCommand = new DelegateCommand(param => 
            { 
                ChangeDisplayedWindow?.Invoke(this, ViewState.SettingUpGame); 
            });
            JoinGameCommand = new DelegateCommand(param => 
            { 
                ChangeDisplayedWindow?.Invoke(this, ViewState.JoinGame);
            });
            BrowseProfilesCommand = new DelegateCommand(param => 
            { 
                ChangeDisplayedWindow?.Invoke(this, ViewState.BrowsingProfiles); 
            });

            //Ez a kettő vonatkozik az OK és CANCEL gombokra a profilok böngészésére való oldalon
            SelectProfileCommand = new DelegateCommand(param =>
            {
                SelectedOrCanceledProfile!.Invoke(this, "selected");
                OnPropertyChanged(nameof(CurrentProfile));
                //save selected Profile
            });
            CancelSelectCommand = new DelegateCommand(param =>
            {
                SelectedOrCanceledProfile!.Invoke(this, "canceled");
            });

            //Ez a kettő az OK és CANCEL gombok funkciói a profil készítésnél
            CreateNewProfileCommand = new DelegateCommand(param =>
            {
                CreateOrCanceledProfile!.Invoke(this, "create");
            });
            CancelCreateCommand = new DelegateCommand(param =>
            {
                CreateOrCanceledProfile!.Invoke(this, "canceled");
            });

            NextProfileCommand = new DelegateCommand(param =>
            {
                LoadNext!.Invoke(this, "profile");
                OnPropertyChanged(nameof(CurrentProfile));
            });
            NextPictureCommand = new DelegateCommand(param =>
            {
                LoadNext!.Invoke(this, "picture");
                OnPropertyChanged(nameof(CurrentPictureForNewProfile));
            });

            //Ez változtatja az ablakot Profile Browsingról Profile Creatingre
            CreateProfileCommand = new DelegateCommand(param => { ChangeDisplayedWindow?.Invoke(this, ViewState.CreateProfile); });

            CharacterMoveCommand = new DelegateCommand(param =>
            {
                int temp = Convert.ToInt32(param);
                Direction e = (Direction)temp;
                if (server != null)
                {
                    gameModel.ControlPlayer(0, e);
                } else
                {
                    ((IClient)client!).SendDataToServer(MessageType.Move, $"{e}");
                }
            });
            PlaceBombCommand = new DelegateCommand(param =>
            {
                if (server != null)
                {
                    gameModel.ControlPlayer(0, "bomb");
                } else
                {
                    ((IClient)client!).SendDataToServer(MessageType.Action, $"bomb");
                }
            });
            PutObstacleCommand = new DelegateCommand(param =>
            {
                if (server != null)
                {
                    gameModel.ControlPlayer(0, "obstacle");
                } else
                {
                    ((IClient)client!).SendDataToServer(MessageType.Action, $"obstacle");
                }
            });
        }
        #endregion

        #region Public Methods
        public async void HostGame(int playerNum,int roundNum, int mapID, bool isBattleRoyale)
        {
            hostRoundsWon = 0;
            fullPlayedTime = 0;

            ChangeDisplayedWindow?.Invoke(this, ViewState.HostGame);

            await Task.Delay(5);

            server = new BMServer();
            server.MovePlayer += Server_MovePlayer;
            server.PlaceThing += (object? sender, (int, string) args) => 
            gameModel!.ControlPlayer(args.Item1, args.Item2);

            IPAddress = server.GetLocalIPAddress();

            server.PlayerConnected += Server_OnPlayerJoin;
            await Task.Run( () => {
                server.AcceptClientConnections(playerNum - 1, isBattleRoyale);
                Thread.Sleep(10);
                });

            Player p = gameModel!.AddPlayerToGame(0, CurrentProfile!);
            for (int i = 0; i < server.Clients.Count; i++)
            {
                p = gameModel!.AddPlayerToGame(i + 1, ((BMClient)server.Clients[i]).PlayerProfile!);
            }

            latest = new Params(playerNum, roundNum, mapID, isBattleRoyale);
            StartNewRound(roundNum);
        }
        public void Shutdown()
        {
            if (server != null)
            {
                server.Shutdown();
            }
        }
        public void JoinGame(string ipAddress)
        {
            client = new BMClient(new TcpClient(), new TcpClient(), CurrentProfile!);
            client.NewBoardEvent += (object? sender, PlaceValueEventArgs<int> e) => { CreateNewBoard(e.Col, e.Row, e.Value); };
            client.NewWallEvent += (object? sender, PlaceValueEventArgs<Field> e) => { ReadField(e.Value, e.Col, e.Row); };
            client.NewPlayerEvent += (object? sender, PlaceValueEventArgs<(Profile, int)> e) => 
            {
                Player p = new Player(e.Col, e.Row, e.Value.Item1, e.Value.Item2);
                p.HasToPlaceBomb += (object? sender, EventArgs e) => { Debug.WriteLine("Placed Bomb"); };
                Application.Current.Dispatcher.Invoke(() => Elements.Add(p));
                DrawAt(new PlaceValueEventArgs<MovingEntity>(e.Col, e.Row, p));
            };
            client.NewMonsterEvent += (object? sender, PlaceValueEventArgs<(string,int)> e) =>
            {
                switch (e.Value.Item1)
                {
                    case "basic":
                        Monster m = new BasicMonster(e.Col, e.Row, "../../../View/Resources/Characters/monster.png", e.Value.Item2,true);
                        Application.Current.Dispatcher.Invoke(() => Elements.Add(m));
                        DrawAt(new PlaceValueEventArgs<MovingEntity>(e.Col, e.Row, m));
                        break;
                    case "ghost":
                        Monster gm = new GhostMonster(e.Col, e.Row, "../../../View/Resources/Characters/monster.png", e.Value.Item2, true);
                        Application.Current.Dispatcher.Invoke(() => Elements.Add(gm));
                        DrawAt(new PlaceValueEventArgs<MovingEntity>(e.Col, e.Row, gm));
                        break;
                    case "seeking":
                        Monster s = new SeekingMonster(e.Col, e.Row, "../../../View/Resources/Characters/monster.png", e.Value.Item2, true);
                        Application.Current.Dispatcher.Invoke(() => Elements.Add(s));
                        DrawAt(new PlaceValueEventArgs<MovingEntity>(e.Col, e.Row, s));
                        break;
                    case "smart":
                        Monster sm = new SmartMonster(e.Col, e.Row, "../../../View/Resources/Characters/monster.png", e.Value.Item2, true);
                        Application.Current.Dispatcher.Invoke(() => Elements.Add(sm));
                        DrawAt(new PlaceValueEventArgs<MovingEntity>(e.Col, e.Row, sm));
                        break;
                    default:
                        break;
                }
            };
            client.PlayerMoved += (object? sender, (int id, Direction dir) e) =>
            {
                if (FindPlayer(e.id, out MovingEntity result))
                {
                    ((Player)result).Move(e.dir);
                    DrawAt(new PlaceValueEventArgs<MovingEntity>(result.Col, result.Row, result));
                }
            };
            client.PlaceObstacle += PlaceBlocker;
            client.PlaceBomb += (object? sender, int id) => 
            {
                if (FindPlayer(id, out MovingEntity target))
                {
                    Bomb? b = ((Player)target).PlaceBomb();
                    if (b != null)
                    {
                        b.BombExploded += (object? sender, PlaceValueEventArgs<Bomb> e) => Application.Current.Dispatcher.Invoke(() =>BombExplodedEventHandler(sender, e));
                        Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(target), b));
                    }
                }
            };
            client.ExplosionEvent += ExplosionDrawEventHandler;
            client.NewPowerupEvent += (object? sender, PlaceValueEventArgs<Type> e) =>
            {
                EffectBase effect = (EffectBase)Activator.CreateInstance(e.Value, e.Col, e.Row)!;//<3 :DDD
                Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(references[e.Row]) - 1, effect));
            };
            client.RemovedUIElement += (object? sender, PlaceValueEventArgs<int> e) => 
            {
                if (FindElement(e.Col, e.Row, out object target))
                {
                    Application.Current.Dispatcher.Invoke(() => Elements.Remove(target));
                    if (target is EffectBase)
                    {
                        if (FindPlayer(e.Value, out MovingEntity player))
                        {
                            ((Player)player).AcquireEffect((EffectBase)target);
                        }
                    }
                }
                else if (FindPlayer(e.Value, out MovingEntity p))
                {
                    Application.Current.Dispatcher.Invoke(() => Elements.Remove((Player)p));
                }
            };
            client.SetBREvent += (object? sender, bool isBr) => { latest.IsBtr = isBr; };
            client.StatsCameInEvent += (object? sender, string results) => ResultHandling(results);
            client.PlaceBlockerEvent += PlaceBlocker;
            client.ChangeTimeEvent += (object? sender, string newTime) =>
            {
                BattleRoyalTime = newTime;
                OnPropertyChanged(nameof(BattleRoyalTime));
            };
            client.GameIsOverEvent += (object? sender, (bool didIWin, int time, string results) args) => 
            { 
                CurrentProfile!.UpdateStats(args.didIWin, args.time);
                Results.Clear();
                ChopUpResults(args.results);
                ChangeDisplayedWindow?.Invoke(this, ViewState.EndScreen);
            };
            client.MonsterMovedEvent += (object? sender, PlaceValueEventArgs<int> e) =>
            {
                if (FindPlayer(e.Value, out MovingEntity result))
                {
                    ((Monster)result).Move(e.Col, e.Row);
                    DrawAt(new PlaceValueEventArgs<MovingEntity>(((Monster)result).Col, ((Monster)result).Row, ((Monster)result)));
                }
            };
            client.ConnectToServer(ipAddress);
            gameModel!.SetBattleRoyal(latest.IsBtr);
        }
        public void StartNewRound(int roundNumber = 0)
        {
            if (roundNumber != 0)
            {
                gameModel!.StartNewGame(latest.PlayerNum, latest.MapID, latest.IsBtr, roundNumber);
            } else
            {
                gameModel!.StartNewGame(latest.PlayerNum, latest.MapID, latest.IsBtr);
            }
        }
        #endregion

        #region Private Methods
        private bool FindPlayer(int id, out MovingEntity target)
        {
            target = null!;
            foreach (var element in Elements)
            {
                if (element is Player)
                {
                    Player p = (element as Player)!;
                    if (p.PlayerID == id)
                    {
                        target = p;
                        return true;
                    }
                }
                if(element is Monster) 
                {
                    Monster m = (element as Monster)!;
                    if (m.MonsterID == id)
                    {
                        target = m;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool FindElement(int col, int row, out object target)
        {
            target = null!;
            for (int i = 0; i < Elements.Count; i++)
            {
                var element = Elements[i];
                if(element is Wall)
                {
                    Wall wall = (Wall)element;
                    if (wall.X == col * 70 && wall.Y == row * 70)
                    {
                        target = wall;
                        return true;
                    }
                }
                else
                if (element is EffectBase)
                {
                    EffectBase effect = (EffectBase)element;
                    if (effect.X == col * 70 && effect.Y == row * 70)
                    {
                        target = effect;
                        return true;
                    }
                }
                else
                if (element is Monster)
                {
                    Monster effect = (Monster)element;
                    if (effect.Col == col * 70 && effect.Row == row * 70)
                    {
                        target = effect;
                        return true;
                    }
                }
                else if(element is Bomb)
                {
                    Bomb effect = (Bomb)element;
                    if (effect.X == col && effect.Y == row)
                    {
                        target = effect;
                        return true;
                    }
                }
            }
            return false;
        }
        private void BreakField(int col, int row)
        {
            if (!FindElement(col, row, out object target)) return;
            if (target is not Wall)
            {
                return;
            }
            Wall wall = (Wall)target;
            switch (wall.Type) 
            {
                case "WeakWall":
                    Application.Current.Dispatcher.Invoke(() => Elements.Remove(wall));
                    break;
                case "Box":
                    if (server != null)
                    {
                        EffectBase latest = gameModel!.GenerateEffect(col, row);
                        ((IServer)server).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.Powerup};{latest.Col};{latest.Row};{latest.SpritePath};{latest.GetType().ToString()}");
                        Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(wall), latest));
                    }
                    Application.Current.Dispatcher.Invoke(() => Elements.Remove(wall));
                    break;
                default: break;
            }
        }
        private void ReadField(Field f, int col, int row)
        {
            Wall wall;
            switch (f)
            {
                case Field.WALL:
                    wall = new Wall(col * _cellsize, row * _cellsize, _cellsize, "Wall", GetSpriteUriForWall() + "tegla.png");
                    Application.Current.Dispatcher.Invoke(() => Elements.Add(wall));
                    if (col * _cellsize == 1260) references.Add(wall);
                    break;
                case Field.WEAK_WALL:
                    wall = new Wall(col * _cellsize, row * _cellsize, _cellsize, "WeakWall", GetSpriteUriForWall() + "torheto.png");
                    Application.Current.Dispatcher.Invoke(() => Elements.Add(wall));
                    break;
                case Field.BOX:
                    wall = new Wall(col * _cellsize, row * _cellsize, _cellsize, "Box", GetSpriteUriForWall() + "doboz.png");
                    Application.Current.Dispatcher.Invoke(() => Elements.Add(wall));
                    break;
                default:
                    break;
            }
        }
        private string GetSpriteUriForWall()
        {
            switch (_mapNumber)
            {
                case 1: return "Resources/Cyber/";
                case 2: return "Resources/Desert/";
                case 3: return "Resources/Swamp/";
                default:
                    return "Resources/Cyber/";
            }
        }
        private void ReadBackground(int type)
        {
            switch (type)
            {
                case 1:
                    _mapNumber = 1;
                    BitmapImage backgroundImage = new BitmapImage(new Uri("../../../View/Resources/Cyber/cyberground.png", UriKind.RelativeOrAbsolute));
                    BackgroundImage = backgroundImage;
                    _backgroundColor= new SolidColorBrush(Colors.Indigo);
                    break;
                case 2:
                    _mapNumber = 2;
                    backgroundImage = new BitmapImage(new Uri("../../../View/Resources/Desert/desertground.png", UriKind.RelativeOrAbsolute));
                    BackgroundImage = backgroundImage;
                    _backgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#d29463")!;
                    break;
                case 3:
                    _mapNumber = 3;
                    backgroundImage = new BitmapImage(new Uri("../../../View/Resources/Swamp/swampground.png", UriKind.RelativeOrAbsolute));
                    BackgroundImage = backgroundImage;
                    _backgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#122e1f")!;
                    break;
                default:
                    Debug.WriteLine("Not found a matching type");
                    break;
            }
        }
        private void DrawAt(PlaceValueEventArgs<MovingEntity> e)
        {
            if(e.Value is Player)
            {
                Application.Current.Dispatcher.Invoke(() => Elements.Remove(e.Value));
                Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(references[((Player)e.Value).Row]) - 1, e.Value));
            }
            else if(e.Value is Monster)
            {
                Application.Current.Dispatcher.Invoke(() => Elements.Remove(e.Value));
                Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(references[((Monster)e.Value).Row]) - 1, e.Value));
            }
        }
        private void CreateNewBoard(int cols, int rows, int type)
        {
            gameModel!.SetTimer(0);
            ChangeDisplayedWindow?.Invoke(this, ViewState.InGame);
            ReadBackground(type);
            Elements.Clear();
            references.Clear();
            if (server != null)
            {
                server.ResumeRead();
            }
            Application.Current.Dispatcher.Invoke(() => Elements.Add(new Board(cols * _cellsize, rows * _cellsize, _backgroundColor)));
        }
        private void ResultHandling(string results)
        {
            //Megjelenítjük az eredményeket
            Results.Clear();
            ChopUpResults(results);
            (int, bool) final = DetermineFinalResult();
            //Ha véget ért a játék
            if (gameModel!.RoundNumber == 0)
            {
                //Üzenetküldés a klienseknek
                //Szervert leállítjuk
                if (server != null)
                {
                    ((IServer)server).SendDataToAllClients(MessageType.Over, $"{final.Item1};{final.Item2};{results}");
                    server.Shutdown(true);
                    CurrentProfile!.UpdateStats(final.Item1 == 0 && !final.Item2, fullPlayedTime);
                }
                //Megjelenítjük a végleges eredményt, lementjük a profilokat, visszadob a főmenüre
                Application.Current.Dispatcher.Invoke( () => ChangeDisplayedWindow?.Invoke(this, ViewState.EndScreen));
            }
            else
            {
                if (server != null)
                {
                    ((IServer)server).SendDataToAllClients(MessageType.StatDisplay, $"{results}");
                }
                //Megjelenítjük az ideiglenes eredményeket, utána úgy játék indul
                Application.Current.Dispatcher.Invoke( () => ChangeDisplayedWindow?.Invoke(this, ViewState.RoundScreen));
            }
        }
        private void ChopUpResults(string results)
        {
            string[] raw = results.Split('@', StringSplitOptions.RemoveEmptyEntries);
            foreach (string stat in raw)
            {
                string[] parts = stat.Split(":");
                Results.Add(new Result(int.Parse(parts[0]), parts[1], int.Parse(parts[2])));
            }
        }
        private (int, bool) DetermineFinalResult()
        {
            int maxWins = ((Result)Results[0]).Wins;
            int winnerId = ((Result)Results[0]).PlayerID;
            bool foundDraw = false;
            for (int i = 1; i < Results.Count; i++)
            {
                if (((Result)Results[i]).Wins > maxWins)
                {
                    maxWins = ((Result)Results[i]).Wins;
                    winnerId = ((Result)Results[i]).PlayerID;
                    foundDraw = false;
                } else if (((Result)Results[i]).Wins == maxWins)
                {
                    foundDraw = true;
                }
            }
            for (int i = 0; i < Results.Count; i++)
            {
                if (((Result)Results[i]).Wins == maxWins)
                {
                    var newResult = ((Result)Results[i]).SetIsWinner(true);
                    Results[i] = newResult;
                }
            }
            return (winnerId, foundDraw);
        }
        #endregion

        #region Eventhandlers
        private void NewGameEventHandler(object? sender, MapEventArgs e)
        {
            Results = new CompositeCollection();
            ((IServer)server!).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.Board};{e.Cols};{e.Rows};{e.Type}");

            CreateNewBoard(e.Cols, e.Rows, e.Type);

            for (int i = 0; i < e.Rows; ++i)
            {
                for (int j = 0; j < e.Cols; ++j)
                {
                    ReadField(e.Map[j, i], j, i);
                    ((IServer)server).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.Wall};{e.Map[j, i]};{j};{i}");
                }
            }
            
            foreach (Player player in e.Players)
            {
                ((IServer)server).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.Player};{player.Col};{player.Row};{player.Profile};{player.PlayerID}");
                Application.Current.Dispatcher.Invoke(() => Elements.Add(player));
                DrawAt(new PlaceValueEventArgs<MovingEntity>(player.Col, player.Row, player));
            }
            foreach (Monster m in e.Monsters)
            {
                m.MonsterMoved += (object? sender, PlaceValueEventArgs<MovingEntity> e) =>
                {
                    ((IServer)server).SendDataToAllClients(MessageType.MonsterMoved, $"{e.Col};{e.Row};{((Monster)e.Value).MonsterID}");
                    if (server != null)
                    {
                        DrawAt(e);
                    }
                };
                ((IServer)server).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.Monster};{m.Col};{m.Row};{m.Type};{m.MonsterID}");
                Application.Current.Dispatcher.Invoke(() => Elements.Add(m));
                DrawAt(new PlaceValueEventArgs<MovingEntity>(m.Col, m.Row, m));
            }
        }
        private void BombPlacedEventHandler(object? sender, PlaceValueEventArgs<(Player player, Bomb bomb)> e)
        {
            Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(e.Value.player), e.Value.bomb));
            if (server != null)
            {
                ((IServer)server).SendDataToAllClients(MessageType.Action, $"bomb;{e.Value.player.PlayerID}");
            }
            e.Value.bomb.BombExploded += BombExplodedEventHandler;
            e.Value.bomb.BombExploded += gameModel!.ExplosionStart;
        }
        private void BombExplodedEventHandler(object? sender, PlaceValueEventArgs<Bomb> e)
        {
            int ind = Elements.IndexOf(e.Value);
            
            if(-1 < ind && ind < Elements.Count)
            {
                Application.Current.Dispatcher.Invoke(() => Elements.RemoveAt(ind));
                if (server !=null)
                {
                    ((IServer)server).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.RemovedElement};{e.Col};{e.Row};{-1}");
                }
            }
        }
        private void ExplosionDrawEventHandler(object? sender, PlaceEventArgs e)
        {
            if (server != null)
            {
                ((IServer)server).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.Explosion};{e.Col};{e.Row}");
            }
            Explosion exp = new Explosion(e.Col * 70, e.Row * 70 );
            BreakField(e.Col, e.Row);
            exp.ExplosionEnd += ExplosionEndEventHandler;
            FindElement(e.Col, e.Row + 1, out object target);
            if (target is not null && target is Wall)
            {
                Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(target) - 1, exp));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => Elements.Add(exp));
            }
        }
        private void ExplosionEndEventHandler(object? sender, EventArgs _)
        {
            if(sender != null && sender is Explosion)
            {
                Application.Current.Dispatcher.Invoke( () => Elements.Remove((Explosion)sender));
            }
        }
        //leteszi a falat vagy dobozt, ha a player le tudta tenni
        private void PlaceBlocker(object? sender, PlaceValueEventArgs<Field> e)
        {
            Wall? newWall = null;
            switch (e.Value)
            {
                case Field.WALL:
                    newWall = new Wall(e.Col * _cellsize, e.Row * _cellsize, _cellsize, "Wall", GetSpriteUriForWall() + "tegla.png");//maybe
                    break;
                case Field.WEAK_WALL:
                    newWall = new Wall(e.Col * _cellsize, e.Row * _cellsize, _cellsize, "WeakWall", GetSpriteUriForWall() + "torheto.png");
                    break;
                case Field.BOX:
                    newWall = new Wall(e.Col * _cellsize, e.Row * _cellsize, _cellsize, "Box", GetSpriteUriForWall() + "doboz.png");
                    break;
                default:
                    break;
            }
            Debug.WriteLine($"Inserting wall here:\n\t{e.Col},{e.Row}");
            Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(references[e.Row]) - 1, newWall));
            if (server != null)
            {
                ((IServer)server).SendDataToAllClients(MessageType.Action, $"obstacle;{e.Col};{e.Row};{e.Value}");
            }
        }
        private void Model_AdvancedTimeEventHandler(object? sender, (int defTime, int brTime) timers)
        {
            if (server != null)
            {
                ((IServer)server).SendDataToAllClients(MessageType.Timer, $"{timers.brTime.ToString("g")}");
                BattleRoyalTime = timers.brTime.ToString("g");
            }
            OnPropertyChanged(nameof(GameTime));
            OnPropertyChanged(nameof(BattleRoyalTime));
        }
        private void Model_PutWall(object? sender, PlaceEventArgs coordinates) 
        {
            if (FindElement(coordinates.Col, coordinates.Row, out object target))
            {
                Application.Current.Dispatcher.Invoke(() => Elements.Remove(target));
            }
            Application.Current.Dispatcher.Invoke(() => Elements.Insert(Elements.IndexOf(references[coordinates.Row]) - 1, new Wall(coordinates.Col * _cellsize, coordinates.Row * _cellsize, _cellsize, "Wall", GetSpriteUriForWall() + "tegla.png")));
            ((IServer)server!).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.RemovedElement};{coordinates.Col};{coordinates.Row};{-1}");
            Debug.WriteLine($"Server: Placing walls here:\n\t{coordinates.Row},{coordinates.Col}");
            ((IServer)server).SendDataToAllClients(MessageType.Shrink, $"{Field.WALL};{coordinates.Col};{coordinates.Row}");
        }
        private void Model_EntityDied(object? sender, (Entity, int) e)
        {
            Application.Current.Dispatcher.Invoke(() => Elements.Remove(e.Item1));
            if (server != null)
            {
                ((IServer)server).SendDataToAllClients(MessageType.UIElement, $"{UIElementType.RemovedElement};{e.Item1.Col};{e.Item1.Row};{e.Item2}");
            }
        }
        private void Server_OnPlayerJoin(object? sender, string msg)
        {
            PlayerJoinedLobby?.Invoke(this, msg);
        }
        private void Server_MovePlayer(object? sender, (int, Direction) e)
        {
            gameModel!.ControlPlayer(e.Item1, e.Item2);
        }
        private void Model_OnPlayerMoved(object? sender, PlaceValueEventArgs<(MovingEntity, Direction)> e)
        {
            ((IServer)server!).SendDataToAllClients(MessageType.Move, $"{((Player)e.Value.Item1).PlayerID};{e.Value.Item2}");
            DrawAt(new PlaceValueEventArgs<MovingEntity>(e.Col, e.Row, e.Value.Item1));
        }
        private void Model_GameOverEventHandler(object? sender, GameOverEventArgs w)//:D
        {
            server!.PauseRead();
            fullPlayedTime += w.PlayedTime;
            if (w.WinnerID == 0)
            {
                hostRoundsWon++;
            }
            //Mindenki megkapja a meccs eredményét
            ((IServer)server).SendDataToAllClients(MessageType.RoundResults, $"{w.WinnerID};{gameModel!.GameTime}");
            //Beállítjuk a Host saját eredményét
            string results = $"0:{CurrentProfile!.Name}:{hostRoundsWon}@";//ID:NAME:WON@ID:NAME:WON@...
            //Elkérjük mindenkitől a saját eredményét
            results += server.GetStatsFromClients();
            //Elküldjük mindenkinek a végleges eredményt

            ResultHandling(results);
        }
        #endregion
    }
}
