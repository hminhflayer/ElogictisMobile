using ElogictisMobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to sign in with user details.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProfilesPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddProfilesPage" /> class.
        /// </summary>
        public AddProfilesPage()
        {
            this.InitializeComponent();
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is AddProfilesPageViewModel vm)
            {
                vm.ProvinceCommand.Execute(e.Value);
            }
        }

        private void District_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is AddProfilesPageViewModel vm)
            {
                vm.DistrictCommand.Execute(e.Value);
            }
        }

        private void Town_SelectedChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is AddProfilesPageViewModel vm)
            {
                vm.TownCommand.Execute(e.Value);
            }
        }
    }
}