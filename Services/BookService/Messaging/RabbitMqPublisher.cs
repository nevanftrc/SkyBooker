using RabbitMQ.Client;
using System.Text;

namespace BookService.Messaging;

public class RabbitMqPublisher
{
    private readonly IConfiguration _config;

    public RabbitMqPublisher(IConfiguration config)
    {
        _config = config;
    }

    public void Publish(string message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQ:Host"],
            UserName = _config["RabbitMQ:Username"],
            Password = _config["RabbitMQ:Password"],
            Port = int.Parse(_config["RabbitMQ:Port"] ?? "5672")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "bookingQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: "bookingQueue",
                             basicProperties: null,
                             body: body);
    }
}
