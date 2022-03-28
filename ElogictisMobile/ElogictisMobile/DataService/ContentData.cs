using ElogictisMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ElogictisMobile.DataService
{
    public static class ContentData
    {
        public static ObservableCollection<Category> TypeProductCollection = new ObservableCollection<Category>()
        {
            new Category() { Id = "1", Name = "Hàng hóa bình thường" },
            new Category() { Id = "2", Name = "Hàng hóa dễ vỡ" },
            new Category() { Id = "3", Name = "Hàng hóa dễ cháy" }
        };

        public static ObservableCollection<Category> WeightCollection = new ObservableCollection<Category>()
        {
            new Category() { Id = "1", Name = "Dưới 1 Kg" },
            new Category() { Id = "2", Name = "2 Kg" },
            new Category() { Id = "3", Name = "3 Kg" },
            new Category() { Id = "4", Name = "4 Kg" },
            new Category() { Id = "5", Name = "5 Kg" },
            new Category() { Id = "6", Name = "6 Kg" },
            new Category() { Id = "7", Name = "7 Kg" },
            new Category() { Id = "8", Name = "8 Kg" },
            new Category() { Id = "9", Name = "9 Kg" },
            new Category() { Id = "10", Name = "10 Kg" },
            new Category() { Id = "11", Name = "11 Kg" },
            new Category() { Id = "12", Name = "12 Kg" },
            new Category() { Id = "13", Name = "13 Kg" },
            new Category() { Id = "14", Name = "14 Kg" },
            new Category() { Id = "15", Name = "15 Kg" },
            new Category() { Id = "16", Name = "16 Kg" },
            new Category() { Id = "17", Name = "17 Kg" },
            new Category() { Id = "18", Name = "18 Kg" },
            new Category() { Id = "19", Name = "19 Kg" },
            new Category() { Id = "20", Name = "20 Kg" },
        };

        public static ObservableCollection<Category> PermissionCollection = new ObservableCollection<Category>()
        {
            new Category() { Id ="1",Name="Người dùng bình thường"},
            new Category() { Id ="2",Name="Nhân viên"},
            new Category() { Id ="3",Name="Quản lý"},
            new Category() { Id ="4",Name="Quản trị viên"}
        };
    }
}
