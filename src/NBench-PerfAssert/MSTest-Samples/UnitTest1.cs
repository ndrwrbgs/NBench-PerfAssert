using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest_Samples
{
    using System.Threading;

    using DelMe.NBench.Demo.PerfAssert.Library;

    using NBench;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Could set globally, but that would cause issues with concurrency
            // Though, we probably don't want concurrent execution during benchmarks, huh? :)
            using (PerfAssert.Context.UsingAssertionViolatedMethod((string message) => Assert.Fail(message)))
            using (PerfAssert.Context.UsingBenchmarkRunner(BenchmarkRunCache.Instance))
            {
                PerfAssert.That<UnitTest1>(t => t.Faster())
                    .Is().FasterThan(t => t.Slower());
            }
        }

        [PerfBenchmark /* TODO: We would like that this only requires a type from our library, but permit types from NBench for ease */]
        [ElapsedTimeAssertion]
        public /* TODO: We would like to hide these methods, but currently required exposed by NBench */ void Faster()
        {
            Thread.Sleep(10);
        }

        [PerfBenchmark]
        [ElapsedTimeAssertion]
        public void Slower()
        {
            Thread.Sleep(100);
        }
    }
}
