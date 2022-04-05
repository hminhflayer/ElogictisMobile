using ElogictisMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to add business details such as name, email address, and phone number.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductFormPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPage" /> class.
        /// </summary>
        public AddProductFormPage()
        {
            this.InitializeComponent();
        }

        private void WeightEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(e.NewTextValue) || string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                return;
            }    
            if (this.BindingContext is AddProductFormPageViewModel vm)
            {
                vm.WeightChangedCommand.Execute(double.Parse(e.NewTextValue));
            }
        }

        private void FromMap_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            object position = new Position(e.Position.Latitude, e.Position.Longitude);
            if (this.BindingContext is AddProductFormPageViewModel vm)
            {
                vm.FromMapClickedCommand.Execute(position);
            }
        }

        private void ToMap_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            object position = new Position(e.Position.Latitude, e.Position.Longitude);
            if (this.BindingContext is AddProductFormPageViewModel vm)
            {
                vm.ToMapClickedCommand.Execute(position);
            }
        }

        private void FromAddressEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.BindingContext is AddProductFormPageViewModel vm)
            {
                vm.FromAddressChangedCommand.Execute(e.NewTextValue);
            }
        }

        private void ToAddressEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.BindingContext is AddProductFormPageViewModel vm)
            {
                vm.ToAddressChangedCommand.Execute(e.NewTextValue);
            }
        }
    }
}