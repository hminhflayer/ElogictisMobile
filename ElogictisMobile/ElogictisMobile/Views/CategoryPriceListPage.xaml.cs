using ElogictisMobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPriceListPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryPriceListPage" /> class.
        /// </summary>
        public CategoryPriceListPage()
        {
            this.InitializeComponent();
        }
        private void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (this.BindingContext is CategoryPriceListPageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }

        private void SearchBar_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (this.BindingContext is CategoryPriceListPageViewModel vm)
            {
                vm.TextChangedCommand.Execute(text);
            }
        }
    }
}