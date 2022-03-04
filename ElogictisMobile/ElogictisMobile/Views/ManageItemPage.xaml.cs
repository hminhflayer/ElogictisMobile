using ElogictisMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to show the no item
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageItemPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManageItemPage" /> class.
        /// </summary>
        public ManageItemPage()
        {
            this.InitializeComponent();
        }
    }
}