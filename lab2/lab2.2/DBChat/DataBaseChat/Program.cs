using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataBaseChat.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DataBaseChat
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatContext db = new ChatContext();
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
            ChatTable message = new ChatTable();

            Task.Run(() =>
            {
                int countIfMsg = 0;
                while (true)
                {
                    ChatContext dbUpdate = new ChatContext();
                    var fullListOfMsg = dbUpdate.ChatTables.ToList();
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
                message = new ChatTable();
                message.Username = username;
                message.Text = null;
                while (string.IsNullOrWhiteSpace(message.Text) || string.IsNullOrEmpty(message.Text))
                {
                    message.Text = Console.ReadLine();
                    log.Error($"User {message.Username} send incorrect message");
                }
                message.Date = DateTime.Now;
                db.ChatTables.Add(message);
                db.SaveChanges();
                log.Information($"User {message.Username} send message \"{message.Text}\" ms.");
            }
        }
    }
}
