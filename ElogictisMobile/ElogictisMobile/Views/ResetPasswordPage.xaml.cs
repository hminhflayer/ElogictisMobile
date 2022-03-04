using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to reset old password
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordPage" /> class.
        /// </summary>
        public ResetPasswordPage()
        {
            this.InitializeComponent();
        }
    }
}