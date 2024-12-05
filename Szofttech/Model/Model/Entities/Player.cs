using System;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using Bomberman_Prototype1.Model.CustomEventArgs;
using Bomberman_Prototype1.Model.Entities.Powerups;
using Bomberman_Prototype1.Persistence;


namespace Bomberman_Prototype1.Model.Entities
{
    public class Player : MovingEntity
    {
        #region PendingEffect inner class
        private class PendingEffect
        {
            public const int EFFECT_DURATION = 10;

            public int duration;
            public System.Timers.Timer timer { get; private set; }
            public EffectType type { get; private set; }

            public EventHandler<EffectType>? DurationOver;
            public EventHandler<EffectType>? StartFlicker;

            public PendingEffect(EffectType type)
            {
                this.duration = EFFECT_DURATION;
                this.type = type;
                timer = new System.Timers.Timer(1000);
                timer.Elapsed += PendingTick;
            }
            public PendingEffect()
            {
                duration = 9999;
                type = EffectType.SpeedUp;
                timer = new System.Timers.Timer(1000);
            }

            private void PendingTick(object? sender, EventArgs e)
            {
                duration--;
                if (duration <= 0)
                {
                    timer.Stop();
                    DurationOver?.Invoke(this, type);
                }
                if (duration == 2)
                {
                    StartFlicker?.Invoke(this, type);
                }

            }
        }
        #endregion

        #region Fields
        private int maxBombs;
        private int bombRange;
        private int obstacles = 0;
        
        //powerupok miatt, kell régi értéket tárolni
        private int oldBombRange;
        private int oldSpeed;
        private int oldMaxBombs;

        /// <summary>
        /// Stores the time based effects on this Player
        /// </summary>
        private List<PendingEffect> pendingEffects;
        #endregion

        #region Properties
        public Profile Profile { get; private set; }
        public bool CanPlaceObstacle {
            get
            {
                return obstacles > 0;
            }
        }
        public bool HasDetonator { get; private set; }
        public bool IsAlive { get; private set; }
        public bool IsGhost { get; private set; }
        public bool IsInvincible { get; private set; }
        public bool StartFlicker { get; private set; }
        public List<Bomb> Bombs { get; private set; }
        public int PlayerID { get; }
        #endregion

        #region Constructor(s)
        public Player(int col, int row, Profile playerProfile, int id) : base(col, row, playerProfile.SpritePath)
        {
            Bombs = new List<Bomb>();
            pendingEffects = new();
            maxBombs = 1;
            bombRange = 2;
            IsInvincible = false;
            StartFlicker = false;
            IsGhost = false;
            pendingEffects = new();
            HasDetonator = false;
            Profile = playerProfile;
            IsAlive = true;
            PlayerID = id;
        }
        #endregion

        #region Events
        public EventHandler? HasToPlaceBomb;
        #endregion

        #region Public methods
        public void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.UP:
                    Row--;
                    break;
                case Direction.DOWN:
                    Row++;
                    break;
                case Direction.LEFT:
                    Col--;
                    break;
                case Direction.RIGHT:
                    Col++;
                    break;
                default:
                    break;
            }
        }
        public void Move(int col, int row)
        {
            this.col = col;
            this.row = row;
            X = col * 70 - 15;
            Y = row * 70;
        }
        public void AcquireEffect(EffectBase e)
        {
            PendingEffect effect;
            switch (e)
            {
                case SpeedUp:
                    ChangeSpeed(14);
                    break;
                case SlowDown:
                    if (SearchEffect(EffectType.SlowDown, out effect))
                    {
                        effect.duration += PendingEffect.EFFECT_DURATION;
                    }
                    else
                    {
                        oldSpeed = moveSpeed;
                        effect.DurationOver += DurationOverHandler;
                        effect.timer.Start();
                        pendingEffects.Add(effect);
                        ChangeSpeed(5);
                    }
                    break;
                case PlusOneRange:
                    if (bombRange == 1)
                    {
                        oldBombRange++;
                    }
                    else
                    {
                        bombRange++;
                    }
                    break;
                case PlusBomb:
                    if (maxBombs == 0)
                    {
                        oldMaxBombs++;
                    }
                    else
                    {
                        maxBombs++;
                    }
                    break;
                case Obstacle:
                    obstacles += 3;
                    break;
                case Ghost:
                    if (SearchEffect(EffectType.Ghost, out effect))
                    {
                        effect.duration += PendingEffect.EFFECT_DURATION;
                    }
                    else
                    {
                        IsGhost = true;
                        effect.DurationOver += DurationOverHandler;
                        effect.StartFlicker += StartFlickerHandler;
                        effect.timer.Start();
                        pendingEffects.Add(effect);
                    }
                    break;
                case Invincible:
                    if (SearchEffect(EffectType.Invincibility, out effect))
                    {
                        effect.duration += PendingEffect.EFFECT_DURATION;
                    }
                    else
                    {
                        IsInvincible = true;
                        effect.DurationOver += DurationOverHandler;
                        effect.StartFlicker += StartFlickerHandler;
                        effect.timer.Start();
                        pendingEffects.Add(effect);
                    }
                    break;
                case OneRange:
                    if (SearchEffect(EffectType.OneRange, out effect))
                    {
                        effect.duration += PendingEffect.EFFECT_DURATION;
                    }
                    else
                    {
                        oldBombRange = bombRange;
                        bombRange = 1;
                        effect.DurationOver += DurationOverHandler;
                        effect.timer.Start();
                        pendingEffects.Add(effect);
                    }
                    break;
                case InstaBomb:
                    if (SearchEffect(EffectType.InstaPlaceBombs, out effect))
                    {
                        effect.duration += PendingEffect.EFFECT_DURATION;
                    }
                    else
                    {
                        effect.DurationOver += DurationOverHandler;
                        effect.timer.Elapsed += InstaPlaceBomb;
                        effect.timer.Start();
                        pendingEffects.Add(effect);
                    }
                    break;
                case NoBomb:
                    if (SearchEffect(EffectType.NoBombs, out effect))
                    {
                        effect.duration += PendingEffect.EFFECT_DURATION;
                    }
                    else
                    {
                        oldMaxBombs = maxBombs;
                        maxBombs = 0;
                        effect.DurationOver += DurationOverHandler;
                        effect.timer.Start();
                        pendingEffects.Add(effect);
                    }
                    break;
                case Detonator:
                    if (SearchEffect(EffectType.Detonator, out effect))
                    {
                        effect.duration += PendingEffect.EFFECT_DURATION;
                    }
                    else
                    {
                        HasDetonator = true;
                        ChangeBombsDetonation();
                        effect.DurationOver += DurationOverHandler;
                        effect.timer.Start();
                        pendingEffects.Add(effect);
                    }
                    break;
                default: break;
            }
        }

        //ha igaz, hogy van ilyen powerupja a playernek, le tud helyezni maga alá egy falat, 1/5 eséllyel dobozt
        public void DecrementAvailableObstacleCount()
        {
            if(obstacles!=0)
            {
                 obstacles--;
            }
        }
        public Bomb? PlaceBomb()
        {
            if (Bombs.Count >= maxBombs)
            {
                if (HasDetonator)
                {
                    DetonateBombs();
                }
                return null;
            }
            Bomb bomb = new Bomb(Col, Row, bombRange, HasDetonator);
            Bombs.Add(bomb);
            bomb.BombExploded += PlayersBombExploded;
            return bomb;//talán gond
        }
        public void Live(bool state = false)
        {
            IsAlive = state;
        }
        #endregion

        #region Private methods
        private void PlayersBombExploded(object? sender, PlaceValueEventArgs<Bomb> e)
        {
            Bombs.Remove(e.Value);
        }
        private bool SearchEffect(EffectType type, out PendingEffect effect)
        {
            effect = new PendingEffect(type);
            foreach (var item in pendingEffects)
            {
                if (item.type == type) { effect = item; return true; }
            }
            return false;
        }
        private void InstaPlaceBomb(object? sender, EventArgs e)
        {
            HasToPlaceBomb!.Invoke(this, EventArgs.Empty);
        }
        private void StartFlickerHandler(object? sender, EffectType type)
        {
            StartFlicker = true;
            OnPropertyChanged(nameof(StartFlicker));
        }
        private void DurationOverHandler(object? sender, EffectType type)
        {
            SearchEffect(type, out PendingEffect effect);
            pendingEffects.Remove(effect);
            switch (type)
            {
                case EffectType.SlowDown:
                    ChangeSpeed(oldSpeed);
                    break;
                case EffectType.Ghost:
                    StartFlicker = false;
                    IsGhost = false;
                    OnPropertyChanged(nameof(IsGhost));
                    break;
                case EffectType.OneRange:
                    bombRange = oldBombRange;
                    break;
                case EffectType.Invincibility:
                    StartFlicker = false;
                    IsInvincible = false;
                    OnPropertyChanged(nameof(IsInvincible));
                    break;
                case EffectType.NoBombs:
                    maxBombs = oldMaxBombs;
                    break;
                case EffectType.Detonator:
                    HasDetonator = false;
                    ChangeBombsDetonation();
                    break;
                case EffectType.InstaPlaceBombs:
                    effect.timer.Elapsed -= InstaPlaceBomb;
                    break;
                default:
                    break;
            }
        }
        private void DetonateBombs()
        {
            int count = Bombs.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (Bombs.Count == 0||Bombs.Count<i) return;
                Bombs[i].ExplodeNow();
            }
        }
        private void ChangeBombsDetonation()
        {
            foreach (Bomb bomb in Bombs)
            {
                bomb.ChangeDetonationMode();
            }
        }
        #endregion
    }
}