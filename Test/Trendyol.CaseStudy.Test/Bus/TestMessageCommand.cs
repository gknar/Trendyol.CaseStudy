using System;
using System.Collections.Generic;
using System.Text;

namespace Trendyol.CaseStudy.Test.Bus
{
   public interface ITestMessageCommand
    {
        string Message { get; set; }

    }
    public class TestMessageCommand : ITestMessageCommand
    {
        public string Message { get; set; }
    }
}
