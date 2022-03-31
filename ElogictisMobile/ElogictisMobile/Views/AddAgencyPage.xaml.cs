
using ElogictisMobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// About us simple page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAgencyPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ElogictisMobile.Views.AddAgencyPage"/> class.
        /// </summary>
        public AddAgencyPage()
        {
            this.InitializeComponent();
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is AddAgencyPageViewModel vm)
            {
                vm.ProvinceCommand.Execute(sender);
            }
        }

        private void District_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (this.BindingContext is AddAgencyPageViewModel vm)
            {
                vm.DistrictCommand.Execute(sender);
            }
        }
    }
}