using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer;
public class Receiver
{
    public bool Receive()
    {
        try
        {
            const string queue = "BasicTest";

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "myuser",
                Password = "mypassword"
            };


            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue, false, false, false, null);

            var consumer = new  EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                byte[] body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received message: {message}");
            };

            channel.BasicConsume(queue, true, consumer);

            Console.WriteLine("Exit...");
            Console.ReadKey();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}
