using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Trendyol.CaseStudy.Common.Messages;
using System.Linq;
using Trendyol.CaseStudy.Common.Exceptions;

namespace Trendyol.CaseStudy.Test.Bus
{
    public class TestMailCommandConsumer : IConsumer<ITestMessageCommand>
    {

        public TestMailCommandConsumer()
        {
            MessageList = new List<string>();
        }
        public List<string> MessageList { get; set; }

        public Task Consume(ConsumeContext<ITestMessageCommand> context)
        {
  

            MessageList.Add(context.Message.Message);

            return  context.CompleteTask;
        }
    }
}
