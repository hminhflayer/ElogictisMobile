using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

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
    }
}