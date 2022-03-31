using ElogictisMobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to display on-boarding gradient with animation
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageAgencyPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManageAgencyPage" /> class.
        /// </summary>
        public ManageAgencyPage()
        {
            this.InitializeComponent();
        }
        private void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (this.BindingContext is ManageAgencyPageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }
    }
}