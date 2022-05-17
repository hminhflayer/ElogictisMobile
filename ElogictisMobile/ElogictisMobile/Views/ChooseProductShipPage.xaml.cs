using ElogictisMobile.Models;
using ElogictisMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseProductShipPage : ContentPage
    {
        public ChooseProductShipPage()
        {
            InitializeComponent();
        }
        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            var checkbox = (Plugin.InputKit.Shared.Controls.CheckBox)sender;
            var ob = checkbox.BindingContext as Products;
            if (this.BindingContext is ChooseProductShipPageViewModel vm)
            {
                if (ob != null)
                {
                    if (checkbox.IsChecked)
                    {
                        vm.selectedItems.Add(ob);
                    }
                    else
                    {
                        vm.selectedItems.Remove(ob);
                    }
                }
            }
            
        }
    }
}