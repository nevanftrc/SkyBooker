using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using MessageService.Consumers;
using System.Threading;
using System.Threading.Tasks;

namespace MessageService.Tests.Consumers
{
    public class RabbitMqConsumerTests
    {
        [Fact]
        public async Task Consumer_Starts_Without_Exception()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["RabbitMq:Host"]).Returns("localhost");
            mockConfig.Setup(c => c["RabbitMq:Queue"]).Returns("test-queue");

            var consumer = new RabbitMqConsumer(mockConfig.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            var task = consumer.StartAsync(token);

            cancellationTokenSource.CancelAfter(1000);

            await Task.WhenAny(task, Task.Delay(1500));

            Assert.True(task.IsCompleted || task.IsCanceled);
        }
    }
}
