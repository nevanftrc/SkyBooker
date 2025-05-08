using System.Text;
using RabbitMQ.Client;

namespace BookService.Messaging
{
    public class RabbitMqPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5673 // asegúrate de usar el puerto correcto configurado en Docker
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "bookingQueue",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "bookingQueue",
                                  basicProperties: null,
                                  body: body);
        }

        ~RabbitMqPublisher()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
