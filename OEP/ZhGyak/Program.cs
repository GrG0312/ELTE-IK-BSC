namespace ZhGyak
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                bool l = true;
                int max = 0;

                InFile input = new InFile("inp.txt");
                while (input.Read(out Megfigyelés megf))
                {
                    l = l && (megf.becsült > megf.összes);
                    int kül = Math.Abs(megf.összes - megf.becsült);
                    if (kül > max)
                    {
                        max = kül;
                    }
                }
                if (l)
                {
                    Console.Write("Igaz ");
                }
                else
                {
                    Console.Write("Hamis ");
                }
                Console.WriteLine(max);
            } catch (FileNotFoundException) {
                Console.WriteLine("Nincs fájl");
            }
        }
    }
}