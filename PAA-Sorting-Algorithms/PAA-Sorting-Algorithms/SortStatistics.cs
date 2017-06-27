using System;

namespace PAA_Sorting_Algorithms
{
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
}