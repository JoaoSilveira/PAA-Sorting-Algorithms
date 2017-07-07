//#define ARRAY_COPY
using System;

namespace PAA_Sorting_Algorithms
{
    public static class MergeHelper<T> where T : IComparable
    {
        public static void Sort(ref T[] collection, int init, int end, ref SortStatistics rep)
        {
            if (end - init < 2)
                return;

            var middle = (init + end) / 2;

            Sort(ref collection, init, middle, ref rep);
            Sort(ref collection, middle, end, ref rep);

            var aux = new T[middle - init];

            Array.Copy(collection, init, aux, 0, aux.Length);

            var left = 0;
            var right = middle;

            int pos;

            for (pos = init; left != aux.Length && right != end; pos++)
            {
                ++rep.Comparations;
                ++rep.Swaps;
                if (aux[left].CompareTo(collection[right]) < 0)
                {
                    collection[pos] = aux[left];
                    ++left;
                }
                else
                {
                    collection[pos] = collection[right];
                    ++right;
                }
            }

            if (right == end)
            {
                Array.Copy(aux, left, collection, pos, aux.Length - left);
            }
        }
    }
}