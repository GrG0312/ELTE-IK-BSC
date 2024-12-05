using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace evfolyamzh
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            int[,] menetrend = new int[N, 2];
            for (int i = 0; i < N; i++)
            {
                string[] sor = Console.ReadLine().Split(' ');
                menetrend[i, 0] = int.Parse(sor[0]);
                menetrend[i, 1] = int.Parse(sor[1]);
            }

            //Első részfeladat
            Console.WriteLine("#");
            int start = 0;
            int end = 0;

            int sp = 0;
            int ep = 0;

            int withoutStop = 0;

            for (int i = 0; i < N; i++)
            {
                if (menetrend[i,0] != 0 && menetrend[i,1] != 0 && menetrend[i,0] == menetrend[i,1])
                {
                    withoutStop++;
                }

                if (menetrend[i,0] == 0 && menetrend[i,1] != 0)
                {
                    start = menetrend[i, 1];
                    sp = i;
                } else if (menetrend[i,0] != 0 && menetrend[i,1] == 0)
                {
                    end = menetrend[i, 0];
                    ep = i;
                }
            }
            Console.WriteLine(end - start);

            //Második részfeladat
            Console.WriteLine("#");
            Console.WriteLine(withoutStop);

            //Harmadik részfeladat
            Console.WriteLine("#");
            bool wasThere = false;
            int max = 0;

            for (int i = sp+1; i < ep; i++)
            {
                if (menetrend[i,0] != 0 && menetrend[i,1] != 0 && menetrend[i,0] < menetrend[i,1])
                {
                    if (max == 0)
                    {
                        max = menetrend[i, 1] - menetrend[i, 0];
                    }else if (max < menetrend[i,1] - menetrend[i,0])
                    {
                        wasThere = true;
                        Console.Write( (i+1) + " ");
                        max = menetrend[i,1] - menetrend[i,0];
                    }
                }
            }
            if (max == 0)
            {
                Console.WriteLine("-2");
            } else if (wasThere == false)
            {
                Console.WriteLine("-1");
            } else
            {
                Console.WriteLine();
            }

            //Negyedik részfeladat
            Console.WriteLine("#");

            for (int i = sp+1; i < ep; i++)
            {
                if (menetrend[i-1, 0] != menetrend[i-1, 1] && menetrend[i,0] == menetrend[i,1])
                {
                    Console.Write((i + 1) + " ");
                }
                if (menetrend[i+1, 0] != menetrend[i+1, 1] && menetrend[i,0] == menetrend[i,1])
                {
                    Console.WriteLine(i + 1);
                }
            }


            //Ötödik részfeladat
            Console.WriteLine("#");
            //wasThere = false;

            start = 0;
            end = 0;
            max = -1;
            int maxstart = 0;
            int maxend = 0;

            for (int i = sp+1; i < ep; i++)
            {
                if ( (menetrend[i-1, 0] == menetrend[i-1, 1] || menetrend[i-1, 0] == 0) && menetrend[i,0] < menetrend[i,1])
                {
                    start = i;
                }
                if ( (menetrend[i + 1, 0] == menetrend[i + 1, 1] || menetrend[i + 1, 1] == 0) && menetrend[i, 0] < menetrend[i, 1])
                {
                    end = i;
                    if (max < end-start)
                    {
                        max = end - start;
                        maxstart = start+1;
                        maxend = end+1;
                    }
                    
                }
            }

            if (max == -1)
            {
                Console.WriteLine("-1");
            } else
            {
                Console.WriteLine(maxstart + " " + maxend);
            }



            //Console.WriteLine();
            //Console.ReadLine();
        }



        /*
        static int Beolvas(int tol, int ig)
        {
            int x = 0;
            bool hiba = false;
            do
            {
                hiba = !int.TryParse(Console.ReadLine(), out x) || x < tol ||x > ig;
            } while (hiba);
            return x;
        }
        */
    }
}
