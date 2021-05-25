using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "*";
            factory.UserName = "*";
            factory.Password = "*";
            _ = Task.Run(async () =>
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("test_q", true, false, false, null);

                        channel.ExchangeDeclare("testDirect", "direct", true);

                        channel.QueueBind(queue: "test_q",
                              exchange: "testDirect",
                              routingKey: "test_q");

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                       {
                           var message = ea.Body.ToArray();
                       };
                        channel.BasicConsume("test_q", true, consumer);

                        while (true)
                        {
                            await Task.Delay(10);
                        }
                    }
                }
            });

            Console.ReadKey();
        }
    }
}
