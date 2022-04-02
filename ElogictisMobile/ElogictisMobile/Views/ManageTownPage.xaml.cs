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
    public partial class ManageTownPage : ContentPage
    {
        public ManageTownPage()
        {
            InitializeComponent();
        }

        private void District_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is ManageTownPageViewModel vm)
            {
                vm.DistrictCommand.Execute(e.Value);
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (this.BindingContext is ManageTownPageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }

        private void Province_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is ManageTownPageViewModel vm)
            {
                vm.ProvinceCommand.Execute(e.Value);
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (this.BindingContext is ManageTownPageViewModel vm)
            {
                vm.TextChangedCommand.Execute(text);
            }
        }
    }
}