using ElogictisMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to show the transaction history.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionHistoryPage : ContentPage
    {
        public TransactionHistoryPage()
        {
            this.InitializeComponent();
        }
    }
}