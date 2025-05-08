using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageService.Consumers;

public class RabbitMqConsumer : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "booking-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[MessageService] Reserva recibida: {message}");
        };

        channel.BasicConsume(queue: "booking-queue", autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
}
