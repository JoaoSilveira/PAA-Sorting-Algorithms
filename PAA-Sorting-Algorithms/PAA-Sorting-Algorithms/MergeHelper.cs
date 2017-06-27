using System;

namespace PAA_Sorting_Algorithms
{
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
}