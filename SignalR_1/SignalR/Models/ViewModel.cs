using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Models
{
    public class ViewModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public Message Message { get; set; }
        public string userName { get; set; }
        public string SenderId { get; set; }
        public string RecevireId { get; set; }

    }
}
