using ElogictisMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// This page used for adding Profile image with Name and phone number.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateProfilePage : ContentPage
    {
        public UpdateProfilePage()
        {
            this.InitializeComponent();
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is UpdateProfilePageViewModel vm)
            {
                vm.ProvinceCommand.Execute(e.Value);
            }
        }

        private void District_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is UpdateProfilePageViewModel vm)
            {
                vm.DistrictCommand.Execute(e.Value);
            }
        }

        private void Town_SelectedChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is UpdateProfilePageViewModel vm)
            {
                vm.TownCommand.Execute(e.Value);
            }
        }
    }
}