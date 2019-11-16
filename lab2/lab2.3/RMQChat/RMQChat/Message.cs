using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQChat
{
    public class Message
    {
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
