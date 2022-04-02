﻿using ElogictisMobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
    /// <summary>
    /// Page to show the empty cart
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageProvincePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManageProvincePage" /> class.
        /// </summary>
        public ManageProvincePage()
        {
            this.InitializeComponent();
        }

        private void SearchBar_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (this.BindingContext is ManageProvincePageViewModel vm)
            {
                vm.TextChangedCommand.Execute(text);
            }
        }

        private void ListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (this.BindingContext is ManageProvincePageViewModel vm)
            {
                vm.ItemTappedCommand.Execute(e.Item);
            }
        }
    }
}