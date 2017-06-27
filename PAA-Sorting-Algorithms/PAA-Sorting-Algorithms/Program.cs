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
        public delegate SortStatistics Sort<T>(ref T[] collection) where T : IComparable;

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
            if (args.Length != 1)
            {
                Console.WriteLine("Write the path to the data directory");
                return;
            }

            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            foreach (var filePath in Directory.GetFiles(args[0]))
            {
                using (var file = File.OpenRead(filePath))
                {
                    var match = Regex.Match(Path.GetFileNameWithoutExtension(filePath),
                        @"^(?<order_type>[a-zA-Z]+)(?<vector_length>\d+)$");
                    var orderType = match.Groups["order_type"].Value;
                    var vectorLength = Convert.ToInt32(match.Groups["vector_length"].Value);

                    RunFile(file, Order[orderType], vectorLength);
                }
            }
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

                Console.WriteLine("{0} {1}", rep.Algorithm, rep.VectorSize);

                rep.Statistics = (SortStatistics)methodInfo.Invoke(null, new object[] { copy });

                SaveReport(rep);
            }
        }

        private static void SaveReport(SortReport rep)
        {
            File.AppendAllLines($"{rep.OrderType}.txt", new[] { $"{rep.Algorithm} - {rep.VectorSize} - {rep.Statistics.Comparations} - {rep.Statistics.Swaps} - {rep.Statistics.Time}" });
        }
    }

    public class SortReport
    {
        public SortStatistics Statistics { get; set; }

        public string Algorithm { get; set; }

        public int VectorSize { get; set; }

        public string OrderType { get; set; }

        public SortReport(string algorithm, int vectorSize, string orderType)
        {
            Algorithm = algorithm;
            VectorSize = vectorSize;
            OrderType = orderType;
        }
    }

    public class SortStatistics
    {
        public int Swaps { get; set; }
        public int Comparations { get; set; }
        public TimeSpan Time { get; set; }

        public SortStatistics()
        {
            Swaps = 0;
            Comparations = 0;
            Time = TimeSpan.Zero;
        }
    }

    public static class SortingTypes<T> where T : IComparable
    {
        public static SortStatistics BubbleSort(ref T[] collection)
        {
            var rep = new SortStatistics();

            bool madeChanges;
            var itemCount = collection.Length;

            var watch = Stopwatch.StartNew();

            watch.Start();
            do
            {
                madeChanges = false;
                --itemCount;
                for (var i = 0; i < itemCount; i++)
                {
                    ++rep.Comparations;

                    if (collection[i].CompareTo(collection[i + 1]) <= 0) continue;

                    var temp = collection[i + 1];
                    collection[i + 1] = collection[i];
                    collection[i] = temp;

                    madeChanges = true;
                    ++rep.Swaps;
                }
            } while (madeChanges);
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortStatistics InsertionSort(ref T[] collection)
        {
            var rep = new SortStatistics();

            var watch = Stopwatch.StartNew();

            watch.Start();
            for (var i = 0 + 1; i < collection.Length; i++)
            {
                var entry = collection[i];
                var j = i;

                while (collection[j - 1].CompareTo(entry) > 0)
                {
                    ++rep.Comparations;

                    ++rep.Swaps;
                    collection[j] = collection[--j];

                    if (j != 0) continue;

                    --rep.Comparations;
                    break;
                }
                ++rep.Comparations;

                collection[j] = entry;
            }
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortStatistics SelectionSort(ref T[] collection)
        {
            var rep = new SortStatistics();

            var watch = Stopwatch.StartNew();

            watch.Start();
            for (var i = 0; i < collection.Length; i++)
            {
                var k = i;
                for (var j = i + 1; j < collection.Length; j++)
                {
                    ++rep.Comparations;
                    if (collection[j].CompareTo(collection[k]) < 0)
                    {
                        k = j;
                    }
                }

                var temp = collection[i];
                collection[i] = collection[k];
                collection[k] = temp;

                ++rep.Swaps;
            }
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortStatistics MergeSort(ref T[] collection)
        {
            var rep = new SortStatistics();

            var watch = Stopwatch.StartNew();

            watch.Start();
            MergeHelper<T>.Sort(ref collection, 0, collection.Length, ref rep);
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortStatistics QuickSort(ref T[] collection)
        {
            var rep = new SortStatistics();

            var watch = Stopwatch.StartNew();

            watch.Start();
            QuickHelper<T>.Sort(ref collection, 0, collection.Length - 1, ref rep);
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortStatistics CountingSort(ref int[] collection)
        {
            var rep = new SortStatistics();

            var watch = Stopwatch.StartNew();

            var max = collection.Max();
            var min = collection.Min();

            var count = new int[max - min + 1];
            var z = 0;

            for (var i = 0; i < count.Length; i++) { count[i] = 0; }

            watch.Start();
            foreach (var t in collection)
                count[t - min]++;

            for (var i = min; i <= max; i++)
            {
                while (count[i - min]-- > 0)
                {
                    collection[z] = i;
                    z++;
                }
            }
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortStatistics BucketSort(ref int[] collection)
        {
            var rep = new SortStatistics();

            var watch = Stopwatch.StartNew();

            var minValue = collection.Min();
            var maxValue = collection.Max();

            var buckets = new List<int>[maxValue - minValue + 1];

            for (var i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new List<int>();
            }

            watch.Start();
            foreach (var t in collection)
            {
                buckets[t - minValue].Add(t);
            }

            var k = 0;
            foreach (var bucket in buckets)
            {
                if (bucket.Count <= 0) continue;

                foreach (var t in bucket)
                {
                    collection[k] = t;
                    k++;
                }
            }
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }
    }

    public static class MergeHelper<T> where T : IComparable
    {
        public static void Sort(ref T[] collection, int init, int end, ref SortStatistics rep)
        {
            var middle = (init + end) / 2;

            if (end - init > 1)
            {
                Sort(ref collection, init, middle, ref rep);
                Sort(ref collection, middle, end, ref rep);
            }

            for (var i = init + 1; i < end; i++)
            {
                var entry = collection[i];
                var j = i;

                while (collection[j - 1].CompareTo(entry) > 0)
                {
                    ++rep.Comparations;

                    ++rep.Swaps;
                    collection[j] = collection[--j];

                    if (j != init) continue;

                    --rep.Comparations;
                    break;
                }
                ++rep.Comparations;
                collection[j] = entry;
            }
        }
    }

    public static class QuickHelper<T> where T : IComparable
    {
        public static void Sort(ref T[] collection, int init, int end, ref SortStatistics rep)
        {
            if (init >= end) return;

            var pivot = Partition(ref collection, init, end, ref rep);

            if (pivot > 1)
            {
                Sort(ref collection, init, pivot - 1, ref rep);
            }
            if (pivot + 1 < end)
            {
                Sort(ref collection, pivot + 1, end, ref rep);
            }
        }

        private static int Partition(ref T[] collection, int left, int right, ref SortStatistics rep)
        {
            var pivot = collection[left];
            while (true)
            {

                while (collection[left].CompareTo(pivot) < 0)
                {
                    left++;
                    ++rep.Comparations;
                }
                ++rep.Comparations;

                while (collection[right].CompareTo(pivot) > 0)
                {
                    right--;
                    ++rep.Comparations;
                }
                ++rep.Comparations;

                if (left < right)
                {
                    ++rep.Comparations;
                    if (collection[left].CompareTo(collection[right]) == 0) return right;

                    var temp = collection[left];
                    collection[left] = collection[right];
                    collection[right] = temp;

                    rep.Swaps++;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
