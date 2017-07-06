using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PAA_Sorting_Algorithms
{
    public class Program
    {
        public static readonly Dictionary<string, string> Order;

        static Program()
        {
            Order = new Dictionary<string, string>
            {
                {"a", "Aleatory"},
                {"d", "Decrescent"},
                {"po", "Partially Ordered"},
                {"o", "Ordered"}
            };
        }

        static void Main(string[] args)
        {
            //if (args.Length != 1)
            //{
            //    Console.WriteLine("Write the path to the data directory");
            //    return;
            //}

            foreach (var file in Directory.GetFiles(@"C:\Users\Malaquias\Source\Repos\PAA-Sorting-Algorithms\PAA-Sorting-Algorithms\PAA-Sorting-Algorithms\Data"))
            {
                var arr = File.ReadAllLines(file).Select(q => Convert.ToInt32(q)).ToArray();

                var rep = SortingTypes<int>.MergeSort(ref arr);

                for (var i = 0; i < arr.Length - 1; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        Console.WriteLine("Deu ruim");
                        break;
                    }
                }
                Console.WriteLine($"{Path.GetFileNameWithoutExtension(file)} || {rep.Comparations} - {rep.Swaps} - {rep.Time}");
            }

            //Thread.CurrentThread.Priority = ThreadPriority.Highest;
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            //foreach (var filePath in Directory.GetFiles(args[0]))
            //{
            //    using (var file = File.OpenRead(filePath))
            //    {
            //        var match = Regex.Match(Path.GetFileNameWithoutExtension(filePath),
            //            @"^(?<order_type>[a-zA-Z]+)(?<vector_length>\d+)$");
            //        var orderType = match.Groups["order_type"].Value;
            //        var vectorLength = Convert.ToInt32(match.Groups["vector_length"].Value);

            //        RunFile(file, Order[orderType], vectorLength);
            //    }
            //}
        }

        private static void RunFile(Stream file, string order, int vectorLength)
        {
            var originalVector = new int[vectorLength];

            using (var reader = new StreamReader(file))
            {
                for (var i = 0; i < vectorLength; i++)
                {
                    originalVector[i] = Convert.ToInt32(reader.ReadLine());
                }
            }

            foreach (var methodInfo in typeof(SortingTypes<int>).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var rep = new SortReport(Regex.Replace(methodInfo.Name, "(\\B[A-Z])", " $1"), vectorLength, order);
                var copy = new int[vectorLength];
                Array.Copy(originalVector, copy, vectorLength);

                Console.WriteLine("{0} {1} - Started at {2}", rep.Algorithm, rep.VectorSize, DateTime.Now);

                rep.Statistics = (SortStatistics)methodInfo.Invoke(null, new object[] { copy });

                SaveReport(rep);
            }
        }

        private static void SaveReport(SortReport rep)
        {
            File.AppendAllLines($"{rep.OrderType}.txt", new[] { $"{rep.Algorithm} - {rep.VectorSize} - {rep.Statistics.Comparations} - {rep.Statistics.Swaps} - {rep.Statistics.Time}" });
        }
    }
}
