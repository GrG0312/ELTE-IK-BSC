using System;

namespace Beadandó
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Cég Vonatkerékpumpáló_BT = new Cég();
            Telephely t;
            Kamion k;
            Sofőr s;
            Fuvar f;

            FileStream fs = new FileStream("input.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            int telephelynumber = int.Parse(sr.ReadLine());
            for (int i = 0; i < telephelynumber; i++)
            {
                t = new Telephely(sr.ReadLine());
                Vonatkerékpumpáló_BT.Bővül(t);
                int kamionnumber = int.Parse(sr.ReadLine());
                for (int j = 0; j < kamionnumber; j++)
                {
                    string[] darabol = sr.ReadLine().Split(" ");
                    k = new Kamion(darabol[0], Double.Parse(darabol[1]), Double.Parse(darabol[2]), t, int.Parse(darabol[3]),Vonatkerékpumpáló_BT);
                }
            }
            int sofőrnumber = int.Parse(sr.ReadLine());
            for (int i = 0; i < sofőrnumber; i++)
            {
                string[] darabol = sr.ReadLine().Split(";");
                switch (darabol[0])
                {
                    case "Törzs": s = new Törzstag(darabol[1]);
                        break;
                    case "Gyakorlott": s = new Gyakorlott(darabol[1]);
                        break;
                    default: s = new Kezdő(darabol[1]);
                        break;
                }
                Vonatkerékpumpáló_BT.Bővül(s);
            }
            int fuvarnubmer = int.Parse(sr.ReadLine());
            for (int i = 0; i < fuvarnubmer; i++)
            {
                string[] darabol = sr.ReadLine().Split(" ");
                Vonatkerékpumpáló_BT.ÚjFuvar(Double.Parse(darabol[0]), Double.Parse(darabol[1]), int.Parse(darabol[2]),new DateTime(int.Parse(darabol[3]), int.Parse(darabol[4]), int.Parse(darabol[5]), int.Parse(darabol[6]), int.Parse(darabol[7]), int.Parse(darabol[8])), int.Parse(darabol[9]), darabol[10], darabol[11]);
            }
            Console.WriteLine("Fájlból rögzítés kész.");

            while (!sr.EndOfStream)
            {
                string[] darabol = sr.ReadLine().Split(" ");
                Vonatkerékpumpáló_BT.Start(new DateTime(int.Parse(darabol[0]), int.Parse(darabol[1]), int.Parse(darabol[2]), int.Parse(darabol[3]), int.Parse(darabol[4]), int.Parse(darabol[5])));
            }

            Console.WriteLine();

            Console.WriteLine("A legkevesebb kamionnal rendelkező telephely címe:\n" + Vonatkerékpumpáló_BT.LegkevesebbK().cím);

            Console.WriteLine("\nMelyik telephelynek adjuk meg a maximum elszállítható terhét? Adjon meg egy címet:");
            Console.WriteLine(Vonatkerékpumpáló_BT.MaxTeher(new Telephely(Console.ReadLine())));

            Console.WriteLine("\nVolt-e túlterhelt kamion a kiszállítások során? " + (Vonatkerékpumpáló_BT.VoltTúlterhelt() == true ? "Igen" : "Nem"));

            Console.WriteLine("A cég nyeresége: " + Vonatkerékpumpáló_BT.Nyereség());
        }
    }
}