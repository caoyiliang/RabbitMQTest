using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace RabbitMQ
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "*";
            factory.UserName = "*";
            factory.Password = "*";
            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("test_q", true, false, false, null);
            channel.ExchangeDeclare("testDirect", "direct", true);
            channel.QueueBind(queue: "test_q",
                      exchange: "testDirect",
                      routingKey: "test_q");

            int i = 0;
            while (true)
            {
                channel.BasicPublish("testDirect", "test_q", null, new byte[] { (byte)i++ });
                await Task.Delay(10);
            }
        }
    }
}
