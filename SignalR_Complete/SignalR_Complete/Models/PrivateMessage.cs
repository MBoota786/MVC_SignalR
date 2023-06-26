using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Models
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsReaded { get; set; }
        // Add other properties as needed

        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}
