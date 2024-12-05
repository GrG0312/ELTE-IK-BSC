using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Bomberman_Prototype1.Model.CustomEventArgs;
using Bomberman_Prototype1.Model.Entities;
using Bomberman_Prototype1.Model.Entities.Monsters;
using Bomberman_Prototype1.Model.Entities.Powerups;
using Bomberman_Prototype1.Persistence;

namespace Bomberman_Prototype1.Model
{
    public class GameModel
    {
        #region Fields
        /// <summary>
        /// The number of seconds between two map-shrinkings
        /// </summary>
        private const int BATTLE_ROYALE_TIME = 30;

        private Field[,] map;
        private int playerNum;

        private int alivePlayers;
        private int drawTime;

        private int cols = 19;
        private int rows = 11;

        private int gameTimer;
        private int battleRoyaleTimer;

        private int numberOfShrinks;

        private List<EffectBase>? pendingEffects;

        private static List<Func<int, int, EffectBase>> Effects = new()
        {
            {   (int x, int y)    => new SpeedUp(x, y)      },
            {   (int x, int y)    => new SlowDown(x, y)     },
            {   (int x, int y)    => new PlusOneRange(x, y) },
            {   (int x, int y)    => new PlusBomb(x, y)     },
            {   (int x, int y)    => new Obstacle(x, y)     },
            {   (int x, int y)    => new Ghost(x, y)        },
            {   (int x, int y)    => new Invincible(x, y)   },
            {   (int x, int y)    => new InstaBomb(x, y)    },
            {   (int x, int y)    => new NoBomb(x, y)       },
            {   (int x, int y)    => new Detonator(x, y)    },
        };
        private static Dictionary<int, string> maps = new()
        {
            { 1, "../../../Maps/map1.txt" },
            { 2, "../../../Maps/map2.txt" },
            { 3, "../../../Maps/map3.txt" }
        };

        /// <summary>
        /// Spawn points for players
        /// </summary>
        private static List<(int col, int row)> spawnPoints = new() {
            (3, 1),
            (16,2),
            (9, 9)
        };
        private static List<(int col, int row)> monsterSP = new()
        {
            (3,7)
        };

        // bomba rombbanás hatásához kell hozzáférés a játékban lévő entitykhez
        private List<Player>? players;
        private List<Monster>? monsters;
        public List<Entity>? entities;

        public IMapLoader MapLoader;
        #endregion

        #region Properties
        public int GameTime { get { return gameTimer; } private set { gameTimer = value; } }
        public int BattleRoyalTime { get { return battleRoyaleTimer; } private set { battleRoyaleTimer = value; } }
        public bool IsBattleRoyale { get; private set; }
        public int RoundNumber { get; private set; }
        #endregion

        #region Events
        public EventHandler<MapEventArgs>? NewGame;
        public EventHandler<PlaceEventArgs>? ExplosionHere;
        public EventHandler<(int, int)>? GameAdvanced;
        public EventHandler<PlaceEventArgs>? PutWall; //battleroyalhoz kell, hogy viewmodel is letegye a falat
        public EventHandler<(Entity, int)>? EntityRemoved;//ha meghal egy entity kell tudnia a viewmodelnek

        //Events for Player handling
        public EventHandler<PlaceValueEventArgs<(MovingEntity,Direction)>>? PlayerMoved;
        public EventHandler<PlaceValueEventArgs<Field>>? PutObstacle;
        public EventHandler<PlaceValueEventArgs<(Player, Bomb)>>? BombPlaced;

        public EventHandler<GameOverEventArgs>? GameOver; //első paramater -1 ha döntetlen, egyébként winnerID
        #endregion

        #region Constructor(s)
        public GameModel()
        {
            //19x11
            map = new Field[0,0];
            MapLoader = new MapLoader();
            MapLoader.MapLoaded += LoadMapIntoModel;
            drawTime = -1;
            playerNum = -1;
            alivePlayers = -1;
            players = new List<Player>();
        }

        public GameModel(IMapLoader mapLoader)
        {
            //19x11
            map = new Field[0, 0];
            MapLoader = mapLoader;
            MapLoader.MapLoaded += LoadMapIntoModel;
            drawTime = -1;
            playerNum = -1;
            alivePlayers = -1;
            players = new List<Player>();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// The method that will start a new game
        /// </summary>
        /// <param name="playerNumber">how many spawn points should be used</param>
        /// <param name="mapID">ID of predefined map</param>
        /// <param name="isBattleRoyale">true if game should use "Battle Royale" mode, false if not</param>
        public void StartNewGame(int playerNumber,int mapID, bool isBR, int roundNumber = 0)
        {
            //Player number:    hány spawnpointra kell majd playereket rakni
            //MapID:            majd a boardType-ot változtatja
            //isBattleRoyale:   ha igen, kell majd egy külön timert csinálni a GamePage-en amin látszik, hogy mennyi idő van még az aréna szűküléséig
            drawTime = -1;
            playerNum = playerNumber;
            alivePlayers = playerNumber;
            if (roundNumber != 0)
            {
                RoundNumber = roundNumber;
            }
            SetTimer(0);
            IsBattleRoyale = isBR;
            numberOfShrinks = 0;
            battleRoyaleTimer = BATTLE_ROYALE_TIME;
            MapLoader.LoadMap(mapID);
            pendingEffects = new List<EffectBase>();
            monsters = new List<Monster>();
            entities = new List<Entity>();
            for (int i = 0; i < players!.Count; i++)
            {
                players[i].Move(spawnPoints[i].col, spawnPoints[i].row);
                players[i].Live(true);
                players[i].HasToPlaceBomb += (object? sender, EventArgs e) => { ControlPlayer(((Player)sender!).PlayerID, "bomb"); }; //instantplacebomb effect miatt kell
            }
            monsters.Clear();
            entities.Clear();
            AddMonsterToGame(0, "smart");
            NewGame?.Invoke(this, new MapEventArgs(map, map.GetLength(0), map.GetLength(1), mapID, players,monsters));
        }
        public void SetTimer(int target)
        {
            gameTimer = target;
        }
        public void SetBattleRoyal(bool isBR)
        {
            IsBattleRoyale = isBR;
        }
        public Player AddPlayerToGame(int index, Profile profile)
        {
            Player newGuy = new Player(spawnPoints[index].col, spawnPoints[index].row, profile, index);
            players!.Add(newGuy);
            return newGuy;
        }
        public Monster AddMonsterToGame(int index, string type)
        {
            Monster monster;
            switch (type)
            {
                case "basic":
                    monster = new BasicMonster(monsterSP[index].col, monsterSP[index].row, "../../../View/Resources/Characters/monster.png",monsters!.Count+100);
                    break;
                case "ghost":
                    monster = new GhostMonster(monsterSP[index].col, monsterSP[index].row, "../../../View/Resources/Characters/monster.png", monsters!.Count+100);
                    ((GhostMonster)monster).CheckAheadEvent += Ghost_CheckMove;
                    break;
                case "seeking":
                    monster = new SeekingMonster(monsterSP[index].col, monsterSP[index].row, "../../../View/Resources/Characters/monster.png", monsters!.Count+100);
                    ((SeekingMonster)monster).GetBestDirEvent += Monster_GetBestDir;
                    break;
                case "smart":
                    monster = new SmartMonster(monsterSP[index].col, monsterSP[index].row, "../../../View/Resources/Characters/monster.png", monsters!.Count + 100);
                    ((SmartMonster)monster).GetBestDirEvent += Monster_GetBestDir;
                    break;
                default:
                    monster = new BasicMonster(monsterSP[index].col, monsterSP[index].row, "../../../View/Resources/Characters/monster.png", monsters!.Count+100);
                    break;
            }
            monster.MonsterMoved += Monster_OnMonsterMoved;
            monster.GetFieldAhead += Monster_OnGetFieldAhead;
            monsters.Add(monster);
            return monster;
        }
        /// <summary>
        /// Executes a <see cref="Player"/>'s method depending on the parameter
        /// </summary>
        /// <param name="playerID">The ID of the player we want to control</param>
        /// <param name="dir">In case of Direction parameter, we execute the Move method with said parameter's value</param>
        public void ControlPlayer(int playerID, Direction dir)
        {
            if(SearchForPlayer(playerID, out Player? player))
            {
                if (player == null || !player.IsAlive)
                {
                    return;
                }
                if (CheckIfLegalMove(dir, player.Col, player.Row, player.IsGhost))
                {
                    player.Move(dir);
                    PlayerMoved!.Invoke(this, new PlaceValueEventArgs<(MovingEntity,Direction)>(player.Col, player.Row, (player, dir)));
                    if (CheckForEffect(player.Col, player.Row, out EffectBase? effect) && effect != null)
                    {
                        player.AcquireEffect(effect);
                        map[player.Col, player.Row] = Field.EMPTY;
                        pendingEffects!.Remove(effect);
                        EntityRemoved?.Invoke(this, (effect, playerID));
                    }
                }
            }
        }
        /// <summary>
        /// Executes a <see cref="Player"/>'s method depending on the parameter
        /// </summary>
        /// <param name="playerID">The ID of the player we want to control</param>
        /// <param name="actionType">There are 2 possibilities: bomb and obstacle</param>
        public void ControlPlayer(int playerID, string actionType)
        {
            if (SearchForPlayer(playerID, out Player? player)&&player!.IsAlive)
            {
                switch (actionType)
                {
                    case "bomb":
                        if(GetFieldType(player.Col, player.Row) == Field.EMPTY)
                        {
                            Bomb? bomb = player!.PlaceBomb();
                            if(bomb is not null)
                            {
                                entities!.Add(bomb);
                                BombPlaced?.Invoke(this, new PlaceValueEventArgs<(Player, Bomb)>(player.Col, player.Row, (player, bomb)));
                            }
                        }
                        break;
                    case "obstacle":
                        if (player.CanPlaceObstacle && (CheckIfLegalMove(Direction.UP, player.Col, player.Row, player.IsGhost) ||
                            CheckIfLegalMove(Direction.DOWN, player.Col, player.Row, player.IsGhost) ||
                            CheckIfLegalMove(Direction.LEFT, player.Col, player.Row, player.IsGhost) ||
                            CheckIfLegalMove(Direction.RIGHT, player.Col, player.Row, player.IsGhost)))
                            {
                                Random random = new Random();
                                int choice = random.Next(1, 6);
                                Field type = choice == 5 ? Field.BOX : Field.WEAK_WALL;
                                PlaceBlocker(player.Col, player.Row, type);
                                player!.DecrementAvailableObstacleCount();
                                PutObstacle!.Invoke(this, new PlaceValueEventArgs<Field>(player.Col, player.Row, type));
                            }
                        break;
                    default:
                        break;
                }
            }
        }
        public void AdvanceTime(object? sender, EventArgs e)
        {
            gameTimer++;
            if (drawTime != -1)
            {
                if (drawTime + 5 == gameTimer)
                {
                    int winnerID = SearchLastPlayer();
                    RoundNumber--;
                    GameOver?.Invoke(this, new GameOverEventArgs(winnerID, gameTimer));
                }
            }
            if (IsBattleRoyale && numberOfShrinks < 6)
            {
                if(numberOfShrinks < 5)
                    battleRoyaleTimer--;
                if (battleRoyaleTimer == 0)
                {
                    //do battleroyale stuff
                    ShrinkMap();
                    if (numberOfShrinks < 5)
                    {
                        battleRoyaleTimer = BATTLE_ROYALE_TIME;
                    }
                }
            }
            GameAdvanced!.Invoke(this, (gameTimer, battleRoyaleTimer));
        }
        public bool CheckIfLegalMove(Direction dir, int X, int Y, bool isGhost)
        {
            if (isGhost)
            {
                return CheckGhost(dir, X, Y);
            }
            return CheckNormalPlayer(dir, X, Y);
        }
        
        //obstacle powerup hatására
        public bool PlaceBlocker(int col, int row, Field type)
        {
            if (map[col, row] == Field.EMPTY)
            {
                map[col,row] = type;
                return true;
            }
            return false;
        }
        public bool CheckForEffect(int col, int row, out EffectBase? effect)
        {
            effect = null;
            for (int i = 0; i < pendingEffects!.Count; i++)
            {
                if (pendingEffects[i].Col == col && pendingEffects[i].Row == row) { effect = pendingEffects[i]; return true; }
            }
            return false;
        }
        // kiveszi a robbanó bombát és robbanást indít el minden irányba
        public void ExplosionStart(object? sender, PlaceValueEventArgs<Bomb> e)
        {
            entities!.Remove(e.Value);
            Explosion(e.Value.Col, e.Value.Row, e.Value.ExplosionRange, Direction.UP);
            Explosion(e.Value.Col, e.Value.Row, e.Value.ExplosionRange, Direction.DOWN);
            Explosion(e.Value.Col, e.Value.Row, e.Value.ExplosionRange, Direction.LEFT);
            Explosion(e.Value.Col, e.Value.Row, e.Value.ExplosionRange, Direction.RIGHT);
        }
        /// <summary>
        /// Random generál egy új effectet, beleteszi a pendingEffects-be, majd visszaadja
        /// </summary>
        /// <param name="x">Melyik oszlopban vagyunk</param>
        /// <param name="y">Melyik sorban vagyunk</param>
        /// <returns></returns>
        public EffectBase GenerateEffect(int x, int y)
        {
            Random r = new Random();
            int roll = r.Next(0, Effects.Count());

            Func<int, int, EffectBase> executable = Effects.ElementAt(roll);
            EffectBase newEffect = executable(x, y);

            //kell tudnia a modelnek is, h hol van effect field
            map[x, y] = Field.EFFECT;

            pendingEffects!.Add(newEffect);
            return newEffect;
        }

        /// <summary>
        /// Returns the field at the target location
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public Field GetFieldType(int col, int row)
        {
            return map[col, row];
        }
        // a GhostMonster-hez kell
        public bool StraightMoveCheck(int col, int row, Direction dir)
        {
            if (GetNeighbour(col, row, dir) == Field.EMPTY)
            {
                return true;
            }
            else if (0 < col && col < cols - 1 && 0 < row && row < rows - 1) // a legszélső mezőkön nem megy tovább
            {
                switch (dir)
                {
                    case Direction.UP:
                        if (row < 2) return false;
                        else return false || StraightMoveCheck(col, row - 1, dir);
                    case Direction.DOWN:
                        if (rows - 3 < row) return false;
                        else return false || StraightMoveCheck(col, row + 1, dir);
                    case Direction.LEFT:
                        if (col < 2) return false;
                        else return false || StraightMoveCheck(col - 1, row, dir);
                    case Direction.RIGHT:
                        if (cols - 3 < col) return false;
                        else return false || StraightMoveCheck(col + 1, row, dir);
                    default: return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Felrobbantja a jelenlegi mezőt, és ha tudja, továbbküldi a robbanást
        /// </summary>
        /// <param name="x">Melyik oszlopában vagyunk a mapnak</param>
        /// <param name="y">Melyik sorában vagyunk a mapnak</param>
        /// <param name="power">Hány cellával tudja még továbbküldeni a robbanást</param>
        /// <param name="dir">Az irány, amerre halad</param>
        private void Explosion(int x, int y, int power, Direction dir)
        {
            if (map[x, y] == Field.WEAK_WALL)
            {
                ExplosionHere?.Invoke(this, new PlaceEventArgs(x, y));
                map[x, y] = Field.EMPTY;
                return;
            }
            else if (map[x, y] == Field.BOX)
            {
                ExplosionHere?.Invoke(this, new PlaceEventArgs(x, y));
                return;
            }
            ExplosionHere?.Invoke(this, new PlaceEventArgs(x, y));

            LocationExploded(x, y);
            if (power <= 0 || GetNeighbour(x, y, dir) == Field.WALL) return;

            System.Timers.Timer wait = new System.Timers.Timer(100);
            wait.Elapsed += (sender, e) =>
            {
                wait.Stop();
                switch (dir)
                {
                    case Direction.UP:
                        Explosion(x, y - 1, power - 1, dir);
                        break;
                    case Direction.DOWN:
                        Explosion(x, y + 1, power - 1, dir);
                        break;
                    case Direction.LEFT:
                        Explosion(x - 1, y, power - 1, dir);
                        break;
                    case Direction.RIGHT:
                        Explosion(x + 1, y, power - 1, dir);
                        break;
                }
            };
            wait.Start();
        }
        /// <summary>
        /// Visszaadja az iránytól függően, hogy milyen típusú cella van mellettünk
        /// </summary>
        /// <param name="x">Oszlopszám / Hanyadik oszlopban vagyunk</param>
        /// <param name="y">Sorszám / Hanyadik sorban vagyunk</param>
        /// <param name="dir">Milyen irányba nézzük a szomszédot</param>
        /// <returns></returns>
        private Field? GetNeighbour(int x, int y, Direction dir)
        {
            switch(dir)
            {
                case Direction.LEFT:
                    return map[x - 1, y];
                case Direction.RIGHT:
                    return map[x + 1, y];
                case Direction.UP:
                    return map[x, y - 1];
                case Direction.DOWN:
                    return map[x, y + 1];
                default:
                    return null;
            }
        }
        private void CheckIfDraw()
        {
            if(alivePlayers == 1)
            {
                drawTime = gameTimer;
            }
            else if (alivePlayers == 0)
            {
                RoundNumber--;
                GameOver?.Invoke(this, new GameOverEventArgs(-1, gameTimer));
            }
        }
        private bool CheckNormalPlayer(Direction dir, int X, int Y)
        {
            switch (dir)
            {
                case Direction.UP:
                    return (map[X, Y - 1] == Field.EMPTY
                        || map[X, Y - 1] == Field.EFFECT) && !IsThereAPlayer(X,Y-1);
                case Direction.DOWN:
                    return (map[X, Y + 1] == Field.EMPTY
                        || map[X, Y + 1] == Field.EFFECT) && !IsThereAPlayer(X, Y + 1);
                case Direction.LEFT:
                    return (map[X - 1, Y] == Field.EMPTY
                        || map[X - 1, Y] == Field.EFFECT) && !IsThereAPlayer(X-1, Y);
                case Direction.RIGHT:
                    return (map[X + 1, Y] == Field.EMPTY
                        || map[X + 1, Y] == Field.EFFECT) && !IsThereAPlayer(X+1, Y);
                default:
                    return false;
            }
        }
        //eldönti, hogy egy adott fielden van-e player-> hogy ne tudjon más playerre lépni (nincs modelben tárolva a mapon)
        private bool IsThereAPlayer(int x, int y)
        {
            foreach (var item in players!)
            {
                if(item.Col==x && item.Row == y)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckGhost(Direction dir, int X, int Y)
        {
            switch (dir)
            {
                case Direction.UP:
                    if (Y == 1 || IsThereAPlayer(X, Y - 1)) return false; break;
                case Direction.DOWN:
                    if (Y == 9 || IsThereAPlayer(X, Y + 1)) return false; break;
                case Direction.LEFT:
                    if (X == 1 || IsThereAPlayer(X-1, Y)) return false; break;
                case Direction.RIGHT:
                    if (X == 17 || IsThereAPlayer(X+1, Y)) return false; break;
                default:
                    return true;
            }
            return true;
        }
        private int SearchLastPlayer()
        {
            foreach(Player player in players!)
            {
                if (player.IsAlive)
                {
                    return player.PlayerID;
                }
            }
            return -1;
        }

        // a kijelölt helyen lévő entitykre kifejti a robbanás hatását
        private void LocationExploded(int col, int row)
        {
            foreach (Entity entity in entities!)
            {
                if (entity is Bomb && ((Bomb)entity).Col == col && ((Bomb)entity).Row == row)
                {
                    ((Bomb)entity).ExplodeNow();
                }
            }
            foreach (Player player in players!)
            {
                if (player.Col == col && player.Row == row && player.IsAlive && !player.IsInvincible)
                {
                    player.Live(false);
                    alivePlayers--;
                    CheckIfDraw();
                    EntityRemoved?.Invoke(this, (player, player.PlayerID));
                }
            }
            int ind = 0;
            while(ind < monsters!.Count)
            {
                if (monsters[ind].Col == col && monsters[ind].Row == row)
                {
                    Monster m = monsters[ind];
                    monsters.Remove(m);
                    EntityRemoved!.Invoke(this, (m,-1));
                }
                ++ind;
            }
        }
        private void ShrinkMap()
        {
            numberOfShrinks++;
            int rowCount = 1;
            if (numberOfShrinks < 6)
            {
                if (numberOfShrinks == 3 || numberOfShrinks == 4) rowCount = 2;
                else if (numberOfShrinks == 5) rowCount = 3;
                for (int i = rowCount; i < map.GetLength(1)-rowCount; i++)
                {
                    map[numberOfShrinks, i] = Field.WALL;
                    PutWall!.Invoke(this, new PlaceEventArgs(numberOfShrinks,i));
                    LocationExploded(numberOfShrinks, i);
                    map[18-numberOfShrinks, i] = Field.WALL;
                    PutWall!.Invoke(this, new PlaceEventArgs(18-numberOfShrinks,i));
                    LocationExploded(18 - numberOfShrinks,i);
                }
                if (numberOfShrinks % 2 == 0)
                {
                    for (int i = numberOfShrinks+1; i < map.GetLength(0)-numberOfShrinks; i++)
                    {
                        map[i, rowCount] = Field.WALL;
                        PutWall!.Invoke(this, new PlaceEventArgs(i,rowCount));
                        LocationExploded(i, rowCount);
                        map[i, 10-rowCount] = Field.WALL;
                        PutWall!.Invoke(this, new PlaceEventArgs(i,10-rowCount));
                        LocationExploded(i, 10-rowCount);
                    }
                }
            }
        }
        private bool SearchForPlayer(int playerId, out Player? player)
        {
            player = null;
            for (int i = 0; i < players!.Count; i++)
            {
                if (players.ElementAt(i) is Player)
                {
                    Player target = players.ElementAt(i);
                    if (target.PlayerID == playerId)
                    {
                        player = target;
                        return true;
                    }
                }
            }
            return false;
        }

        //visszaadja, hogy elérhető-e játékos, out-ba, hogy melyik irányban
        private bool SeekAPlayer(int col, int row, out Direction dir)
        {
            Collection<(int col, int row)> fields = new Collection<(int col, int row)>();
            AddPlayerCoords(fields);
            bool found = false;
            int currentsize = fields.Count();
            int prevsize = 0;
            while (!found && currentsize != prevsize)
            {
                prevsize = currentsize;
                found = ExpandOne(fields, (col, row));
                currentsize = fields.Count();
            }
            dir = Direction.UP;
            if (found)
            {
                (int col, int row) prev = fields[currentsize - 1];
                if (prev.col != col)
                {
                    if (prev.col < col)
                    {
                        dir = Direction.LEFT;
                    }
                    else
                    {
                        dir = Direction.RIGHT;
                    }
                }
                else
                {
                    if (prev.row < row)
                    {
                        dir = Direction.UP;
                    }
                    else
                    {
                        dir = Direction.DOWN;
                    }
                }
            }
            return found;
        }

        private void AddPlayerCoords(Collection<(int col, int row)> fields)
        {
            foreach (Player player in players!)
            {
                fields.Add((player.Col, player.Row));   
            }
        }

        private bool ExpandOne(Collection<(int col, int row)> fields, (int col, int row) target)
        {
            int count = fields.Count();
            for (int i = 0; i < count; i++)
            {
                Collection<(int col, int row)> temp = new Collection<(int col, int row)>();
                temp.Add((fields[i].col - 1, fields[i].row));
                temp.Add((fields[i].col + 1, fields[i].row));
                temp.Add((fields[i].col, fields[i].row - 1));
                temp.Add((fields[i].col, fields[i].row + 1));
                RandomOrder<(int col, int row)>(temp);
                foreach ((int col, int row) f in temp)
                {
                    if (f == target)
                    {
                        fields.Add(fields[i]);
                        return true;
                    }
                    else if (map[f.col, f.row] == Field.EMPTY && !fields.Contains(f))
                    {
                        fields.Add(f);
                    }
                }
            }
            return false;
        }

        // puts collection in random order
        private void RandomOrder<T>(Collection<T> coll)
        {
            Random rand = new Random();
            Collection<T> temp = coll;
            for (int i = 0; i < temp.Count; i++)
            {
                int ind = rand.Next(temp.Count);
                coll[i] = temp[ind];
                temp.RemoveAt(ind);
            }
        }

        #endregion

        #region Event Handlers
        // This event handler will be executed when the MapLoader is finished with loading from the file system
        // Current implementation is VERY temporary and should be reworked when a final solution for a clean implementation can be determined
        private void LoadMapIntoModel(object? sender, EventArgs e)
        {
            map = new Field[MapLoader.Map!.GetLength(0), MapLoader.Map.GetLength(1)];
            if (MapLoader.Map != null)
            {
                for (int i = 0; i < MapLoader.Map.GetLength(0); ++i)
                {
                    for (int j = 0; j < MapLoader.Map.GetLength(1); ++j)
                    {
                        map[i, j] = MapLoader.Map[i, j];
                    }
                }
                
                map = MapLoader.Map;

            }
            else
            {
                throw new ArgumentNullException("The map was null in the MapLoader!");
            }
        }

        private void Monster_OnMonsterMoved(object? sender, PlaceEventArgs e)
        {
            foreach (Player player in players!)
            {
                if (player.Col == e.Col && player.Row == e.Row && player.IsAlive && !player.IsInvincible)
                {
                    player.Live(false);
                    alivePlayers--;
                    CheckIfDraw();
                    EntityRemoved?.Invoke(this, (player, player.PlayerID));
                }
            }
        }
        private void Monster_OnGetFieldAhead(object? sender, PlaceValueEventArgs<Direction> e)
        {
            Monster monster = (sender as Monster)!;
            switch (e.Value)
            {
                case Direction.UP:
                    monster.FieldAhead=GetFieldType(e.Col, e.Row - 1);
                    break;
                case Direction.DOWN:
                    monster.FieldAhead = GetFieldType(e.Col, e.Row + 1);
                    break;
                case Direction.LEFT:
                    monster.FieldAhead = GetFieldType(e.Col - 1, e.Row);
                    break;
                case Direction.RIGHT:
                    monster.FieldAhead = GetFieldType(e.Col + 1, e.Row);
                    break;
                default:
                    monster.FieldAhead = Field.WALL;
                    break;
            }
        }

        private void Ghost_CheckMove(object? sender, PlaceValueEventArgs<Direction> e)
        {
            bool allowed = StraightMoveCheck(e.Col, e.Row, e.Value);
            if(sender is GhostMonster)
            {
                ((GhostMonster)sender).CanMove = allowed;
            }
        }

        private void Monster_GetBestDir(object? sender, PlaceEventArgs e)
        {
            Direction bestdir;
            bool found = SeekAPlayer(e.Col, e.Row, out bestdir);
            if(sender is SeekingMonster)
            {
                ((SeekingMonster)sender).SeesPlayer = found;
                if (found)
                {
                    ((SeekingMonster)sender).BestDir = bestdir;
                }
            }
            if (sender is SmartMonster)
            {
                ((SmartMonster)sender).SeesPlayer = found;
                if (found)
                {
                    ((SmartMonster)sender).BestDir = bestdir;
                }
            }
        }

        #endregion
    }
}
