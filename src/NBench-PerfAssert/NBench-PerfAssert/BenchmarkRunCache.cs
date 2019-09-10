using System.Text;

namespace DelMe.NBench.Demo.PerfAssert.Library
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;

    using DelMe.NBench.Demo.PerfAssert.Library.Utils.Expressions.Expressions;

    using global::NBench.Reporting;
    using global::NBench.Reporting.Targets;
    using global::NBench.Sdk;
    using global::NBench.Sdk.Compiler;

    public sealed class BenchmarkRunCache
    {
        private static readonly IBenchmarkOutput defaultOutput = new ConsoleBenchmarkOutput();
        public static BenchmarkRunCache Instance { get; } = new BenchmarkRunCache(defaultOutput);

        private ConcurrentDictionary<string, BenchmarkResults> cachedResults = new ConcurrentDictionary<string, BenchmarkResults>();
        private IDiscovery discovery;

        private BenchmarkRunCache(IBenchmarkOutput output)
        {
            discovery = new ReflectionDiscovery(output);
        }

        public BenchmarkResults GetResults(MethodInfo benchmarkMethod)
        {
            var containerType = benchmarkMethod.DeclaringType;
            var methodName = benchmarkMethod.Name;
            var benchmarkName = $"{containerType.FullName}+{methodName}";

            return cachedResults
                .GetOrAdd(
                    benchmarkName,
                    _ =>
                    {

                        var benchmarksInContainer = discovery.FindBenchmarks(containerType);
                        var matchingBenchmark = benchmarksInContainer.Single(b => b.BenchmarkName == benchmarkName);
                        
                        Benchmark.PrepareForRun();
                        matchingBenchmark.Run();

                        var exceptions = matchingBenchmark.CompileResults().Exceptions;
                        if (exceptions.Any())
                        {
                            // TODO: record ALL exceptions
                            throw exceptions.First(); // TODO: REthrow
                        }

                        return matchingBenchmark.CompileResults();
                    });
        }
    }
}
