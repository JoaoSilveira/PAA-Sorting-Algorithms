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
                Sort(ref collection, middle + 1, end, ref rep);
            }

            var arr = new T[end - init];
            var left = init;
            var right = middle;

            for (var i = 0; i < arr.Length; i++)
            {
                if (collection[left].CompareTo(collection[right]) < 0)
                {
                    arr[i] = collection[left];
                    ++left;
                }
                else
                {
                    arr[i] = collection[right];
                    ++right;
                }
                
                if (left == middle)
                {
                    for (var j = right; j < end; j++, i++)
                        arr[i] = collection[j];
                    break;
                }

                if (right != end) continue;

                for (var j = left; j < middle; j++, i++)
                    arr[i] = collection[j];
                break;
            }

            Array.Copy(arr, 0, collection, init, arr.Length);
            //for (var i = init + 1; i < end; i++)
            //{
            //    var entry = collection[i];
            //    var j = i;

            //    while (collection[j - 1].CompareTo(entry) > 0)
            //    {
            //        ++rep.Comparations;

            //        ++rep.Swaps;
            //        collection[j] = collection[--j];

            //        if (j != init) continue;

            //        --rep.Comparations;
            //        break;
            //    }
            //    ++rep.Comparations;
            //    collection[j] = entry;
            //}
        }
    }
}