using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FileChat
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
}
