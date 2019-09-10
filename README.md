# NBench-PerfAssert
Fluent assertions for performance tests and performance comparisons built atop [NBench](https://github.com/petabridge/NBench)

## Targeted usage scenarios
Primary target: small benchmarks where the difference between them is expected to be statistically relevant -- e.g. we strongly expect (in the test execution environment) that treatment is faster than baseline.

## Motivation story
When making performance improvements like [FastLinq](https://github.com/ndrwrbgs/FastLinq) I needed a way to assert speed differences. [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) was the best library for benchmarking at the time of writing, but lacked the ability to run inside a unit test (due to features, actually, as it makes VERY accurate measurements so requires owning the context it runs in, at least today**).

Through market analysis, it was found that [NBench](https://github.com/petabridge/NBench) provides similar functionality but can be run within the same AppDomain (at the cost of a bit of accuracy, likely) so this library usese NBench to run performance tests and compare the results.

    ** Note that https://github.com/dotnet/BenchmarkDotNet/issues/155 and related issues should fix this

## Usage
Note that in my experience, when I put code in the readme.md like this, I let it get stale. Please look for a Samples.cs file in the (hopefully existing) Tests project for the most up to date.

```C#
// Treatment should be faster!
PerfAssert.That((MyTestClass target) => target.Treatment(), BenchmarkRunCache.Instance)
    .Is().FasterThan((MyTestClass target) => target.Baseline());
    
// Treatment doesn't need to be faster, but must NOT be slower
PerfAssert.That((MyTestClass target) => target.Treatment(), BenchmarkRunCache.Instance)
    .Is().NotWorseRuntimeThan((MyTestClass target) => target.Baseline());
```

Generally you'll start with `PerfAssert.That()`, `.Is()` is a utility method for legibility, and does nothing. Extension methods available for the result of `PerfAssert.That()` have xmldoc comments that will show you what they can do, and until/unless the API becomes cemented in stone, this will be the preferred way to identify the usage.

## Future work
Ideally, the engine running the tests would be hidden beneath the covers, and the `PerfAssert` API could be applied to multiple runners. This may come in the future, but isn't worth the investment presently as we only have one implementation.
