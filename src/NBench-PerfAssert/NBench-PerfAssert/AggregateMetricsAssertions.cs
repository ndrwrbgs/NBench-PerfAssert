namespace DelMe.NBench.Demo.PerfAssert.Library {
    using System.Diagnostics;
    using System.Linq;

    // TODO: Abstract out the statistical tests to allow other non-Accord implementations
    using Accord.Statistics;
    using Accord.Statistics.Testing;

    using global::NBench.Reporting;

    internal static class AggregateMetricsAssertions
    {
        // TODO: Try with other metric types, like memory, since this might not actually be generic/accurate for all AggregateMetrics but only for ElapsedTime
        public static void FirstIsFaster(AggregateMetrics first, AggregateMetrics second)
        {
            /*/
            DummyNotStatisticalTest(first, second);
            /*/
            RealTest(first, second);
            //*/
        }

        private static void RealTest(AggregateMetrics first, AggregateMetrics second)
        {
            // Wilcoxon test to not assume normal distributions

            var mannWhitneyWilcoxonTest = new MannWhitneyWilcoxonTest(
                first.Runs.Select(run => run.ElapsedNanos).ToArray(),
                second.Runs.Select(run => run.ElapsedNanos).ToArray(),
                TwoSampleHypothesis.FirstValueIsSmallerThanSecond);

            Trace.WriteLine(
                "MannWWT s1 < s2:  " + mannWhitneyWilcoxonTest.PValue +
                " Stat1: " + mannWhitneyWilcoxonTest.Statistic1 + "; Stat2: " + mannWhitneyWilcoxonTest.Statistic2 +
                " - Significant: " + mannWhitneyWilcoxonTest.Significant + " - Hyp: " + mannWhitneyWilcoxonTest.Hypothesis);

            PerfAssertContext.AssertIsTrue(mannWhitneyWilcoxonTest.Significant);
        }

        private static void DummyNotStatisticalTest(AggregateMetrics first, AggregateMetrics second)
        {
            // TODO: Dummy NOT statistical test
            var firstAvg = first.Runs.Average(run => run.ElapsedNanos);
            var secondAvg = second.Runs.Average(run => run.ElapsedNanos);

            PerfAssertContext.AssertIsTrue2(firstAvg < secondAvg, "Expected first to be faster (less time) than second");
        }

        public static void FirstIsNotSlower(AggregateMetrics first, AggregateMetrics second)
        {
            double[] firstSamples = first.Runs.Select(run => run.ElapsedNanos).ToArray();
            double[] secondSamples = second.Runs.Select(run => run.ElapsedNanos).ToArray();

            // Wilcoxon test to not assume normal distributions

            var mannWhitneyWilcoxonTest = new MannWhitneyWilcoxonTest(
                firstSamples,
                secondSamples,
                TwoSampleHypothesis.FirstValueIsGreaterThanSecond);

            Trace.WriteLine($"Sample 1 mean: {firstSamples.Average()} Sample 2 mean: {secondSamples.Average()}");
            Trace.WriteLine(
                "MannWWT s1 > s2:  " + mannWhitneyWilcoxonTest.PValue +
                " Stat1: " + mannWhitneyWilcoxonTest.Statistic1 + "; Stat2: " + mannWhitneyWilcoxonTest.Statistic2 +
                " - Significant: " + mannWhitneyWilcoxonTest.Significant + " - Hyp: " + mannWhitneyWilcoxonTest.Hypothesis);

            // We want to assert that we CANNOT statistically say that first > second (it can be less than, equal, or it might be greater than but not
            // with statistical significance -- the only thing it CAN'T be, is KNOWN to be slower [>])
            PerfAssertContext.AssertIsFalse(mannWhitneyWilcoxonTest.Significant);
        }

        public static void FirstDoesNotHaveHigherVariance(AggregateMetrics first, AggregateMetrics second)
        {
            double[] firstSamples = first.Runs.Select(run => run.ElapsedNanos).ToArray();
            double[] secondSamples = second.Runs.Select(run => run.ElapsedNanos).ToArray();
            
            // Fisher's F-test (also known as Snedecor)
            var firstVariance = Measures.Variance(firstSamples);
            var secondVariance = Measures.Variance(secondSamples);
            var fishersFTest = new FTest(
                firstVariance,
                secondVariance,
                firstSamples.Length - 1,
                secondSamples.Length - 1,
                TwoSampleHypothesis.FirstValueIsGreaterThanSecond);

            Trace.WriteLine(
                "FTest Var(s1) > Var(s2):  " + fishersFTest.PValue +
                " - Significant: " + fishersFTest.Significant + " - Hyp: " + fishersFTest.Hypothesis);

            PerfAssertContext.AssertIsFalse(fishersFTest.Significant);
        }
    }
}