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
        public string Avatar { get; set; }
        public double Money { get; set; }
        public string AgencyId { get; set; }
        public string ManageAgency { get; set; }
        public bool IsConfirm { get; set; }

        /*
         *AUTH:
         *1:USERS
         *2:STAFF
         *3:MANAGE
         *4:ADMIN
         */
        public string Auth { get; set; }
        public string Auth_ext { get; set; }
        public string Province { get; set; }
        public string Province_ext { get; set; }
        public string District { get; set; }
        public string District_ext { get; set; }
        public string Town { get; set; }
        public string Town_ext { get; set; }
        public int HolderProductPrioritize { get; set; }
        public int CountHolderProduct { get; set; }
    }
}
