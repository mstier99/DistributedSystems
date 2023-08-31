using RabbitMQ.Client.Logging;

namespace Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Receive...");

            new Receiver().Receive();

            Console.ReadKey();
        }
    }
}
