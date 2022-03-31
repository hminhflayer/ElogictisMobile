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
    public partial class ManageProductNewPage : ContentPage
    {
        public ManageProductNewPage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (this.BindingContext is ManageProductNewPageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }
    }
}