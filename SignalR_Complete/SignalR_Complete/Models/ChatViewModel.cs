using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Models
{
    public class ChatViewModel
    {
        public AppUser User { get; set; }
        public AppUser Participant { get; set; }
        public List<PrivateMessage> Messages { get; set; }
    }
}
