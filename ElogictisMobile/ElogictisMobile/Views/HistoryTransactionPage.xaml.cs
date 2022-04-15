using ElogictisMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryTransactionPage : ContentPage
    {
        public HistoryTransactionPage()
        {
            InitializeComponent();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            //if (this.BindingContext is ManageProductNewPageViewModel vm)
            //{
            //    vm.ItemTappedCommand.Execute(e.Item);
            //}
        }
    }
}