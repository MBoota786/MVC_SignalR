using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalR_GPT.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentTime { get; set; }
        public bool IsRead { get; set; }
        public bool IsSender { get; set; }
        public bool IsReceiver { get; set; }
    }
}
