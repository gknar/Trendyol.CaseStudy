using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Trendyol.CaseStudy.Common.Exceptions;
using Trendyol.CaseStudy.Common.Messages;
using Trendyol.CaseStudy.Messaging.Configurations;
using Trendyol.CaseStudy.Messaging.Configurator;
using Trendyol.CaseStudy.Test.Bus;
using Xunit;

namespace Trendyol.CaseStudy.Test
{


    public class BusConfiguratorTest : IClassFixture<BusConfiguratorFixture>
    {

        private static BusConfiguratorFixture _busConfiguratorFixture;



        public BusConfiguratorTest(BusConfiguratorFixture busConfiguratorFixture)
        {
            _busConfiguratorFixture = busConfiguratorFixture;
        }

        [Fact]
        public void Publish_ShouldAssertTrue_WhenPublishMessage()
        {
            _busConfiguratorFixture._busControl.Publish<ITestMessageCommand>(new TestMessageCommand()
            {
                Message = "Publish_ShouldAssertTrue_WhenPublishMessage"
            });

            Thread.Sleep(50000);

            Assert.Contains("Publish_ShouldAssertTrue_WhenPublishMessage", _busConfiguratorFixture._testConsumer.MessageList);
        }

        [Fact]
        public void Send_ShouldAssertTrue_WhenSendMessage()
        {
            var endpoint = GetSendEndpoint().Result;

            endpoint.Send<ITestMessageCommand>(new TestMessageCommand()
            {
                Message = "Send_ShouldAssertTrue_WhenSendMessage"
            });

            Thread.Sleep(10000);

            Assert.Contains("Send_ShouldAssertTrue_WhenSendMessage", _busConfiguratorFixture._testConsumer.MessageList);
        }

        private async Task<ISendEndpoint> GetSendEndpoint()
        {
            var endpoint = new Uri($"{_busConfiguratorFixture._busConfigurations.HostName}/{_busConfiguratorFixture.queueName}");

            return await _busConfiguratorFixture._busControl.GetSendEndpoint(endpoint);
        }

    }
}
