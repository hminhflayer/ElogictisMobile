using ElogictisMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ElogictisMobile.DataService
{
    public static class ContentData
    {
        public static ObservableCollection<TypeProduct> TypeProductCollection = new ObservableCollection<TypeProduct>()
        {
            new TypeProduct() { Id = "1", Name = "Hàng hóa bình thường" },
            new TypeProduct() { Id = "2", Name = "Hàng hóa dễ vỡ" },
            new TypeProduct() { Id = "3", Name = "Hàng hóa dễ cháy" }
        };
    }
}
