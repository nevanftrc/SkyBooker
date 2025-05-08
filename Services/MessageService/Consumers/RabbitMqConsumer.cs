using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace MessageService.Consumers;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IConfiguration _config;

    public RabbitMqConsumer(IConfiguration config)
    {
        _config = config;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var host = _config["RabbitMq:Host"] ?? "rabbitmq";
        var queue = _config["RabbitMq:Queue"] ?? "booking-queue";
        var toNumber = _config["Twilio:To"] ?? "+41782440574";

        var factory = new ConnectionFactory() { HostName = host };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var nachricht = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[MessageService] Buchung empfangen: {nachricht}");

            // WhatsApp Simulation
            Console.WriteLine($"[WhatsApp] Nachricht an {toNumber} gesendet: {nachricht}");

            // Twilio Simulation
            Console.WriteLine($"[Twilio] SMS an {toNumber} gesendet: {nachricht}");
        };

        channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
}
