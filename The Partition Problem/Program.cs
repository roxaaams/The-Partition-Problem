using System;
using System.Linq;
using System.IO;

namespace The_Partition_Problem
{
    class Program
    {
        static void Main() 
        {
            using (var streamReader = new StreamReader("input.txt")) 
            {
                var arrangement = streamReader.ReadLine().Split(new char[] { ',', ' ' },
                                  StringSplitOptions.RemoveEmptyEntries).Select
                                  (s => int.Parse(s)).ToArray();

                var numberOfRanges = streamReader.ReadLine().Split(new char[] { ',', ' ' },
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

            // construct prefix sums
            prefixSums[0] = 0;
            for (var i = 0; i < arrangement.Length; i++)
            { 
                prefixSums[i + 1] = prefixSums[i] + arrangement[i];
            }

            //initialize boundaries conditions
            for (var i = 1; i <= arrangement.Length; i++) 
            { 
                values[i, 1] = prefixSums[i]; 
            }

            for (var j = 1; j <= numberOfRanges; j++)
            {
                values[1, j] = arrangement[1];
            }

            //evaluate main recurrence
            for (var i = 2; i <= arrangement.Length; i++) 
            {
                for (var j = 2; j <= numberOfRanges; j++)
                {
                    values[i, j] = Constants.MAXINT;

                    for (var x = 1; x <= (i - 1); x++)
                    {
                        var testSplitCost = Math.Max(values[x, j - 1], prefixSums[i] - prefixSums[x]);

                        if (values[i, j] > testSplitCost)
                        {
                            values[i, j] = testSplitCost;
                            dividers[i, j] = x;
                        }
                    }
                }
            }

            ReconstructPartition(arrangement, dividers, arrangement.Length, numberOfRanges); 
        }

        static void ReconstructPartition(int[] arrangement, int[,] dividers, int arrangementLength, int numberOfRanges) 
        {
            if (numberOfRanges == 1)
            {
                PrintBooks(arrangement, 1, arrangementLength);
            }
            else
            {
                ReconstructPartition(arrangement, dividers, dividers[arrangementLength, numberOfRanges], numberOfRanges - 1);

                PrintBooks(arrangement, dividers[arrangementLength, numberOfRanges], arrangementLength);
            }

        }

        static void PrintBooks(int[] arrangement, int start, int end) 
        {
            using (var streamWriter = new StreamWriter("output.txt"))
            {
                for (var i = start; i < end; i++)
                {
                    streamWriter.Write(" " + arrangement[i] + " ");
                }

                streamWriter.WriteLine();
            }

        }
  
     }
}
