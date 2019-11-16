using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataBaseChat.Models
{
    public class ChatTable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
