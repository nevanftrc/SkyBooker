using RabbitMQ.Client;
using System.Text;

namespace BookService.Messaging;

public class RabbitMqPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqPublisher()
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "booking-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Send(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "", routingKey: "booking-queue", body: body);
    }
}
