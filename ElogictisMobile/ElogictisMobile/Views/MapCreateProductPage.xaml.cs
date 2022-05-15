using ElogictisMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapCreateProductPage : ContentPage
    {
        public MapCreateProductPage()
        {
            InitializeComponent();
        }

        private void comboBox_TypeShipProduct_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is MapCreateProductPageViewModel vm)
            {
                vm.TypeShipeChangeCommand.Execute(null);
            }
        }
    }
}