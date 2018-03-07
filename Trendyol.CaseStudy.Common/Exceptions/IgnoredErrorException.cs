using System;

namespace Trendyol.CaseStudy.Common.Exceptions
{
    public class IgnoredErrorException : Exception
    {
        public IgnoredErrorException(string message) : base(message)
        {
        }
    }
}