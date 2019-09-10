// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerfAssertThatExtensions.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DelMe.NBench.Demo.PerfAssert.Library {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using DelMe.NBench.Demo.PerfAssert.Library.Utils.Expressions.Expressions;

    using global::NBench.Metrics.Timing;
    using global::NBench.Reporting;

    /// <summary>
    /// 
    /// TODO: COPY PASTE!!!
    /// 
    /// </summary>
    public static partial class PerfAssertThatExtensions
    {
        // TODO: COPY PASTE!!!
        #region Different types of assertions (memory/speed/etc)

        // TODO: COPY PASTE!!!
        public static PerfAssertObjectWithSourceAndTarget<T> FasterThan<T>(
            this PerfAssertObjectWithSource<T> source,
            Expression<Action<T>> perfBenchmark)
        {
            // TODO: COPY PASTE!!!
            string targetName = ExpressionUtils.GetPerfBenchmarkName(perfBenchmark);

            var perfAssert = new PerfAssertObjectWithSourceAndTarget<T>(
                source.SourceName,
                targetName: targetName,
                benchmarkRunCache: source.BenchmarkRunCache);

            var results = source.BenchmarkRunCache.GetResults(perfAssert);

            var speedMetricsByBenchmark = results
                .ToDictionary(
                    result => result.BenchmarkName,
                    result =>
                    {
                        AggregateMetrics timingMetricValues = result.StatsByMetric
                            .Single /* TODO: Error handling to help developers with usage */(kvp => kvp.Key is TimingMetricName /* Counter, Gc, Memory, Timing */)
                            .Value;
                        return timingMetricValues;
                    });
            var speedMetricForSource = speedMetricsByBenchmark[source.SourceName] /* TODO: Error handling */;
            var speedMetricForTarget = speedMetricsByBenchmark[targetName] /* TODO: Error handling */;

            // Run the validation, and wrap the exception with a meaningful message
            // TODO: Pass the 'meaningful message' down so we call the user delegate with it
            //AssertionFailedExceptionWrapper.Wrap(
            //    () =>
                    AggregateMetricsAssertions.FirstIsFaster(
                        first: speedMetricForSource,
                        second: speedMetricForTarget)
                        ;
                //        ,
                //ex => new Exception($"Expected {source.SourceName} to be faster than {targetName}", ex));

            return perfAssert;
        }

        /// <summary>
        /// TODO: Would prefer NotWorseThan(X, InTermsOf.Runtime), but leaving for later
        /// </summary>
        public static PerfAssertObjectWithSourceAndTarget<T> NotWorseRuntimeThan<T>(
            this PerfAssertObjectWithSource<T> source,
            Expression<Action<T>> perfBenchmark)
        {
            // TODO: COPY PASTE!!!
            string targetName = ExpressionUtils.GetPerfBenchmarkName(perfBenchmark);

            var perfAssert = new PerfAssertObjectWithSourceAndTarget<T>(
                source.SourceName,
                targetName: targetName,
                benchmarkRunCache: source.BenchmarkRunCache);

            var results = source.BenchmarkRunCache.GetResults(perfAssert);

            var speedMetricsByBenchmark = results
                .ToDictionary(
                    result => result.BenchmarkName,
                    result =>
                    {
                        AggregateMetrics timingMetricValues = result.StatsByMetric
                            .Single /* TODO: Error handling to help developers with usage */(kvp => kvp.Key is TimingMetricName /* Counter, Gc, Memory, Timing */)
                            .Value;
                        return timingMetricValues;
                    });
            var speedMetricForSource = speedMetricsByBenchmark[source.SourceName] /* TODO: Error handling */;
            var speedMetricForTarget = speedMetricsByBenchmark[targetName] /* TODO: Error handling */;

            // Run the validation, and wrap the exception with a meaningful message
            //AssertionFailedExceptionWrapper.Wrap(
            //    () =>
                    AggregateMetricsAssertions.FirstIsNotSlower(
                        first: speedMetricForSource,
                        second: speedMetricForTarget)
                        ;
                //        ,
                //ex => new AssertFailedException($"Expected {source.SourceName} to not be slower than {targetName}.\r\n"
                //                                + $"{ex.Message}", ex));
            //AssertionFailedExceptionWrapper.Wrap(
            //    () =>
                    AggregateMetricsAssertions.FirstDoesNotHaveHigherVariance(
                        first: speedMetricForSource,
                        second: speedMetricForTarget)
                        ;
                        //,
                //ex => new AssertFailedException(
                //    $"Expected {source.SourceName} to not have more variance(/noise)/less-reliability(/reproducibility) than {targetName}.\r\n"
                //    + $"{ex.Message}",
                //    ex));

            return perfAssert;
        }

        #endregion
    }
}