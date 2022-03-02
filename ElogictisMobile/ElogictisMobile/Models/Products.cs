using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class Products
    {
        public string Product_ID { get; set; }
        public string Product_From_FullName { get; set; }
        public string Product_From_PhoneNumber { get; set; }
        public string Product_From_Address { get; set; }
        public string Product_To_FullName { get; set; }
        public string Product_To_PhoneNumber { get; set; }
        public string Product_To_Address { get; set; }
        public string Product_Weight { get; set; }
        public string Product_Quanlity { get; set; }
        public string Product_Money { get; set; }
        public string Product_Description { get; set; }
        public string Product_CreateTime { get; set; }
        public string Product_CreateBy { get; set; }
        public string Product_LastUpdateTime { get; set; }
        public string Product_LastUpdateBy { get; set; }
        public bool Product_IsDelete { get; set; }
    }
}
