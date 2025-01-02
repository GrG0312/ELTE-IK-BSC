using System.Diagnostics.CodeAnalysis;

public class Program
{
    public static void Main(string[] args)
    {

        List<int> primes = new List<int>();
        for (int i = 2; i < 100; i++)
        {
            if(IsPrime(i))
            {
                primes.Add(i);
            }
        }
        Console.WriteLine($"Primes ({primes.Count}):");
        int sum = 0;
        foreach (int i in primes) 
        { 
            sum += i; 
            Console.Write(i + "; "); 
        }

        Console.WriteLine($"\nSum: {sum} ({Math.Ceiling((double)sum/100)})");

        primes.Reverse();

        List<Bucket> buckets = new List<Bucket>() { new Bucket(100) };
        
        while(primes.Count != 0)
        {
            int prime = primes.First();
            Bucket last = buckets.Last();

            bool used = false;
            foreach (Bucket bucket in buckets)
            {
                if (bucket.MaxSize >= bucket.NumbersInside.Sum() + prime)
                {
                    bucket.NumbersInside.Add(prime);
                    used = true;
                    break;
                }
            }
            if (!used)
            {
                Bucket newOne = new Bucket(100);
                newOne.NumbersInside.Add(prime);
                buckets.Add(newOne);
            }
            primes.Remove(prime);
        }

        int index = 1;
        foreach (Bucket b in buckets)
        {
            string elements = string.Join("; ", b.NumbersInside);
            Console.WriteLine($"{index}. [ {b.NumbersInside.Sum()} ]: {elements}");
            index++;
        }
    }

    public static bool IsPrime(int target)
    {
        double sqrt = Math.Sqrt(target);
        for (int i = 2; i <= sqrt; i++)
        {
            if (i % 2 == 0 && i != 2)
            {
                continue;
            }
            if (target % i == 0)
            {
                return false;
            }
        }
        return true;
    }
}

public class Bucket
{
    public readonly int MaxSize;
    public List<int> NumbersInside;
    public Bucket(int size)
    {
        MaxSize = size;
        NumbersInside = new List<int>();
    }
}