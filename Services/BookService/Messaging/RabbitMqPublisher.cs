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
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare("bookingQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("", "bookingQueue", null, body);
    }
}
