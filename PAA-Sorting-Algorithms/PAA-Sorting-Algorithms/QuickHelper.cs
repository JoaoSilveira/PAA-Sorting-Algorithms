using System;

namespace PAA_Sorting_Algorithms
{
    public static class QuickHelper<T> where T : IComparable
    {
        public static void Sort(ref T[] collection, int init, int end, ref SortStatistics rep)
        {
            while (init < end)
            {
                var left = init + 1;
                var right = end;

                var pivot = collection[left];

                while (left <= right)
                {
                    if (collection[left].CompareTo(pivot) <= 0)
                    {
                        ++left;
                        ++rep.Comparations;
                        continue;
                    }
                    rep.Comparations++;

                    if (collection[right].CompareTo(pivot) > 0)
                    {
                        --right;
                        ++rep.Comparations;
                        continue;
                    }
                    ++rep.Comparations;
                    ++rep.Swaps;

                    var temp = collection[left];
                    collection[left] = collection[right];
                    collection[right] = temp;

                    ++left;
                    --right;
                }

                ++rep.Swaps;
                collection[init] = collection[right];
                collection[right] = pivot;

                var pos = right;

                if (pos - init < end - pos)
                {
                    Sort(ref collection, init, pos - 1, ref rep);
                    init = pos + 1;
                }
                else
                {
                    Sort(ref collection, pos + 1, end, ref rep);
                    end = pos - 1;
                }
            }
        }
    }
}