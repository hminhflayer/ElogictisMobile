using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class DetailRevenue
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductMoney { get; set; }
        public double Profit { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string ProfilesId { get; set; }
    }
}
