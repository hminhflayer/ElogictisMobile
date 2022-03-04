using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to sign in with user details.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpPage" /> class.
        /// </summary>
        public SignUpPage()
        {
            this.InitializeComponent();
        }
    }
}