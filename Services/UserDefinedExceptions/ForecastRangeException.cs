using System;

namespace Services.UserDefinedExceptions
{
    public class ForecastRangeException : Exception
    {
        public ForecastRangeException()
        {
        }

        public ForecastRangeException(string message)
            : base(message)
        {
        }

        public ForecastRangeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
