using ElogictisMobile.DataService;
using ElogictisMobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to show the catalog list. 
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryTypeProductPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTypeProductPage" /> class.
        /// </summary>
        public CategoryTypeProductPage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (this.BindingContext is CategoryTypeProductPageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }
    }
}