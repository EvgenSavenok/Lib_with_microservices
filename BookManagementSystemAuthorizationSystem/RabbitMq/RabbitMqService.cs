using System.Text;
using System.Text.Json;
using BookManagementSystemAuthorizationSystem.RabbitMq.Contracts;
using RabbitMQ.Client;

namespace BookManagementSystemAuthorizationSystem.RabbitMq;

public class RabbitMqService : IRabbitMqService
{
    private readonly IConfiguration _configuration;
    private readonly string _hostName;
    private readonly string _queueName;

    public RabbitMqService(IConfiguration configuration)
    {
        _configuration = configuration;
        _hostName = configuration["RabbitMqSettings:HostName"]!;
        _queueName = configuration["RabbitMqSettings:QueueName"]!;
    }

    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body);
        }
    }
}
