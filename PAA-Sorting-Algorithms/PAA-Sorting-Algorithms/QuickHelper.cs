using System;

namespace PAA_Sorting_Algorithms
{
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