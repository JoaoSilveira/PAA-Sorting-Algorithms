using System;

namespace PAA_Sorting_Algorithms
{
    public static class QuickHelper<T> where T : IComparable
    {
        public static void Sort(ref T[] collection, int init, int end, ref SortStatistics rep)
        {
            var left = init;
            var right = end;
            var pivot = collection[(init + end) / 2];

            while (left <= right)
            {
                while (collection[left].CompareTo(pivot) < 0)
                {
                    left++;
                    rep.Comparations++;
                }
                rep.Comparations++;

                while (collection[right].CompareTo(pivot) > 0)
                {
                    right--;
                    rep.Comparations++;
                }
                rep.Comparations++;

                if (left > right) continue;

                rep.Swaps++;

                var tmp = collection[left];
                collection[left] = collection[right];
                collection[right] = tmp;

                left++;
                right--;
            }

            if (init < right)
            {
                Sort(ref collection, init, right, ref rep);
            }
            if (left < end)
            {
                Sort(ref collection, left, end, ref rep);
            }
        }
    }
}