using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Models
{
    public class GroupUser
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        // Add other properties as needed

        public Group Group { get; set; }
        public AppUser User { get; set; }
    }
}
