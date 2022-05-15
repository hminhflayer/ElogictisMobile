using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public  class PriceList
    {
        public string Id { get; set; }
        public string From_Kilometer { get; set; }
        public string To_Kilometer { get; set; }
        public string From_Weight { get; set; }
        public string To_Weight { get; set; }
        public string TypeProduct { get; set; }
        public string TypeProduct_ext { get; set; }
        public string Price { get; set; }
        public bool IsDelete { get; set; }
        public string TypeShipProduct_ext { get; set; }
        public string TypeShipProduct { get; set; }
    }
}
