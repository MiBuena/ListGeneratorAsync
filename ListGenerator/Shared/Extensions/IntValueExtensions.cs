using System;
using System.Collections.Generic;
using System.Text;

namespace ListGenerator.Shared.Extensions
{
    public static class IntValueExtensions
    {
        public static void ThrowIfZeroOrNegative(this int value)
        {
            if (value <= 0)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        public static void ThrowIfZeroOrNegative(this int value, string errorMessage)
        {
            if (value <= 0)
            {
                throw new ArgumentNullException(nameof(value), errorMessage);
            }
        }
    }
}
