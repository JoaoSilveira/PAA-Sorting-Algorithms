using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAA_Sorting_Algorithms
{
    public class Program
    {
        public delegate SortReport Sort<T>(ref T[] collection) where T : IComparable;

        static void Main(string[] args)
        {
        }
    }

    public class SortReport
    {
        public int Swaps { get; set; }
        public int Comparations { get; set; }
        public TimeSpan Time { get; set; }

        public SortReport()
        {
            Swaps = 0;
            Comparations = 0;
            Time = TimeSpan.Zero;
        }
    }

    public static class SortingTypes<T> where T : IComparable
    {
        public static SortReport BubbleSort(ref T[] collection)
        {
            var rep = new SortReport();

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

        public static SortReport InsertionSort(ref T[] collection)
        {
            var rep = new SortReport();

            var watch = Stopwatch.StartNew();

            watch.Start();
            for (var i = 0 + 1; i < collection.Length; i++)
            {
                var entry = collection[i];
                var j = i;

                while (collection[j - 1].CompareTo(entry) > 0)
                {
                    ++rep.Comparations;

                    if (j == 0)
                    {
                        --rep.Comparations;
                        break;
                    }

                    ++rep.Swaps;
                    collection[j] = collection[--j];
                }
                ++rep.Comparations;

                collection[j] = entry;
            }
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortReport SelectionSort(ref T[] collection)
        {
            var rep = new SortReport();

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

        public static SortReport MergeSort(ref T[] collection)
        {
            var rep = new SortReport();

            var watch = Stopwatch.StartNew();

            watch.Start();
            MergeHelper<T>.Sort(ref collection, 0, collection.Length, ref rep);
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortReport QuickSort(ref T[] collection)
        {
            var rep = new SortReport();

            var watch = Stopwatch.StartNew();

            watch.Start();
            QuickHelper<T>.Sort(ref collection, 0, collection.Length, ref rep);
            watch.Stop();

            rep.Time = watch.Elapsed;

            return rep;
        }

        public static SortReport CountingSort(ref int[] collection)
        {
            var rep = new SortReport();

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

        public static SortReport BucketSort(ref int[] collection)
        {
            var rep = new SortReport();

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
        public static void Sort(ref T[] collection, int init, int end, ref SortReport rep)
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
                    if (j == init)
                    {
                        --rep.Comparations;
                        break;
                    }

                    ++rep.Swaps;
                    collection[j] = collection[--j];
                }
                ++rep.Comparations;
                collection[j] = entry;
            }
        }
    }

    public static class QuickHelper<T> where T : IComparable
    {
        public static void Sort(ref T[] collection, int init, int end, ref SortReport rep)
        {
            if (end - init < 2)
                return;

            var pivot = (init + end) / 2;
            var right = init;
            var left = end;

            while (left <= right)
            {
                while (collection[left].CompareTo(collection[pivot]) < 0)
                {
                    ++rep.Comparations;
                    ++left;
                }
                ++rep.Comparations;

                while (collection[right].CompareTo(collection[pivot]) > 0)
                {
                    ++rep.Comparations;
                    --right;
                }
                rep.Comparations += 2;

                if (collection[left].CompareTo(collection[right]) >= 0) continue;

                var aux = collection[left];
                collection[left++] = collection[right];
                collection[right--] = aux;

                ++rep.Swaps;
            }

            Sort(ref collection, init, right, ref rep);
            Sort(ref collection, left, end, ref rep);
        }
    }
}
