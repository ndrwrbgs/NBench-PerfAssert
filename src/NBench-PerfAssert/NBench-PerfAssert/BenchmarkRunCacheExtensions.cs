// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkRunCacheExtensions.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DelMe.NBench.Demo.PerfAssert.Library {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::NBench.Reporting;

    /// <summary>
    /// Just for easy of migrating existing code to the cache based setup
    /// </summary>
    internal static class BenchmarkRunCacheExtensions
    {
        public static IEnumerable<BenchmarkResults> GetResults<T>(
            this BenchmarkRunCache cache,
            PerfAssertObjectWithSourceAndTarget<T> perfAssert)
        {
            var sourceName = perfAssert.SourceName;
            var targetName = perfAssert.TargetName;

            MethodInfo sourceMethod = GetMethodInfoFromBenchmarkName(sourceName);
            MethodInfo targetMethod = GetMethodInfoFromBenchmarkName(targetName);
            return new[]
            {
                cache.GetResults(sourceMethod),
                cache.GetResults(targetMethod),
            };
        }

        private static MethodInfo GetMethodInfoFromBenchmarkName(string benchmarkName)
        {
            // benchmarkName = DelMe.NBench.Demo.XUnit.UnitTest1+Benchmark2
            var beforePlus = benchmarkName.Split('+')[0];
            var afterPlus = benchmarkName.Split('+')[1];

            var methodName = afterPlus;

            var type = AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType(beforePlus))
                .First(resolved => resolved != null);
            var method = type.GetMethod(methodName);
            return method;
        }
    }
}