using ElogictisMobile.DataService;
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
            this.BindingContext = ContactsDataService.Instance.ManageProfilesPageViewModel;
        }
    }
}