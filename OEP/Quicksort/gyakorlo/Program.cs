using System;

namespace gyakorlo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            int[] tömb = new int[] { 1,2,3,9,1,7,8,11,2,3,4,5};
            int max = 1;
            int tmp = 1;
            for (int i = 0; i < tömb.Length; i++)
            {
                if (i>0 && tömb[i] == (tömb[i-1]+1))
                {
                    tmp += 1;
                }
                else
                {
                    tmp = 1;
                }
                if (max < tmp)
                {
                    max = tmp;
                }
            }
            Console.WriteLine(max);
        }
    }
}