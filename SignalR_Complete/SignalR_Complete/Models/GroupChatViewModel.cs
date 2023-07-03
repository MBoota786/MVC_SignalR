using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Models
{
    public class GroupChatViewModel
    {
        public List<AppUser> User { get; set; }
        public Group Group { get; set; }
        public List<GroupMessage> Messages { get; set; }
    }
}
