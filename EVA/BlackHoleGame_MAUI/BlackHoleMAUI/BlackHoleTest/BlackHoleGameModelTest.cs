using BlackHole.Model;
using BlackHole.Persistance;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Win32.SafeHandles;

namespace BlackHole.Test
{
    [TestClass]
    public class BlackHoleGameModelTest
    {
        private BlackHoleGameModel model = null!; // a tesztelendõ modell
        private BlackHoleTable mockedTable = null!; // mockolt játéktábla
        private Mock<IBlackHoleDataManager> mock = null!; // az adatelérés mock-ja

        [TestInitialize]
        public void Initialize()
        {
            mockedTable = new BlackHoleTable(7);
            mockedTable.SetFieldValue(3, 3, 3);//fekete lyuk
            mockedTable.SetFieldValue(1, 3, 1);//piros kettõvel balra a lyuktól
            mockedTable.SetFieldValue(4, 3, 2);//kék eggyel jobbra a lyuktól
            mockedTable.SetFieldValue(6, 6, 2);//kék a pálya jobb alsó sarkában
            mockedTable.SetFieldValue(0, 0, 1);//piros a pálya bal felsõ sarkában

            mock = new Mock<IBlackHoleDataManager>();
            mock.Setup(mock => mock.LoadGame(It.IsAny<String>()))
                .Returns(() => Task.FromResult((2, 2, 2, 7, mockedTable)));
            // a mock a LoadAsync mûveletben bármilyen paraméterre az elõre beállított játéktáblát fogja visszaadni

            model = new BlackHoleGameModel(mock.Object);
            // példányosítjuk a modellt a mock objektummal

            model.FieldHasChanged += new EventHandler<BlackHoleFieldEventArgs>(Model_OnFieldChange);
            model.ShipEntered += new EventHandler<int>(Model_ShipHasEntered);
        }

        #region Tesztesetek
        [TestMethod]
        public void BlackHole_Game_NewGame_Test()
        {
            int size = 5;
            int shipnumber = 0;
            model.NewGame(size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (model.GetTableFieldValue(i,j) == 1)
                    {
                        shipnumber++;
                    }
                }
            }
            Assert.AreEqual(size-1, shipnumber);

            Assert.AreEqual(model.GetEnteredShipsNumber(1), 0);
            Assert.AreEqual(model.GetEnteredShipsNumber(1), 0);
        }

        [TestMethod]
        public async Task BlackHole_Game_LoadGame_Test()
        {
            model.NewGame(9);

            await model.LoadGameAsync("asd");
            /*
            mockedTable.SetFieldValue(3, 3, 3);//fekete lyuk
            mockedTable.SetFieldValue(1, 3, 1);//piros kettõvel balra a lyuktól
            mockedTable.SetFieldValue(4, 3, 2);//kék eggyel jobbra a lyuktól
            mockedTable.SetFieldValue(6, 6, 2);//kék a pálya jobb alsó sarkában
            mockedTable.SetFieldValue(0, 0, 1);//piros a pálya bal felsõ sarkában
            */

            mock.Verify(dataAccess => dataAccess.LoadGame("asd"), Times.Once);

            Assert.AreEqual(model.gameSize, 7);
            Assert.AreEqual(3, model.GetTableFieldValue(3, 3));
            Assert.AreEqual(1, model.GetTableFieldValue(1, 3));
            Assert.AreEqual(2, model.GetTableFieldValue(4, 3));
            Assert.AreEqual(1, model.GetTableFieldValue(0, 0));
            Assert.AreEqual(2, model.GetTableFieldValue(6, 6));
            Assert.AreEqual(0, model.GetTableFieldValue(1, 0));
            Assert.AreEqual(model.currentParty, 2);
        }

        [TestMethod]
        public async Task BlackHole_Game_StepTaken_Test()
        {
            await model.LoadGameAsync("asd");
            Assert.AreEqual(2, model.GetTableFieldValue(6,6));
            model.Click(6,6);
            model.Click(6,5);
            Assert.AreEqual(0, model.GetTableFieldValue(6, 6));
            Assert.AreEqual(2, model.GetTableFieldValue(6, 0));
        }

        [TestMethod]
        public async Task BlackHole_Game_ShipEntered_Test()
        {
            await model.LoadGameAsync("pepega");
            /*
            mockedTable.SetFieldValue(3, 3, 3);//fekete lyuk
            mockedTable.SetFieldValue(1, 3, 1);//piros kettõvel balra a lyuktól
            mockedTable.SetFieldValue(4, 3, 2);//kék eggyel jobbra a lyuktól
            mockedTable.SetFieldValue(6, 6, 2);//kék a pálya jobb alsó sarkában
            mockedTable.SetFieldValue(0, 0, 1);//piros a pálya bal felsõ sarkában
            */

            Assert.AreEqual(2,model.GetTableFieldValue(4,3));
            model.Click(4,3);
            Assert.AreEqual(2, model.GetEnteredShipsNumber(1));
            Assert.AreEqual(2, model.GetEnteredShipsNumber(2));
            model.Click(3,3);
            Assert.AreEqual(2, model.GetEnteredShipsNumber(1));
            Assert.AreEqual(3, model.GetEnteredShipsNumber(2));
        }

        [TestMethod]
        public async Task BlackHole_Game_Victory_Test()
        {
            await model.LoadGameAsync("quack quack");

            bool hasWon = false;
            int winner = 0;
            model.Victory += (s, e) => { hasWon = true; winner = e; };
            Assert.IsFalse(hasWon);
            Assert.AreEqual(0, winner);
            Assert.AreEqual(2, model.GetEnteredShipsNumber(1));
            Assert.AreEqual(2, model.GetEnteredShipsNumber(2));
            /*
             * 1 0 0 0 0 0 0
             * 0 0 0 0 0 0 0
             * 0 0 0 0 0 0 0
             * 0 1 0 3 2 0 0
             * 0 0 0 0 0 0 0
             * 0 0 0 0 0 0 0
             * 0 0 0 0 0 0 2
             */
            model.Click(4,3);
            model.Click(3,3);
            Assert.AreEqual(0, model.GetTableFieldValue(4,3));
            Assert.AreEqual(2, model.GetEnteredShipsNumber(1));
            Assert.AreEqual(3, model.GetEnteredShipsNumber(2));

            Assert.IsTrue(hasWon);
            Assert.AreEqual(2, winner);
        }
        #endregion

        #region Event metódusok

        private void Model_OnFieldChange(object? sender, BlackHoleFieldEventArgs e)
        {
            if (e.Value == 1 || e.Value == 2 || e.Value == 3 || e.Value == 0)
            {
                Assert.AreEqual(model.GetTableFieldValue(e.X, e.Y), e.Value);
            }
        }

        private void Model_ShipHasEntered(object? sender, int e)
        {
            Assert.AreEqual(3,model.GetEnteredShipsNumber(e));
        }

        #endregion
    }
}