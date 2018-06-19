using System;

namespace Alpaca.Markets
{
    public class RestClientErrorException : Exception
    {
        internal RestClientErrorException(
            JsonError error)
            : base(error.Message)
        {
            ErrorCode = error.Code;
        }

        public Int32 ErrorCode { get; }
    }
}