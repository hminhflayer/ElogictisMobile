using ElogictisMobile.DataService;
using ElogictisMobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to display the Contacts list.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageProfilesPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManageProfilesPage" /> class.
        /// </summary>
        public ManageProfilesPage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if(this.BindingContext is ManageProfilesPageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }

        private void SearchBar_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (this.BindingContext is ManageProfilesPageViewModel vm)
            {
                vm.TextChangedCommand.Execute(text);
            }
        }
    }
}