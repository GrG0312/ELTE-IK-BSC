using Beadandó;
using System;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LegkevesebbKTEST()
        {
            Cég Villamoskerék_Pumpáló_KFT = new Cég();
            try
            {
                Villamoskerék_Pumpáló_KFT.LegkevesebbK();
                Assert.Fail();
            }
            catch (Cég.NoSiteFoundException)
            {
                Console.WriteLine("Exception elkapva");
            }

            Telephely hely01 = new Telephely("1111 Hangos Csendes-óceán út 23");
            Telephely hely02 = new Telephely("2222 Méghangosabb Csendes-óceán út 145");
            Telephely hely03 = new Telephely("Erre nincs ötlet");

            Villamoskerék_Pumpáló_KFT.Bõvül(hely01);
            Assert.AreSame(hely01, Villamoskerék_Pumpáló_KFT.LegkevesebbK());

            Villamoskerék_Pumpáló_KFT.Bõvül(hely02);
            Villamoskerék_Pumpáló_KFT.Bõvül(hely03);
            Console.WriteLine("Cég kibõvítve");

            Kamion audi = new Kamion("123-ABC", 15.5, 14.0, hely01, 2, Villamoskerék_Pumpáló_KFT);
            Kamion bmw = new Kamion("555-WMB", 3.5, 60.0, hely02, 2, Villamoskerék_Pumpáló_KFT);
            Kamion man = new Kamion("088-PAN", 45.5, 10.0, hely02, 2, Villamoskerék_Pumpáló_KFT);
            Kamion tesla = new Kamion("999-BES", 30.0, 4.5, hely02, 3, Villamoskerék_Pumpáló_KFT);
            Console.WriteLine("Kamionok hozzáadva");

            Assert.AreEqual(1,hely01.Mennyi());
            Assert.AreEqual(3, hely02.Mennyi());
            Assert.AreEqual(0,hely03.Mennyi());
            Console.WriteLine("Telephelyek jármûszáma jó");

            Assert.AreSame(hely03, Villamoskerék_Pumpáló_KFT.LegkevesebbK());
            Console.WriteLine("LegkevesebbK sikeres");
        }

        [TestMethod]
        public void MaxTeherTEST()
        {
            Cég Villamoskerék_Pumpáló_KFT = new Cég();

            Telephely hely01 = new Telephely("1111 Hangos Csendes-óceán út 23");
            Telephely hely02 = new Telephely("2222 Méghangosabb Csendes-óceán út 145");
            Telephely hely03 = new Telephely("Erre nincs ötlet");
            try
            {
                Villamoskerék_Pumpáló_KFT.MaxTeher(hely01);
                Assert.Fail();
            }
            catch (Cég.NoSiteFoundException)
            {
                Console.WriteLine("Exception elkapva");
            }

            Villamoskerék_Pumpáló_KFT.Bõvül(hely01);
            Assert.AreEqual(0, Villamoskerék_Pumpáló_KFT.MaxTeher(hely01));
            Console.WriteLine("1/0: visszatérés 0");

            Kamion audi = new Kamion("123-ABC", 15.5, 14.0, hely01, 2, Villamoskerék_Pumpáló_KFT);
            Kamion bmw = new Kamion("555-WMB", 3.0, 60.0, hely01, 2, Villamoskerék_Pumpáló_KFT);
            Kamion man = new Kamion("088-PAN", 45.5, 10.0, hely01, 2, Villamoskerék_Pumpáló_KFT);
            //Console.WriteLine(hely01.Mennyi());
            
            Assert.AreEqual(15.5+3.0+45.5, Villamoskerék_Pumpáló_KFT.MaxTeher(hely01));
            Console.WriteLine("Az összeg jó");

            Villamoskerék_Pumpáló_KFT.Bõvül(hely02);
            Villamoskerék_Pumpáló_KFT.Bõvül(hely03);
            Kamion tesla = new Kamion("999-BES", 30.0, 4.5, hely02, 3, Villamoskerék_Pumpáló_KFT);
            Assert.AreEqual(15.5 + 3.0 + 45.5, Villamoskerék_Pumpáló_KFT.MaxTeher(hely01));
            Console.WriteLine("A teszt sikeres");
        }

        [TestMethod]
        public void VoltTúlterheltTEST()
        {
            Cég Villamoskerék_Pumpáló_KFT = new Cég();

            Telephely hely01 = new Telephely("1111 Hangos Csendes-óceán út 23");
            Telephely hely02 = new Telephely("2222 Méghangosabb Csendes-óceán út 145");
            Telephely hely03 = new Telephely("Erre nincs ötlet");

            Villamoskerék_Pumpáló_KFT.Bõvül(hely01);
            Villamoskerék_Pumpáló_KFT.Bõvül(hely02);
            Villamoskerék_Pumpáló_KFT.Bõvül(hely03);

            Kamion audi = new Kamion("123-ABC", 15.5, 14.0, hely01, 2, Villamoskerék_Pumpáló_KFT);
            Kamion bmw = new Kamion("555-WMB", 3.0, 60.0, hely02, 2, Villamoskerék_Pumpáló_KFT);
            Kamion man = new Kamion("088-PAN", 45.5, 10.0, hely03, 3, Villamoskerék_Pumpáló_KFT);

            Sofõr péter = new Törzstag("Péter");
            Sofõr feri = new Gyakorlott("Ferenc");
            Sofõr karcsi = new Kezdõ("Károly");

            Villamoskerék_Pumpáló_KFT.Bõvül(péter);
            Villamoskerék_Pumpáló_KFT.Bõvül(feri);
            Villamoskerék_Pumpáló_KFT.Bõvül(karcsi);

            Assert.AreEqual(false, Villamoskerék_Pumpáló_KFT.VoltTúlterhelt());

            Villamoskerék_Pumpáló_KFT.ÚjFuvar(60.0, 15.0, 15000, new DateTime(2023, 05, 20, 15, 01, 01), 1, péter, audi);
            Villamoskerék_Pumpáló_KFT.ÚjFuvar(30.0, 2.0, 20000, new DateTime(2023, 05, 20, 16, 30, 01), 1, péter, bmw);
            Villamoskerék_Pumpáló_KFT.ÚjFuvar(120.0, 40.0, 7500, new DateTime(2023, 05, 25, 15, 01, 01), 1, feri, man);

            Assert.AreEqual(false, Villamoskerék_Pumpáló_KFT.VoltTúlterhelt());
            Console.WriteLine("Nem volt, nem indult.");

            Villamoskerék_Pumpáló_KFT.Start(new DateTime(2023, 05, 20, 15, 01, 01));

            Assert.AreEqual(false, Villamoskerék_Pumpáló_KFT.VoltTúlterhelt());
            Console.WriteLine("Nem volt, indult.");

            Villamoskerék_Pumpáló_KFT.ÚjFuvar(15.0, 60.0, 150000, new DateTime(2023,05,27,11,48,01),2,karcsi,bmw);
            Villamoskerék_Pumpáló_KFT.Start(new DateTime(2023, 05, 27, 11, 48, 01));

            Assert.AreEqual(true, Villamoskerék_Pumpáló_KFT.VoltTúlterhelt());
            Console.WriteLine("Volt, indult.");
        }

        [TestMethod]
        public void NyereségTEST()
        {
            Cég Villamoskerék_Pumpáló_KFT = new Cég();

            Telephely hely01 = new Telephely("1111 Hangos Csendes-óceán út 23");
            Telephely hely02 = new Telephely("2222 Méghangosabb Csendes-óceán út 145");
            Telephely hely03 = new Telephely("Erre nincs ötlet");

            Villamoskerék_Pumpáló_KFT.Bõvül(hely01);
            Villamoskerék_Pumpáló_KFT.Bõvül(hely02);
            Villamoskerék_Pumpáló_KFT.Bõvül(hely03);

            Kamion audi = new Kamion("123-ABC", 15.5, 14.0, hely01, 2, Villamoskerék_Pumpáló_KFT);
            Kamion bmw = new Kamion("555-WMB", 3.0, 60.0, hely02, 2, Villamoskerék_Pumpáló_KFT);
            Kamion man = new Kamion("088-PAN", 45.5, 10.0, hely03, 3, Villamoskerék_Pumpáló_KFT);

            Sofõr péter = new Törzstag("Péter");
            Sofõr feri = new Gyakorlott("Ferenc");
            Sofõr karcsi = new Kezdõ("Károly");

            Villamoskerék_Pumpáló_KFT.Bõvül(péter);
            Villamoskerék_Pumpáló_KFT.Bõvül(feri);
            Villamoskerék_Pumpáló_KFT.Bõvül(karcsi);

            Assert.AreEqual(0, Villamoskerék_Pumpáló_KFT.Nyereség());
            Console.WriteLine("Nem volt csomag, nincs nyereség.");

            //Villamoskerék_Pumpáló_KFT.ÚjFuvar(60.0, 15.0, 15000, new DateTime(2023, 05, 20, 15, 01, 01), 1, péter, audi);
            //Villamoskerék_Pumpáló_KFT.ÚjFuvar(30.0, 2.0, 20000, new DateTime(2023, 05, 20, 16, 30, 01), 1, péter, bmw);
            //Villamoskerék_Pumpáló_KFT.ÚjFuvar(120.0, 40.0, 5000, new DateTime(2023, 05, 20, 17, 01, 01), 1, feri, man);

            // 40*60=2400 BÉR#1 14/100*60=8.4 ÜZEMANYAG#1 ===> 15000-2400-8.4 = 12591.6 = 
            // 40*30=1200 BÉR#2 60/100*30=18  ÜZEMANYAG#2 ===> 20000-1200-18  = 18782   ====> 32162
            //35*120=4200 BÉR#3 10/100*120=12 ÜZEMANYAG#3 ===> 5000-4200-12   = 788     =

            /*Ez a felsõ 6 sor az eredeti, vasárnap bemutatott teszt része, csak gondoltam bennehagyom, hogy mi volt a probléma:
             * Az eredmény amit ezekkel kiad: 12591,6
             * Ami csak a legelsõ fuvar nyeresége, és ami ennek az indoka, hogy elrontottam a fuvarok kiosztását, hisz az Indul()
             *  metódus úgy mûködik, hogy:
             *          1) megnéz egy kamiont, hogy van-e akkor kiszállítandó fuvarja, ha van, kiszállítja
             *          2) még ugyanennél a kamionnál megnézi, hogy mielõtt visszatérne a telephelyre, van-e másik kiszállítandó fuvara, ami akkor igaz ha
             *              a másik fuvar indulása >= elõzõ érkezése && másik fuvar indulása <= elõzõ érkezése + elõzõ kiszállítási ideje (mivel ha x idõ kivinni,
             *                  x idõ visszatérni a telephelyre), ha van ilyen fuvar kiszállítja, ezt a lépést pedig addig ismételi ameddig nem marad kiszállítandó fuvar
             *          3) ha nincs több kiszállítandó fuvar akkor visszatér a telephelyre a kamion és tovább lépünk a következõ kamionra, DE az eredeti indulási idõt adjuk ki neki
             * Mivel az eredeti indulási idõ = new DateTime(2023, 05, 20, 15, 01, 01), ezért az a második kamionnál nem egyenlõ az õ fuvarának kiszállítási idejével
             * A javított változat: egy kamionnak kell kiosztani a fuvarokat.
            */

            Villamoskerék_Pumpáló_KFT.ÚjFuvar(60.0, 15.0, 15000, new DateTime(2023, 05, 20, 15, 01, 01), 1, péter, man);
            Villamoskerék_Pumpáló_KFT.ÚjFuvar(30.0, 2.0, 20000, new DateTime(2023, 05, 20, 16, 30, 01), 1, péter, man);
            Villamoskerék_Pumpáló_KFT.ÚjFuvar(120.0, 40.0, 5000, new DateTime(2023, 05, 20, 17, 01, 01), 1, feri, man);

            /* Ekkor az új számolás:
             * 40* 60=2400 #BÉR1 10/100* 60= 6 #ÜZEMANYAG1 == 15000 - 2400 - 6 = 12594
             * 40* 30=1200 #BÉR2 10/100* 30= 3 #ÜZEMANYAG2 == 20000 - 1200 - 3 = 18797 === 32179
             * 35*120=4200 #BÉR3 10/100*120=12 #ÜZEMANYAG3 ==  5000 - 4200 -12 =   788
            */

            Assert.AreEqual(0, Villamoskerék_Pumpáló_KFT.Nyereség());
            Console.WriteLine("Volt fuvar, nem vitték ki");

            Villamoskerék_Pumpáló_KFT.Start(new DateTime(2023, 05, 20, 15, 01, 01));
            Assert.AreEqual(32179, Villamoskerék_Pumpáló_KFT.Nyereség());
            Console.WriteLine("Volt fuvar, kivitték");
        }
        //teszt, double, kérdések
    }
}