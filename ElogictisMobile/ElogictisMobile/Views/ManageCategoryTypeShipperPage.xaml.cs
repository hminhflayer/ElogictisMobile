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
    public partial class ManageCategoryTypeShipperPage : ContentPage
    {
        public ManageCategoryTypeShipperPage()
        {
            InitializeComponent();
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (this.BindingContext is ManageCategoryTypeShipperPageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }

        private void SearchBar_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (this.BindingContext is ManageCategoryTypeShipperPageViewModel vm)
            {
                vm.TextChangedCommand.Execute(text);
            }
        }
    }
}