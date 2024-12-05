using Beadand�;
using System;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LegkevesebbKTEST()
        {
            C�g Villamosker�k_Pump�l�_KFT = new C�g();
            try
            {
                Villamosker�k_Pump�l�_KFT.LegkevesebbK();
                Assert.Fail();
            }
            catch (C�g.NoSiteFoundException)
            {
                Console.WriteLine("Exception elkapva");
            }

            Telephely hely01 = new Telephely("1111 Hangos Csendes-�ce�n �t 23");
            Telephely hely02 = new Telephely("2222 M�ghangosabb Csendes-�ce�n �t 145");
            Telephely hely03 = new Telephely("Erre nincs �tlet");

            Villamosker�k_Pump�l�_KFT.B�v�l(hely01);
            Assert.AreSame(hely01, Villamosker�k_Pump�l�_KFT.LegkevesebbK());

            Villamosker�k_Pump�l�_KFT.B�v�l(hely02);
            Villamosker�k_Pump�l�_KFT.B�v�l(hely03);
            Console.WriteLine("C�g kib�v�tve");

            Kamion audi = new Kamion("123-ABC", 15.5, 14.0, hely01, 2, Villamosker�k_Pump�l�_KFT);
            Kamion bmw = new Kamion("555-WMB", 3.5, 60.0, hely02, 2, Villamosker�k_Pump�l�_KFT);
            Kamion man = new Kamion("088-PAN", 45.5, 10.0, hely02, 2, Villamosker�k_Pump�l�_KFT);
            Kamion tesla = new Kamion("999-BES", 30.0, 4.5, hely02, 3, Villamosker�k_Pump�l�_KFT);
            Console.WriteLine("Kamionok hozz�adva");

            Assert.AreEqual(1,hely01.Mennyi());
            Assert.AreEqual(3, hely02.Mennyi());
            Assert.AreEqual(0,hely03.Mennyi());
            Console.WriteLine("Telephelyek j�rm�sz�ma j�");

            Assert.AreSame(hely03, Villamosker�k_Pump�l�_KFT.LegkevesebbK());
            Console.WriteLine("LegkevesebbK sikeres");
        }

        [TestMethod]
        public void MaxTeherTEST()
        {
            C�g Villamosker�k_Pump�l�_KFT = new C�g();

            Telephely hely01 = new Telephely("1111 Hangos Csendes-�ce�n �t 23");
            Telephely hely02 = new Telephely("2222 M�ghangosabb Csendes-�ce�n �t 145");
            Telephely hely03 = new Telephely("Erre nincs �tlet");
            try
            {
                Villamosker�k_Pump�l�_KFT.MaxTeher(hely01);
                Assert.Fail();
            }
            catch (C�g.NoSiteFoundException)
            {
                Console.WriteLine("Exception elkapva");
            }

            Villamosker�k_Pump�l�_KFT.B�v�l(hely01);
            Assert.AreEqual(0, Villamosker�k_Pump�l�_KFT.MaxTeher(hely01));
            Console.WriteLine("1/0: visszat�r�s 0");

            Kamion audi = new Kamion("123-ABC", 15.5, 14.0, hely01, 2, Villamosker�k_Pump�l�_KFT);
            Kamion bmw = new Kamion("555-WMB", 3.0, 60.0, hely01, 2, Villamosker�k_Pump�l�_KFT);
            Kamion man = new Kamion("088-PAN", 45.5, 10.0, hely01, 2, Villamosker�k_Pump�l�_KFT);
            //Console.WriteLine(hely01.Mennyi());
            
            Assert.AreEqual(15.5+3.0+45.5, Villamosker�k_Pump�l�_KFT.MaxTeher(hely01));
            Console.WriteLine("Az �sszeg j�");

            Villamosker�k_Pump�l�_KFT.B�v�l(hely02);
            Villamosker�k_Pump�l�_KFT.B�v�l(hely03);
            Kamion tesla = new Kamion("999-BES", 30.0, 4.5, hely02, 3, Villamosker�k_Pump�l�_KFT);
            Assert.AreEqual(15.5 + 3.0 + 45.5, Villamosker�k_Pump�l�_KFT.MaxTeher(hely01));
            Console.WriteLine("A teszt sikeres");
        }

        [TestMethod]
        public void VoltT�lterheltTEST()
        {
            C�g Villamosker�k_Pump�l�_KFT = new C�g();

            Telephely hely01 = new Telephely("1111 Hangos Csendes-�ce�n �t 23");
            Telephely hely02 = new Telephely("2222 M�ghangosabb Csendes-�ce�n �t 145");
            Telephely hely03 = new Telephely("Erre nincs �tlet");

            Villamosker�k_Pump�l�_KFT.B�v�l(hely01);
            Villamosker�k_Pump�l�_KFT.B�v�l(hely02);
            Villamosker�k_Pump�l�_KFT.B�v�l(hely03);

            Kamion audi = new Kamion("123-ABC", 15.5, 14.0, hely01, 2, Villamosker�k_Pump�l�_KFT);
            Kamion bmw = new Kamion("555-WMB", 3.0, 60.0, hely02, 2, Villamosker�k_Pump�l�_KFT);
            Kamion man = new Kamion("088-PAN", 45.5, 10.0, hely03, 3, Villamosker�k_Pump�l�_KFT);

            Sof�r p�ter = new T�rzstag("P�ter");
            Sof�r feri = new Gyakorlott("Ferenc");
            Sof�r karcsi = new Kezd�("K�roly");

            Villamosker�k_Pump�l�_KFT.B�v�l(p�ter);
            Villamosker�k_Pump�l�_KFT.B�v�l(feri);
            Villamosker�k_Pump�l�_KFT.B�v�l(karcsi);

            Assert.AreEqual(false, Villamosker�k_Pump�l�_KFT.VoltT�lterhelt());

            Villamosker�k_Pump�l�_KFT.�jFuvar(60.0, 15.0, 15000, new DateTime(2023, 05, 20, 15, 01, 01), 1, p�ter, audi);
            Villamosker�k_Pump�l�_KFT.�jFuvar(30.0, 2.0, 20000, new DateTime(2023, 05, 20, 16, 30, 01), 1, p�ter, bmw);
            Villamosker�k_Pump�l�_KFT.�jFuvar(120.0, 40.0, 7500, new DateTime(2023, 05, 25, 15, 01, 01), 1, feri, man);

            Assert.AreEqual(false, Villamosker�k_Pump�l�_KFT.VoltT�lterhelt());
            Console.WriteLine("Nem volt, nem indult.");

            Villamosker�k_Pump�l�_KFT.Start(new DateTime(2023, 05, 20, 15, 01, 01));

            Assert.AreEqual(false, Villamosker�k_Pump�l�_KFT.VoltT�lterhelt());
            Console.WriteLine("Nem volt, indult.");

            Villamosker�k_Pump�l�_KFT.�jFuvar(15.0, 60.0, 150000, new DateTime(2023,05,27,11,48,01),2,karcsi,bmw);
            Villamosker�k_Pump�l�_KFT.Start(new DateTime(2023, 05, 27, 11, 48, 01));

            Assert.AreEqual(true, Villamosker�k_Pump�l�_KFT.VoltT�lterhelt());
            Console.WriteLine("Volt, indult.");
        }

        [TestMethod]
        public void Nyeres�gTEST()
        {
            C�g Villamosker�k_Pump�l�_KFT = new C�g();

            Telephely hely01 = new Telephely("1111 Hangos Csendes-�ce�n �t 23");
            Telephely hely02 = new Telephely("2222 M�ghangosabb Csendes-�ce�n �t 145");
            Telephely hely03 = new Telephely("Erre nincs �tlet");

            Villamosker�k_Pump�l�_KFT.B�v�l(hely01);
            Villamosker�k_Pump�l�_KFT.B�v�l(hely02);
            Villamosker�k_Pump�l�_KFT.B�v�l(hely03);

            Kamion audi = new Kamion("123-ABC", 15.5, 14.0, hely01, 2, Villamosker�k_Pump�l�_KFT);
            Kamion bmw = new Kamion("555-WMB", 3.0, 60.0, hely02, 2, Villamosker�k_Pump�l�_KFT);
            Kamion man = new Kamion("088-PAN", 45.5, 10.0, hely03, 3, Villamosker�k_Pump�l�_KFT);

            Sof�r p�ter = new T�rzstag("P�ter");
            Sof�r feri = new Gyakorlott("Ferenc");
            Sof�r karcsi = new Kezd�("K�roly");

            Villamosker�k_Pump�l�_KFT.B�v�l(p�ter);
            Villamosker�k_Pump�l�_KFT.B�v�l(feri);
            Villamosker�k_Pump�l�_KFT.B�v�l(karcsi);

            Assert.AreEqual(0, Villamosker�k_Pump�l�_KFT.Nyeres�g());
            Console.WriteLine("Nem volt csomag, nincs nyeres�g.");

            //Villamosker�k_Pump�l�_KFT.�jFuvar(60.0, 15.0, 15000, new DateTime(2023, 05, 20, 15, 01, 01), 1, p�ter, audi);
            //Villamosker�k_Pump�l�_KFT.�jFuvar(30.0, 2.0, 20000, new DateTime(2023, 05, 20, 16, 30, 01), 1, p�ter, bmw);
            //Villamosker�k_Pump�l�_KFT.�jFuvar(120.0, 40.0, 5000, new DateTime(2023, 05, 20, 17, 01, 01), 1, feri, man);

            // 40*60=2400 B�R#1 14/100*60=8.4 �ZEMANYAG#1 ===> 15000-2400-8.4 = 12591.6 = 
            // 40*30=1200 B�R#2 60/100*30=18  �ZEMANYAG#2 ===> 20000-1200-18  = 18782   ====> 32162
            //35*120=4200 B�R#3 10/100*120=12 �ZEMANYAG#3 ===> 5000-4200-12   = 788     =

            /*Ez a fels� 6 sor az eredeti, vas�rnap bemutatott teszt r�sze, csak gondoltam bennehagyom, hogy mi volt a probl�ma:
             * Az eredm�ny amit ezekkel kiad: 12591,6
             * Ami csak a legels� fuvar nyeres�ge, �s ami ennek az indoka, hogy elrontottam a fuvarok kioszt�s�t, hisz az Indul()
             *  met�dus �gy m�k�dik, hogy:
             *          1) megn�z egy kamiont, hogy van-e akkor kisz�ll�tand� fuvarja, ha van, kisz�ll�tja
             *          2) m�g ugyanenn�l a kamionn�l megn�zi, hogy miel�tt visszat�rne a telephelyre, van-e m�sik kisz�ll�tand� fuvara, ami akkor igaz ha
             *              a m�sik fuvar indul�sa >= el�z� �rkez�se && m�sik fuvar indul�sa <= el�z� �rkez�se + el�z� kisz�ll�t�si ideje (mivel ha x id� kivinni,
             *                  x id� visszat�rni a telephelyre), ha van ilyen fuvar kisz�ll�tja, ezt a l�p�st pedig addig ism�teli ameddig nem marad kisz�ll�tand� fuvar
             *          3) ha nincs t�bb kisz�ll�tand� fuvar akkor visszat�r a telephelyre a kamion �s tov�bb l�p�nk a k�vetkez� kamionra, DE az eredeti indul�si id�t adjuk ki neki
             * Mivel az eredeti indul�si id� = new DateTime(2023, 05, 20, 15, 01, 01), ez�rt az a m�sodik kamionn�l nem egyenl� az � fuvar�nak kisz�ll�t�si idej�vel
             * A jav�tott v�ltozat: egy kamionnak kell kiosztani a fuvarokat.
            */

            Villamosker�k_Pump�l�_KFT.�jFuvar(60.0, 15.0, 15000, new DateTime(2023, 05, 20, 15, 01, 01), 1, p�ter, man);
            Villamosker�k_Pump�l�_KFT.�jFuvar(30.0, 2.0, 20000, new DateTime(2023, 05, 20, 16, 30, 01), 1, p�ter, man);
            Villamosker�k_Pump�l�_KFT.�jFuvar(120.0, 40.0, 5000, new DateTime(2023, 05, 20, 17, 01, 01), 1, feri, man);

            /* Ekkor az �j sz�mol�s:
             * 40* 60=2400 #B�R1 10/100* 60= 6 #�ZEMANYAG1 == 15000 - 2400 - 6 = 12594
             * 40* 30=1200 #B�R2 10/100* 30= 3 #�ZEMANYAG2 == 20000 - 1200 - 3 = 18797 === 32179
             * 35*120=4200 #B�R3 10/100*120=12 #�ZEMANYAG3 ==  5000 - 4200 -12 =   788
            */

            Assert.AreEqual(0, Villamosker�k_Pump�l�_KFT.Nyeres�g());
            Console.WriteLine("Volt fuvar, nem vitt�k ki");

            Villamosker�k_Pump�l�_KFT.Start(new DateTime(2023, 05, 20, 15, 01, 01));
            Assert.AreEqual(32179, Villamosker�k_Pump�l�_KFT.Nyeres�g());
            Console.WriteLine("Volt fuvar, kivitt�k");
        }
        //teszt, double, k�rd�sek
    }
}