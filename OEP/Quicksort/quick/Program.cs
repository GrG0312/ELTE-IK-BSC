using System;

namespace quick
{
    public class Progam
    {
        public static void Main(string[] args)
        {
            //int[] tömb = new int[] { 5, 9, 11, 10, 3, 1, 2, 6, 0 };

            int[] a = { 1, 2, 3, 1 };
            int[] b = { 4, 1, 1, 3 };
            bool volt;

            for (int i = 0; i < a.Length; i++)
            {
                volt = false;
                for (int j = 0; j < b.Length; j++)
                {
                    if (a[i] == b[j] && !volt)
                    {
                        Console.WriteLine(b[j]);
                        b[j] = 0;
                        volt = true;
                    }
                    if (volt && a[i] == b[j])
                    {
                        b[j] = 0;
                    }
                }
            }

        }
        /*
        public void QuickSort(int[] kapottTömb, int elso, int utolso)
        {
            if (elso < utolso)
            {
                int ujUtolso = Partition(kapottTömb, elso, utolso);
                QuickSort(kapottTömb, elso, ujUtolso-1);
                QuickSort(kapottTömb, ujUtolso + 1, utolso);
            }
        }
        
        public int Partition(int[] kapottTömb, int elso, int utolso)
        {
            int i = new Random().Next(elso, utolso);
            int x = kapottTömb[i];
            kapottTömb[i] = kapottTömb[utolso];
            i = elso;

            while (i<utolso && kapottTömb[i] <= x)
            {
                i = i + 1;
            }

            if (i<utolso)
            {

            }
        }*/
    }
}