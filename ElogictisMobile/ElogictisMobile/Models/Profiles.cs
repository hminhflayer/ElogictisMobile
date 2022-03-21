using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class Profiles
    {
        public string Profile_Id { get; set; }
        public string Profile_Name { get; set; }
        public string Profile_Email { get; set; }
        public string Profile_Phone { get; set; }
        public string Profile_Address { get; set; }
        public string Profile_CreateTime { get; set; }
        public string Profile_CreateBy { get; set; }
        public string Profile_LastUpdateTime { get; set; }
        public string Profile_LastUpdateBy { get; set; }
        public string Profile_Identity { get; set; }
        public bool Profile_IsDelete { get; set; }
        /*
         *AUTH:
         *1:USERS
         *2:STAFF
         *3:MANAGE
         *4:ADMIN
         */
        public int Profile_Auth { get; set; }
        public string Profile_Avatar { get; set; }
        public string Token { get; set; }
        public DateTime ExpireToken { get; set; }
    }
}
