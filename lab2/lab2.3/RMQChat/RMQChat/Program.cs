using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace RMQChat
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File("log.txt")
               .CreateLogger();
            string username;

            Console.WriteLine("Imput username:");
            username = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Your username empty, try again:");
                username = Console.ReadLine();
            }

            Receiver receiver = Receiver.Instance;
            Producer producer = new Producer();
            log.Information($"User {username} connected to chat.");
            Console.WriteLine("Write message, (Q) for exit:");

            Message message = new Message();
            message.Username = username;
            while (message.Text!="Q")
            {
                message.Text = null;
                while (string.IsNullOrWhiteSpace(message.Text) || string.IsNullOrEmpty(message.Text))
                {
                    message.Text = Console.ReadLine();
                    log.Error($"User {message.Username} send incorrect message");
                }
                message.Date = DateTime.Now;
                producer.Produce(message);
                log.Information($"User {message.Username} send message \"{message.Text}\" ms.");
            }

            receiver.Dispose();
            log.Information($"User {username} has leaved chat.");
        }
    }
}
