using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PAA_Sorting_Algorithms
{
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
}