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
    public partial class MapExtendPage : ContentPage
    {
        public MapExtendPage()
        {
            InitializeComponent();
        }


        private void Map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            object position = new Position(e.Position.Latitude, e.Position.Longitude);
            if (this.BindingContext is MapExtendPageViewModel vm)
            {
                vm.MapClickedCommand.Execute(position);
            }
        }
    }
}