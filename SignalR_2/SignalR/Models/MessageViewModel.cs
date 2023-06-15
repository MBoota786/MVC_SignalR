using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Models
{
    public class MessageViewModel
    {
        [Required]
        public string Receiver { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
