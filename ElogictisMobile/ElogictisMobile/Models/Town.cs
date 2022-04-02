using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class Town
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string FullAddress { get; set; }
    }
}
