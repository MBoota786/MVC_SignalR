using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Models
{
    
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupUser> GroupUsers { get; set; }
        public List<GroupMessage> GroupMessages { get; set; }
        // Add other properties as needed
    }
}
