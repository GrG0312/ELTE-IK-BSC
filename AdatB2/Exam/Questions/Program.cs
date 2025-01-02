namespace Questions
{
    public class Program
    {
        public static List<Question> allQuestions = new List<Question>();
        public static void Main(string[] args)
        {
            OnStartup();

            while(true)
            {
                Run? current = InitRun();
                if (current is null)
                {
                    break;
                }
                ProgressRun(current);
            }
        }

        public static void OnStartup()
        {
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    // *Empty*
                    // Question text
                    // Answer text
                    // *Empty*
                    string q = sr.ReadLine()!;
                    string a = "";
                    string line = sr.ReadLine()!;
                    do
                    {
                        a += line + "\n";
                        line = sr.ReadLine()!;
                    } while(line != "ztz");
                    Question read = new(q, a);
                    allQuestions.Add(read);
                    sr.ReadLine();
                }
            }
        }
        public static Run? InitRun()
        {
            Console.Clear();
            Console.WriteLine("How to use:");
            Console.WriteLine("1-60: Get the questions from the 1st to the 60th");
            Console.WriteLine("full: Get all the questions");
            Console.WriteLine("exit: Stop the run");
            DisplayChapters();
            string? input;
            bool shouldLeave = false;
            do
            {
                input = Console.ReadLine();
                switch (input?.Trim().ToLower())
                {
                    case null:
                        continue;
                    case "exit":
                        shouldLeave = true;
                        break;
                    case "full":
                        return new Run(allQuestions);
                    default:
                        try
                        {
                            string[] splitted = input.Split('-');
                            int startInd = int.Parse(splitted[0]) - 1;
                            int endInd = int.Parse(splitted[1]);
                            if (startInd < 0 || startInd > endInd || endInd > allQuestions.Count)
                            {
                                throw new Exception("szar indexet adtál meg");
                            }
                            List<Question> newSet = new List<Question>(allQuestions.GetRange(startInd, endInd - startInd));
                            return new Run(newSet);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"\nbazdmeg: {e.Message}\n");
                        }
                        break;
                }
            } while (!shouldLeave);
            return null;
        }
        public static void ProgressRun(Run current)
        {
            int counter = 0;
            while (!current.IsOver)
            {
                counter++;
                Console.Clear();
                Question cq = current.Next();
                Console.WriteLine($"[{counter}/{current.Length}] " + cq.Text);
                if (WaitForInput())
                {
                    break;
                }
                Console.Clear();
                Console.WriteLine($"[{counter}/{current.Length}] " + cq.ToString());
                if (WaitForInput())
                {
                    break;
                }
            }
        }
        public static bool WaitForInput()
        {
            string? input = Console.ReadLine();
            return input?.Trim().ToLower() == "exit";
        }

        public static void DisplayChapters()
        {
            Console.WriteLine("\n----- CHAPTERS -----");
            Console.WriteLine("Alap fogalmak:                   1-9");
            Console.WriteLine("Fájlszervezés, kupacok:          10-12");
            Console.WriteLine("Hasító indexelés:                13-22");
            Console.WriteLine("Rendezett állomány:              23-26");
            Console.WriteLine("Elsődleges / másodlagos indexek: 27-31");
            Console.WriteLine("Klaszterezés:                    33-35");
            Console.WriteLine("t-szintű indexelés:              36-41");
            Console.WriteLine("B+ fa és példák:                 42-49");
            Console.WriteLine("Bitmap példák:                   50-51");
            Console.WriteLine("Relációs algebrai optimalizálás: 52-84");
            Console.WriteLine("Újabb jelölések:                 91-92");
            Console.WriteLine("Átlagos költség:                 93-95");
            Console.WriteLine("Lekérdezések kiszámolási módja:  96-97");
            Console.WriteLine("Rendezések:                      98-108");
            Console.WriteLine("Összekapcsolások:                109-117");
            Console.WriteLine("Hány sora van a lekérdezésnek:   118-126");
        }
    }
}