using RabbitMQ.Client;
using System.Text;

namespace Producer;
public class Sender
{
    public bool Send()
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

            var message = "Test.";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", queue, null, body);

            Console.WriteLine("Sent message: " + message);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}
