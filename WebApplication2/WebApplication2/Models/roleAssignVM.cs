using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class roleAssign_VM
    {
        public user user { get; set; }
        public role role { get; set; }
        public usersRole userRole { get; set; }
        public List<user> userList { get; set; }
        public List<role> roleList { get; set; }
        public List<usersRole> userRoleList { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public List<int> checkObject { get; set; }
    }
    
    public class checkbox
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
    }
}