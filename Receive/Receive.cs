using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

class Receive
{
    public static void Main()
    {
        var endpoints = new List<AmqpTcpEndpoint>
        {
            new AmqpTcpEndpoint("localhost", 5674),
            new AmqpTcpEndpoint("localhost", 5675),
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                
                var message = Encoding.UTF8.GetString(body, 0 , Array.IndexOf(body, Convert.ToByte(46)));
                Console.WriteLine(" [x] Received \"{0}\"", message);
            };
            channel.BasicConsume(queue: "hello",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}