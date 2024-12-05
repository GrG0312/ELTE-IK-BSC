using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexNumbers
{
    internal class menu
    {
        Complex szam1, szam2;


        public void Run()
        {
            int option = 0;
            SetNumbers();
            do
            {
                WriteOptions();
                option = int.Parse(Console.ReadLine());
                ExecuteOption(option);
            } while (option != 0);
        }

        private void WriteOptions()
        {
            //Console.Clear();
            Console.WriteLine("\n0 - Kilépés");
            Console.WriteLine("1 - Komplex számok megadása");
            Console.WriteLine("2 - Megadott elemek összeadása");
            Console.WriteLine("3 - Megadott elemek kivonása");
            Console.WriteLine("4 - Megadott elemek szorzása");
            Console.WriteLine("5 - Megadott elemek osztása");
        }

        private void ExecuteOption(int option)
        {
            switch (option)
            {
                case 0:
                    Console.WriteLine("Viszlát!");
                    break;

                case 1:
                    Console.Clear();
                    SetNumbers();
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine(szam1 + szam2);
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine(szam1 - szam2);
                    break;

                case 4:
                    Console.Clear();
                    Console.WriteLine(szam1 * szam2);
                    break;

                case 5:
                    Console.Clear();
                    try
                    {
                        Console.WriteLine(szam1 / szam2);
                    }
                    catch (DivideByZeroException)
                    {
                        Console.WriteLine("Helytelen eredmény, az osztó nulla!");
                    }
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Helytelen opció!");
                    break;
            }
        }

        private void SetNumbers()
        {
            double re, im;
            Console.Write("Add meg az első komplex szám valós részét: ");
            re = double.Parse(Console.ReadLine());
            Console.Write("Add meg az első komplex szám képzetes részét: ");
            im = double.Parse(Console.ReadLine());
            szam1 = new Complex(re, im);
            Console.Write("Add meg a második komplex szám valós részét: ");
            re = double.Parse(Console.ReadLine());
            Console.Write("Add meg a második komplex szám képzetes részét: ");
            im = double.Parse(Console.ReadLine());
            szam2 = new Complex(re, im);
        }
    }
}
