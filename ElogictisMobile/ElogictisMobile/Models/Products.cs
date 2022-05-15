using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class Products
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string From_FullName { get; set; }
        public string From_PhoneNumber { get; set; }
        public string From_Address { get; set; }
        public string To_FullName { get; set; }
        public string To_PhoneNumber { get; set; }
        public string To_Address { get; set; }
        public double Weight { get; set; }
        public int Quanlity { get; set; }
        public double Money { get; set; }
        public string Type { get; set; }
        public string Type_ext { get; set; }
        public string Description { get; set; }
        public string CreateTime { get; set; }
        public string CreateBy { get; set; }
        public string LastUpdateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public bool IsDelete { get; set; }
        /*
         *STATUS:
         *1:CHỜ NHẬN HÀNG
         *2:CHỜ LẤY HÀNG
         *3:ĐANG GIAO
         *4:GIAO THÀNH CÔNG
         *5:GIAO KHÔNG THÀNH CÔNG
         */
        public int Status { get; set; }
        public string Status_ext { get; set; }
        public bool IsConfirm { get; set; }
        public string Holder { get; set; }
        public string AgencyId { get; set; }
        public string TypeShip { get; set; }
        public string TypeShip_ext { get; set; }
        public double LngFromAddress { get; set; }
        public double LatFromAddress { get; set; }
        public double LngToAddress { get; set; }
        public double LatToAddress { get; set; }
        public double DistanceEstimate { get; set; }
        public string OrderExpirationDate { get; set; }
    }
}
