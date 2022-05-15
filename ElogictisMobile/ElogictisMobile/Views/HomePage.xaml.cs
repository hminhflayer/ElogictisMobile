using ElogictisMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }
        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Monkey selectedItem = e.CurrentSelection[0] as Monkey;
        }
    }

}