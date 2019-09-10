// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerfAssert.cs" company="Microsoft Corporation">
//   Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DelMe.NBench.Demo.PerfAssert.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;

    using DelMe.NBench.Demo.PerfAssert.Library.Utils.Expressions.Expressions;

    public static partial class PerfAssert
    {
        public static PerfAssertObjectWithSource<T> That<T>(Expression<Action<T>> perfBenchmark, BenchmarkRunCache benchmarkRunCache)
        {
            return new PerfAssertObjectWithSource<T>(
                sourceName: ExpressionUtils.GetPerfBenchmarkName(perfBenchmark),
                benchmarkRunCache: benchmarkRunCache);
        }
    }

    /// <summary>
    /// Context related methods
    /// </summary>
    public static partial class PerfAssert
    {
        // TODO: AsyncLocal
        public static PerfAssertContext Context { get; } = new PerfAssertContext();

        public static PerfAssertObjectWithSource<T> That<T>(Expression<Action<T>> perfBenchmark)
        {
            return That(perfBenchmark, PerfAssertContext.RunCache.Value);
        }
    }

    public class PerfAssertContext
    {
        // TODO: 'Context' class should be instantiated, and non-static
        internal static AsyncLocal<BenchmarkRunCache> RunCache = new AsyncLocal<BenchmarkRunCache>();

        internal static AsyncLocal<Action<string> /* TODO: Delegates express more info than Actions */> AssertMethod = new AsyncLocal<Action<string>>
        {
            Value = message => throw new Exception(message)
        };

        internal static Action<bool> AssertIsFalse => predicate => AssertIsTrue(!predicate);
        internal static Action<bool, string> AssertIsFalse2 => (predicate, message) => AssertIsTrue2(!predicate, message);

        internal static Action<bool> AssertIsTrue => predicate =>
        {
            if (!predicate)
            {
                throw new Exception("Expected condition to be true");
            }
        };

        internal static Action<bool, string> AssertIsTrue2 => (predicate, message) =>
        {
            if (!predicate)
            {
                throw new Exception(message);
            }
        };

        public IDisposable UsingAssertionViolatedMethod(Action<string> action)
        {
            // TODO: Impl properly
            AssertMethod.Value = action;
            // TODO: Impl
            return new List<string>().GetEnumerator() /* just a fake idisposable */;
        }

        public IDisposable UsingBenchmarkRunner(BenchmarkRunCache instance)
        {
            // TODO: Impl properly
            RunCache.Value = instance;
            // TODO: Impl
            return new List<string>().GetEnumerator() /* just a fake idisposable */;
        }
    }
}