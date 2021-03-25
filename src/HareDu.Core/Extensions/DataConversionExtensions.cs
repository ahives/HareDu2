namespace HareDu.Core.Extensions
{
    using System;

    public static class DataConversionExtensions
    {
        public static uint ConvertTo(this int value) => Convert.ToUInt32(value);
    }
}