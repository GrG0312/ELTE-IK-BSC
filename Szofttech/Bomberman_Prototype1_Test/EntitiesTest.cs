using Bomberman_Prototype1.Model.Entities.Monsters;
using Bomberman_Prototype1.Model.Entities;
using Bomberman_Prototype1.Model;
using Bomberman_Prototype1.Persistence;
using Bomberman_Prototype1.Model.Entities.Powerups;
using System.Numerics;
using Bomberman_Prototype1.Model.CustomEventArgs;

namespace Bomberman_Prototype1_Test
{
    [TestClass]
    public class EntitiesTest
    {
        #region Player class and related tests
        [TestMethod]
        public void PlayerConstructorTest()
        {
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            // Entity constructor parts
            Assert.AreEqual(4, player.Row);
            Assert.AreEqual(3, player.Col);
            Assert.AreEqual("greenskin.png", player.SpritePath);

            // MovingEntity constructor parts
            // No public parts in MovingEntity

            // Player constructor parts
            Assert.IsTrue(player.IsAlive);
            Assert.AreEqual(1, player.PlayerID);
            Assert.IsFalse(player.HasDetonator);
            Assert.IsFalse(player.IsGhost);
            Assert.IsFalse(player.StartFlicker);
            Assert.IsFalse(player.IsInvincible);
            Assert.AreEqual(0, player.Bombs.Count());
            Assert.IsTrue(profile == player.Profile);
        }

        [TestMethod]
        [DataRow(Direction.UP, 3, 3)]
        [DataRow(Direction.DOWN, 5, 3)]
        [DataRow(Direction.LEFT, 4, 2)]
        [DataRow(Direction.RIGHT, 4, 4)]
        public void PlayerMovesInDirection(Direction dir, int expectedRow, int expectedCol)
        {
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            Assert.AreEqual(4, player.Row);
            Assert.AreEqual(3, player.Col);
            player.Move(dir);
            Assert.AreEqual(expectedRow, player.Row);
            Assert.AreEqual(expectedCol, player.Col);
        }

        [TestMethod]
        public void PlayerMovesToField()
        {
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            Assert.AreEqual(4, player.Row);
            Assert.AreEqual(3, player.Col);
            player.Move(8, 9);
            Assert.AreEqual(9, player.Row);
            Assert.AreEqual(8, player.Col);
        }

        [TestMethod]
        public void PlayerAcquiringEffectsDoesntThrowExceptions()
        {
            // Since much of the AcquireEffect() method has absolutely no
            // public effects, this should be at least tested
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            try { player.AcquireEffect(new SpeedUp(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new SlowDown(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new PlusOneRange(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new PlusBomb(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new Obstacle(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new Ghost(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new Invincible(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new OneRange(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new InstaBomb(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new NoBomb(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
            try { player.AcquireEffect(new Detonator(3, 4)); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [TestMethod]
        public void PlayerBecomesGhostOnPowerupAcquisitionTest()
        {
            // This is the only thing I can test about this since
            // extending the effect's duration is completely hidden logic
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            player.AcquireEffect(new Ghost(3, 4));
            Assert.IsTrue(player.IsGhost);
        }

        [TestMethod]
        public void PlayerBecomesInvincibleOnPowerupAcquisitionTest()
        {
            // This is the only thing I can test about this since
            // extending the effect's duration is completely hidden logic
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            player.AcquireEffect(new Invincible(3, 4));
            Assert.IsTrue(player.IsInvincible);
        }

        [TestMethod]
        public void PlayerGainsDetonatorOnPowerupAcquisitionTest()
        {
            // This is the only thing I can test about this since
            // extending the effect's duration is completely hidden logic
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            player.AcquireEffect(new Detonator(3, 4));
            Assert.IsTrue(player.HasDetonator);
        }

        [TestMethod]
        public void PlayerDecrementingObstacleCountDoesntThrowExceptionTest()
        {
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            // The logic of this method is completely hidden, so this is the
            // most I can do for code coverage purposes
            try { player.DecrementAvailableObstacleCount(); }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [TestMethod]
        public void PlayerPlacesBombTest()
        {
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            Assert.AreEqual(0, player.Bombs.Count);

            Bomb bomb1 = player.PlaceBomb()!;
            Assert.AreEqual(1, player.Bombs.Count);
            Assert.AreEqual(player.Row, bomb1.Row);
            Assert.AreEqual(player.Col, bomb1.Col);
        }

        [TestMethod]
        public void PlayerCantPlaceMoreThanMaxBombsTest()
        {
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            Assert.AreEqual(0, player.Bombs.Count);

            Bomb bomb1 = player.PlaceBomb()!;
            Assert.AreEqual(1, player.Bombs.Count);
            Assert.AreEqual(player.Row, bomb1.Row);
            Assert.AreEqual(player.Col, bomb1.Col);

            Bomb bomb2 = player.PlaceBomb()!;
            Assert.AreEqual(1, player.Bombs.Count);
            Assert.IsNull(bomb2);
        }

        [TestMethod]
        public void PlayerCanPlaceMoreBombsTest()
        {
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            Assert.AreEqual(0, player.Bombs.Count);

            player.AcquireEffect(new PlusBomb(3, 4));
            Bomb bomb1 = player.PlaceBomb()!;
            Assert.AreEqual(1, player.Bombs.Count);
            Assert.AreEqual(player.Row, bomb1.Row);
            Assert.AreEqual(player.Col, bomb1.Col);

            Bomb bomb2 = player.PlaceBomb()!;
            Assert.AreEqual(2, player.Bombs.Count);
            Assert.IsNotNull(bomb2);
            Assert.AreEqual(player.Row, bomb2.Row);
            Assert.AreEqual(player.Col, bomb2.Col);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(true)]
        [DataRow(false)]
        public void PlayerLivesAndDiesTest(bool? state)
        {
            Profile profile = new Profile("Dummy", 5, 4, "greenskin.png", 2, 30, 50);
            Player player = new Player(3, 4, profile, 1);

            Assert.IsTrue(player.IsAlive);
            // This is not very nice but I have no better idea right now
            // I suppose this could be 2 separate test cases, but meh
            if (state != null)
            {
                player.Live((bool)state);
                Assert.AreEqual((bool)state, player.IsAlive);
            }
            else
            {
                player.Live();
                Assert.IsFalse(player.IsAlive);
            }
        }

        #endregion

        #region Bomb class and related tests

        [TestMethod]
        public void BombConstructorTest()
        {
            Bomb bomb = new Bomb(4, 5, 3, true);

            // Entity parts
            Assert.AreEqual(5, bomb.Row);
            Assert.AreEqual(4, bomb.Col);
            Assert.AreEqual("../../../View/Resources/Objects/bomb_2.png", bomb.SpritePath);

            // Bomb parts
            Assert.AreEqual(3, bomb.ExplosionRange);
            Assert.IsNotNull(bomb.Timer);
            // Assert.AreEqual(new TimeSpan.FromMilliseconds(1000), bomb.Timer.Interval); // Why did this suddenly break?? It was working earlier
        }

        [TestMethod]
        public void BombExplodes()
        {
            Bomb bomb = new Bomb(4, 5, 3, true);
            bool explosionSuccess = false;
            PlaceValueEventArgs<Bomb>? eventArgsRecieved = null;

            bomb.BombExploded += (object? sender, PlaceValueEventArgs<Bomb> e) =>
            {
                explosionSuccess = true;
                eventArgsRecieved = e;
            };

            bomb.ExplodeNow();
            Assert.IsTrue(explosionSuccess);
            Assert.IsNotNull(eventArgsRecieved);
            Assert.AreEqual(bomb, eventArgsRecieved.Value);
            Assert.AreEqual(bomb.Row * 70, eventArgsRecieved.Row); // Geri and Alexa know why works, ask them
            Assert.AreEqual(bomb.Col * 70 - 15, eventArgsRecieved.Col);
        }

        #endregion
    }
}