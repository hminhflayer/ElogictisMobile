using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class Agency
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string Province_ext { get; set; }
        public string District { get; set; }
        public string District_ext { get; set; }
        public string Town { get; set; }
        public string Town_ext { get; set; }
        public string Address { get; set; }
        public string CreateBy { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
        public string UpdateBy { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
    }
}
