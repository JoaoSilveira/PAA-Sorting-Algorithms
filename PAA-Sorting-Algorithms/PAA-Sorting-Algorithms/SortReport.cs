namespace PAA_Sorting_Algorithms
{
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
}