using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using RabbitMQ.Client;
using System.Text;

class Send
{
    public static void Main()
    {
        var endpoints = new List<AmqpTcpEndpoint>
        {
            //new AmqpTcpEndpoint("localhost", 5676),
            new AmqpTcpEndpoint("localhost", 5677),
        };

        var factory = new ConnectionFactory();
        using (var connection = factory.CreateConnection(endpoints))
        using (var channel = connection.CreateModel())
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("x-queue-type", "quorum");

            channel.QueueDeclare(queue: "hello",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: dict);

            Console.WriteLine(" Press [q] to exit.");

            for (int i = 0;; i++)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q) break;

                string message = i + " message.";
                var textBody = Encoding.UTF8.GetBytes(message);

                var randomBody = new byte[100 * 1024 * 1024];
                var rnd = new Random();
                rnd.NextBytes(randomBody);

                var combine = textBody.Concat(randomBody).ToArray();

                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: combine);
                Console.WriteLine(" [x] Sent \"{0}\"", message);

                System.Threading.Thread.Sleep(500);
            }
        }
    }
}