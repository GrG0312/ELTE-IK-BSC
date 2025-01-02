using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public const int TEAMS = 4;
    public static List<ScoreBoard> SCORES = new List<ScoreBoard>() { new ScoreBoard(TEAMS) };
    public static void Main(string[] args)
    {
        // Lejátszuk az összes rangadót
        for (int i = 0; i < TEAMS; i++)
        {
            for (int j = i + 1; j < TEAMS; j++)
            {
                PlayMatch(i,j);
            }
        }

        int raw = SCORES.Count;
        Console.WriteLine("Raw amount: " + raw);

        // Kiszűrjük az ismétlődéseket
        List<ScoreBoard> filteredList = new List<ScoreBoard>();
        foreach (ScoreBoard board in SCORES)
        {
            if (filteredList.All(x => !x.Equals(board)))
            {
                filteredList.Add(board);
            }
        }
        SCORES = filteredList;

        /*
         * Ha számít, hogy melyik csapat szerzett mennyi pontot:
         */
        int filtered = SCORES.Count;
        Console.WriteLine("Filtered amount: " + filtered + $" ({filtered-raw})");

        /*
         * Ha nem számít, hogy ki érte el a pontokat:
         */
        // Kiszűrjük az ismétlődéseket, miután csökkenő sorrendbe helyeztük az értékeket
        List<ScoreBoard> filteredOrderedList = new List<ScoreBoard>();
        foreach (ScoreBoard board in SCORES)
        {
            board.Reorder();
            if (filteredOrderedList.All(x => !x.Equals(board)))
            {
                filteredOrderedList.Add(board);
            }
        }
        SCORES = filteredOrderedList;

        //PrintScores();
        int filteredOrdered = SCORES.Count;
        Console.WriteLine("Filtered and ordered amount: " + filteredOrdered);
    }


    public static void PrintScores()
    {
        foreach (ScoreBoard board in SCORES)
        {
            Console.WriteLine(board);
        }
    }
    public static void Reset()
    {
        SCORES = new List<ScoreBoard>() { new ScoreBoard(TEAMS) };
    }
    public static void PlayMatch(int myIndex, int opponentIndex)
    {
        List<ScoreBoard> updated = new List<ScoreBoard>();
        foreach (ScoreBoard old in SCORES)
        {
            //Win
            ScoreBoard scoreWin = new ScoreBoard(TEAMS);
            old.CopyTo(scoreWin);
            scoreWin[myIndex] += 3;
            updated.Add(scoreWin);

            //Draw
            ScoreBoard scoreDraw = new ScoreBoard(TEAMS);
            old.CopyTo(scoreDraw);
            scoreDraw[myIndex] += 1;
            scoreDraw[opponentIndex] += 1;
            updated.Add(scoreDraw);

            //Lose
            ScoreBoard scoreLose = new ScoreBoard(TEAMS);
            old.CopyTo(scoreLose);
            scoreLose[opponentIndex] += 3;
            updated.Add(scoreLose);
        }
        SCORES = updated;
    }
}

public class ScoreBoard
{
    private int[] Scores;

    public int this[int index]
    {
        get
        {
            return Scores[index];
        }
        set
        {
            Scores[index] = value;
        }
    }

    public ScoreBoard(int size)
    {
        Scores = new int[size];
    }

    public void CopyTo(ScoreBoard target)
    {
        Scores.CopyTo(target.Scores, 0);
    }
    public void Reorder()
    {
        Scores = Scores.OrderByDescending(x => x).ToArray();
    }

    public override string ToString()
    {
        return string.Join(" ", Scores);
    }
    public bool Equals(ScoreBoard obj)
    {
        if (Scores.Length != obj.Scores.Length)
        {
            return false;
        }
        for (int index = 0; index < Scores.Length; index++)
        {
            if (Scores[index] != obj.Scores[index])
            {
                return false;
            }
        }
        return true;
    }
}