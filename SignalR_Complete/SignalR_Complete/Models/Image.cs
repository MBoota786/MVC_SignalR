using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public byte[] ImageBytes { get; set; }
        public DateTime Timestamp { get; set; }
        // Add other properties as needed

        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}
