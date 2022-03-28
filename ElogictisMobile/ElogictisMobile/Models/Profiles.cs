using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class Profiles
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CreateTime { get; set; }
        public string CreateBy { get; set; }
        public string LastUpdateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public string Identity { get; set; }
        public bool IsDelete { get; set; }
        /*
         *AUTH:
         *1:USERS
         *2:STAFF
         *3:MANAGE
         *4:ADMIN
         */
        public int Auth { get; set; }
        public string Auth_ext { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }
        public DateTime ExpireToken { get; set; }
        public int Money { get; set; }
    }
}
