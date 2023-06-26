using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Complete.Models
{
    public class AppUser:IdentityUser
    {
        public string ConnectionId { get; set; }
        public string Role { get; set; }
    }
}
