namespace DelMe.NBench.Demo.PerfAssert.Library {
    public static partial class PerfAssertThatExtensions
    {
        #region Legibility

        /// <summary>
        /// Used for making it read more like english, but not required at all
        /// </summary>
        public static PerfAssertObjectWithSource<T> Is<T>(
            this PerfAssertObjectWithSource<T> source)
        {
            return source;
        }

        #endregion
    }
}