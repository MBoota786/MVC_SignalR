using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Models
{
    public class IndexViewModel
    {
        public List<AppUser> Users { get; set; }
        public List<Group> Groups { get; set; }
    }
}
