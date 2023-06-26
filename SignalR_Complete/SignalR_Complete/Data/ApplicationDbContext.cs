using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR_Complete.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignalR_Complete.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupUser> GroupUser { get; set; }
        public DbSet<PrivateMessage> PrivateChat { get; set; }
        public DbSet<GroupMessage> GroupMessage { get; set; }
        public DbSet<PrivateMessage> PrivateMessage { get; set; }
        public DbSet<Image> Image { get; set; }

    }
}
