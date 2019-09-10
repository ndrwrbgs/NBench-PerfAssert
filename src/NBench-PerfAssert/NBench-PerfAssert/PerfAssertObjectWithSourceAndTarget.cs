namespace DelMe.NBench.Demo.PerfAssert.Library {
    public class PerfAssertObjectWithSourceAndTarget<T> : PerfAssertObjectWithSource<T>
    {
        internal string TargetName { get; }

        public PerfAssertObjectWithSourceAndTarget(string sourceName, string targetName, BenchmarkRunCache benchmarkRunCache)
            : base(sourceName, benchmarkRunCache)
        {
            this.TargetName = targetName;
        }
    }
}