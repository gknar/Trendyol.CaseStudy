using System;

namespace Trendyol.CaseStudy.Common.Exceptions
{
    public class HandledErrorException : Exception
    {
        public HandledErrorException(string message) : base(message)
        {
        }
    }
}