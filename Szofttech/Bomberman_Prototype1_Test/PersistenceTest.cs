using Bomberman_Prototype1.Model;
using Bomberman_Prototype1.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Bomberman_Prototype1_Test
{
    [TestClass]
    public class PersistenceTest
    {
        [TestMethod]
        [DataRow(Field.WALL, "b")]
        [DataRow(Field.EMPTY, "n")]
        [DataRow(Field.WEAK_WALL, "r")]
        [DataRow(Field.BOX, "x")]
        [DataRow(Field.EMPTY, "z")] // this is an invalid token, testing for the default case
        public void MapLoaderParsesFieldTokenTest(Field expectedField, string c)
        {
            MapLoader mapLoader = new MapLoader();
            Assert.AreEqual(expectedField, mapLoader.ParseFieldToken(c));
        }

        [TestMethod]
        public void ProfileConstructorWithFullParameterListTest()
        {
            Profile profile = new Profile("dummy", 5, 3, "greenskin.png", 3, 4, 5);
            Assert.AreEqual("dummy", profile.Name);
            Assert.AreEqual(5, profile.PlayedGames);
            Assert.AreEqual(3, profile.GamesWon);
            Assert.AreEqual("greenskin.png", profile.SpritePath);
            Assert.AreEqual(new TimeSpan(3, 4, 5), profile.PlayTime);
        }

        [TestMethod]
        public void ProfileConstructorWithoutTimespanParametersTest()
        {
            Profile profile = new Profile("dummy", 5, 3, "greenskin.png");
            Assert.AreEqual("dummy", profile.Name);
            Assert.AreEqual(5, profile.PlayedGames);
            Assert.AreEqual(3, profile.GamesWon);
            Assert.AreEqual("greenskin.png", profile.SpritePath);
            Assert.AreEqual(new TimeSpan(0, 0, 0), profile.PlayTime);
        }

        [TestMethod]
        [DataRow(true, 50)]
        [DataRow(false, 50)]
        public void ProfileUpdatesStatsTest(bool won, int seconds)
        {
            Profile profile = new Profile("dummy", 5, 3, "greenskin.png");
            profile.UpdateStats(won, seconds);
            Assert.AreEqual(6, profile.PlayedGames);
            Assert.AreEqual(won ? 4 : 3, profile.GamesWon);
            Assert.AreEqual(new TimeSpan(0, 0, 50), profile.PlayTime);
        }

        [TestMethod]
        public void ProfileToStringTest()
        {
            Profile profile = new Profile("dummy", 5, 3, "greenskin.png");
            string expected = "dummy;5;3;greenskin.png";
            Assert.AreEqual(expected, profile.ToString());
        }
    }
}
