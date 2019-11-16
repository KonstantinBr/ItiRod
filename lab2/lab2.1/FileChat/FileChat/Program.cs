using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using System.Text;
using Serilog;
using System.Threading;

namespace FileChat
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .WriteTo.File("log.txt")
              .CreateLogger();
            Console.WriteLine("Imput username:");
            string username = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Your username empty, try again:");
                username = Console.ReadLine();
            }

            log.Information($"User {username} connected to chat.");
            Console.WriteLine("Write message, (Q) for exit:");
            Message message = new Message();
            Task.Run(() =>
            {
                int countIfMsg = 0;
                while (true)
                {
                    MessageFileTransformer.GetFileFromServer();
                    var fullListOfMsg = MessageFileTransformer.CreateMsgsFromLocalJson();
                    var exListOfMsg = fullListOfMsg.Skip(countIfMsg);
                    countIfMsg = fullListOfMsg.Count;
                    foreach (var msg in exListOfMsg)
                    {
                        Console.WriteLine("{0} ({1}) {2}", msg.Username, msg.Date, msg.Text);
                    }
                    Thread.Sleep(1000);
                }
            });

            while (message.Text != "Q")
            {
                message = new Message();
                message.Username = username;
                message.Text = null;
                while (string.IsNullOrWhiteSpace(message.Text) || string.IsNullOrEmpty(message.Text))
                {
                    message.Text = Console.ReadLine();
                    log.Error($"User {message.Username} send incorrect message");
                }
                message.Date = DateTime.Now;
                var fullListOfMsg = MessageFileTransformer.CreateMsgsFromLocalJson();
                fullListOfMsg.Add(message);
                MessageFileTransformer.CreateLocalJsonFromMsgs(fullListOfMsg);
                MessageFileTransformer.SendFileToServer();
                log.Information($"User {message.Username} send message \"{message.Text}\" ms.");
            }
        }
    }
}
