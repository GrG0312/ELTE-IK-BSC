using Bomberman_Prototype1.Model;
using Bomberman_Prototype1.Model.CustomEventArgs;
using Bomberman_Prototype1.Model.Entities;
using Bomberman_Prototype1.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman_Prototype1_Test
{
    [TestClass]
    public class GameModelTest
    {
        [TestMethod]
        public void GameModelConstructorWithoutMapLoaderInjectionTest()
        {
            GameModel? model = null;
            try { model = new GameModel(); }
            catch (Exception ex) { Assert.Fail(ex.Message); }

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.MapLoader);
        }

        [TestMethod]
        public void GameModelConstructorWithMapLoaderInjectionTest()
        {
            GameModel? model = null;
            try { model = new GameModel(new MockedMapLoader()); }
            catch (Exception ex) { Assert.Fail(ex.Message); }

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.MapLoader);
        }

        [TestMethod]
        public void GameModelStartsNewGameTest()
        {
            GameModel model = new GameModel(new MockedMapLoader());

            bool mapLoadSuccess = false;
            model.MapLoader.MapLoaded += (object? sender, EventArgs e) =>
            {
                mapLoadSuccess = true;
            };

            MapEventArgs? recievedMapEventArgs = null;
            bool newGameSuccess = false;
            model.NewGame += (object? sender, MapEventArgs e) =>
            {
                newGameSuccess = true;
                recievedMapEventArgs = e;
            };


            model.StartNewGame(3, 1, false, 1);
            Assert.AreEqual(1, model.RoundNumber);
            Assert.IsFalse(model.IsBattleRoyale);
            Assert.IsTrue(mapLoadSuccess);

            Assert.IsTrue(newGameSuccess);
            Assert.IsNotNull(recievedMapEventArgs);
            Assert.AreEqual(3, recievedMapEventArgs.Map.GetLength(0));
            Assert.AreEqual(3, recievedMapEventArgs.Map.GetLength(1));
            Assert.AreEqual(3, recievedMapEventArgs.Cols);
            Assert.AreEqual(3, recievedMapEventArgs.Rows);
            Assert.AreEqual(1, recievedMapEventArgs.Type);
            // Assert.AreEqual(3, recievedMapEventArgs.Players.Length); // Nope
        }

        [TestMethod]
        public void GameModelSetsTimerTest()
        {
            GameModel model = new GameModel(new MockedMapLoader());

            try { model.SetTimer(5); } // Honestly no idea what this does, but it sure doesn't throw an exception
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void GameModelSetsBattleRoyalTest(bool isBR)
        {
            GameModel model = new GameModel(new MockedMapLoader());

            model.SetBattleRoyal(isBR);
            Assert.AreEqual(isBR, model.IsBattleRoyale);
        }

        [TestMethod]
        public void GameModelAddsPlayerTest()
        {
            GameModel model = new GameModel(new MockedMapLoader());
            Profile profile = new Profile("dummy", 5, 3, "greenskin.png", 3, 4, 5);

            Player playerAdded = model.AddPlayerToGame(1, profile);
            Assert.AreEqual(2, playerAdded.Row);
            Assert.AreEqual(16, playerAdded.Col);
            Assert.IsTrue(profile == playerAdded.Profile);
        }

        [TestMethod]
        public void GameModelAddsBasicMonsterToGameTest()
        {
            GameModel model = new GameModel(new MockedMapLoader());
            model.AddMonsterToGame(0, "basic"); // This has absolutely no way of working
        }

        [TestMethod]
        public void GameModelChecksNormalPlayersMoveTest()
        {
            GameModel model = new GameModel(new MockedLargeMapLoader());
            model.StartNewGame(2, 1, false, 2);
            bool legal = model.CheckIfLegalMove(Direction.UP, 5, 5, false);
            Assert.IsFalse(legal);

            legal = model.CheckIfLegalMove(Direction.DOWN, 5, 5, false);
            Assert.IsTrue(legal);
        }

        [TestMethod]
        public void GameModelChecksGhostPlayersMoveTest()
        {
            GameModel model = new GameModel(new MockedLargeMapLoader());
            model.StartNewGame(2, 1, false, 2);
            bool legal = model.CheckIfLegalMove(Direction.UP, 5, 5, true);
            Assert.IsTrue(legal);
        }

        [TestMethod]
        [DataRow(1, 1, false)]
        [DataRow(5, 5, true)]
        public void GameModelPlacesBlockersTest(int row, int col, bool expectedPlacementSuccess)
        {
            GameModel model = new GameModel(new MockedLargeMapLoader());
            model.StartNewGame(2, 1, false, 2);
            bool placementSuccess = model.PlaceBlocker(col, row, Field.WEAK_WALL);
            Assert.AreEqual(expectedPlacementSuccess, placementSuccess);
        }

        [TestMethod]
        [DataRow(1, 1, Field.BOX)]
        [DataRow(5, 5, Field.EMPTY)]
        public void GameModelGetsFieldTypeTest(int row, int col, Field expectedFieldType)
        {
            GameModel model = new GameModel(new MockedLargeMapLoader());
            model.StartNewGame(2, 1, false, 2);
            Field field = model.GetFieldType(row, col);
            Assert.AreEqual(expectedFieldType, field);
        }

        [TestMethod]
        public void GameModelChecksForLegalStraightMove()
        {
            GameModel model = new GameModel(new MockedLargeMapLoader());
            model.StartNewGame(2, 1, false, 2);
            bool legal = model.StraightMoveCheck(5, 5, Direction.UP);
            Assert.IsFalse(legal); // I don't care, this should be false

            legal = model.StraightMoveCheck(5, 5, Direction.DOWN);
            Assert.IsTrue(legal);
        }
    }
}
