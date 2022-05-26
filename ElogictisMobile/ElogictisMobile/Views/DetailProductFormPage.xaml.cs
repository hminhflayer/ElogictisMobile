using ElogictisMobile.ViewModels;
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
    public partial class DetailProductFormPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailProductFormPage" /> class.
        /// </summary>
        public DetailProductFormPage()
        {
            this.InitializeComponent();
        }
        private void WeightEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue) || string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                return;
            }
            if (this.BindingContext is DetailProductFormPageViewModel vm)
            {
                vm.WeightChangedCommand.Execute(double.Parse(e.NewTextValue));
            }
        }
    }
}