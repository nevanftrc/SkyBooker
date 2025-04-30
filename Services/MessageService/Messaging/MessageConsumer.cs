using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MessageService.Messaging;

public class MessageConsumer : BackgroundService
{
    private readonly IConfiguration _config;

    public MessageConsumer(IConfiguration config)
    {
        _config = config;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQ:Host"],
            UserName = _config["RabbitMQ:Username"],
            Password = _config["RabbitMQ:Password"]
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "bookingQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Received: {message}");

            await SendWhatsAppNotification(message);
        };

        channel.BasicConsume(queue: "bookingQueue", autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    private async Task SendWhatsAppNotification(string message)
    {
        var accountSid = _config["Twilio:AccountSid"];
        var authToken = _config["Twilio:AuthToken"];
        var fromNumber = _config["Twilio:From"];
        var toNumber = _config["Twilio:To"];

        TwilioClient.Init(accountSid, authToken);

        await MessageResource.CreateAsync(
            body: $"Reserva confirmada: {message}",
            from: new PhoneNumber($"whatsapp:{fromNumber}"),
            to: new PhoneNumber($"whatsapp:{toNumber}")
        );
    }
}
