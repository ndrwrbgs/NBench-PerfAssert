namespace DelMe.NBench.Demo.PerfAssert.Library {
    public class PerfAssertObjectWithSource<T>
    {
        internal BenchmarkRunCache BenchmarkRunCache { get;}
        internal string SourceName { get; }

        public PerfAssertObjectWithSource(string sourceName, BenchmarkRunCache benchmarkRunCache)
        {
            this.BenchmarkRunCache = benchmarkRunCache;
            this.SourceName = sourceName;
        }
    }
}