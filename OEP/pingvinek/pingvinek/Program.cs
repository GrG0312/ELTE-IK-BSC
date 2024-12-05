using System;
using TextFile;

namespace pingvinek
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InFile file = new InFile("inp.txt");
            Dolgozz(file);
        }
        public static void Dolgozz(InFile file)
        {
            bool l = true;
            int max = 0;
            while (file.Read(out Megfigyelés m))
            {
                l = l && m.becslés > m.Össz(m.pingvinek);
                int kül = Math.Abs(m.Össz(m.pingvinek) - m.becslés);
                if (kül > max)
                {
                    max = kül;
                }
            }
            if (l)
            {
                Console.Write("Igaz ");
            } else
            {
                Console.Write("Hamis ");
            }
            Console.WriteLine(max);
        }
        
        
    }
}