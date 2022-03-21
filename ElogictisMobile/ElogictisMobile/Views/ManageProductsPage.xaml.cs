﻿using ElogictisMobile.DataService;
using ElogictisMobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to display the Contacts list.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageProductsPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManageProductsPage" /> class.
        /// </summary>
        public ManageProductsPage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if(this.BindingContext is ManageProductsPageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }
    }
}