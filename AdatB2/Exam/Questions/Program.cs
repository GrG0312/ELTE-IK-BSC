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
                bool shouldLeave = ProgressRun(current);
                if (shouldLeave)
                {
                    break;
                }

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
            Console.WriteLine("To start a new Run, type [start question index]-[end question index]\nTo stop a Run or exit the program anytime, type \"exit\"");
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
        public static bool ProgressRun(Run current)
        {
            int counter = 0;
            bool shouldLeave = false;
            while (!current.IsOver || shouldLeave)
            {
                counter++;
                Console.Clear();
                Question cq = current.Next();
                Console.WriteLine($"[{counter}/{current.Length}] " + cq.Text);
                shouldLeave |= WaitForInput();
                Console.Clear();
                Console.WriteLine($"[{counter}/{current.Length}] " + cq.ToString());
                shouldLeave |= WaitForInput();
            }
            return shouldLeave;
        }
        public static bool WaitForInput()
        {
            string? input = Console.ReadLine();
            return input?.Trim().ToLower() == "exit";
        }
    }
}