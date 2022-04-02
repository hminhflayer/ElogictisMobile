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
    public partial class AddTownPage : ContentPage
    {
        public AddTownPage()
        {
            InitializeComponent();
        }

        private void Province_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is AddTownPageViewModel vm)
            {
                vm.ProvinceCommand.Execute(e.Value);
            }
        }
    }
}