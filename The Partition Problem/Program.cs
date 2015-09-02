using System;
using System.Linq;
using System.IO;

namespace The_Partition_Problem
{
    static class Constants
    {
        public const int MAXN = 10000;
        public const int MAXK = 10000;
        public const int MAXINT = Int32.MaxValue;
    }

    class Program
    {
        static void Main() 
        {
            using (StreamReader sr = new StreamReader("input.txt")) 
            {
                var arrangement = sr.ReadLine().Split(new char[] { ',', ' ' },
                                  StringSplitOptions.RemoveEmptyEntries).Select
                                  (s => int.Parse(s)).ToArray(); 

                var numberOfRanges = sr.ReadLine().Split(new char[] { ',', ' ' },
                                     StringSplitOptions.RemoveEmptyEntries).Select
                                     (k => int.Parse(k)).ToArray(); 

                Partition(arrangement, numberOfRanges[0]);
            }

        }

        static void Partition(int[] arrangement, int numberOfRanges) 
        {
            var values = new int[Constants.MAXN + 1, Constants.MAXK + 1]; 
            var dividers = new int[Constants.MAXN + 1, Constants.MAXK + 1]; 
            var prefixSums = new int[Constants.MAXN + 1]; 
            int testSplitCost;
            int i, j, x; // counters 

            // construct prefix sums
            prefixSums[0] = 0;   
            for (i = 0; i < arrangement.Length; i++) prefixSums[i + 1] = prefixSums[i] + arrangement[i];

            //initialize boundaries conditions
            for (i = 1; i <= arrangement.Length; i++) values[i, 1] = prefixSums[i]; 
            for (j = 1; j <= numberOfRanges; j++)  values[1, j] = arrangement[1];

            //evaluate main recurrence
            for (i = 2; i <= arrangement.Length; i++) 
            {
                for (j = 2; j <= numberOfRanges; j++)
                {
                    values[i, j] = Constants.MAXINT;

                    for (x = 1; x <= (i - 1); x++)
                    {
                        testSplitCost = Math.Max(values[x, j - 1], prefixSums[i] - prefixSums[x]);

                        if (values[i, j] > testSplitCost)
                        {
                            values[i, j] = testSplitCost;
                            dividers[i, j] = x;
                        }
                    }
                }
            }

            Reconstruct_partition(arrangement, dividers, arrangement.Length, numberOfRanges); 
        }

        static void Reconstruct_partition(int[] arrangement, int[,] dividers, int arrangementLength, int numberOfRanges) 
        {
            if (numberOfRanges == 1)
            {
                Print_books(arrangement, 1, arrangementLength);
            }
            else
            {
                Reconstruct_partition(arrangement, dividers, dividers[arrangementLength, numberOfRanges], numberOfRanges - 1);

                Print_books(arrangement, dividers[arrangementLength, numberOfRanges], arrangementLength);
            }

        } 

        static void Print_books(int[] arrangement, int start, int end) 
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                for (var i = start; i < end; i++)
                {
                    sw.Write(" " + arrangement[i] + " ");
                }

                 sw.WriteLine();
            }

        }
  
     }
}
