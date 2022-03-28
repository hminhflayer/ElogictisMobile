﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.Models
{
    public class Products
    {
        public string ID { get; set; }
        public string From_FullName { get; set; }
        public string From_PhoneNumber { get; set; }
        public string From_Address { get; set; }
        public string To_FullName { get; set; }
        public string To_PhoneNumber { get; set; }
        public string To_Address { get; set; }
        public string Weight { get; set; }
        public string Quanlity { get; set; }
        public string Money { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string CreateTime { get; set; }
        public string CreateBy { get; set; }
        public string LastUpdateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public bool IsDelete { get; set; }
        /*
         *STATUS:
         *1:Wait for confirmation
         *2:Delivering
         *3:Delivered Success
         *4:Cancel
         */
        public int Status { get; set; }
        public string Holder { get; set; }
    }
}
