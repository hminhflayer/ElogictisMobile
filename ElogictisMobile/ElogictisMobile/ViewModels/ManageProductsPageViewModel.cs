using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for contacts page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ManageProductsPageViewModel : BaseViewModel
    {
        #region Fields

        private Command<object> itemTappedCommand;
        private Command<object> backCommand;
        private Command<object> addProductCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProductsPageViewModel"/> class.
        /// </summary>
        public ManageProductsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ContactList = RealtimeFirebase.Instance.GetAll<Products>("Products");
            LocalContext.ProductsList = ContactList;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command<object> ItemTappedCommand
        {
            get
            {
                return this.itemTappedCommand ?? (this.itemTappedCommand = new Command<object>(this.NavigateToNextPage));
            }
        }

        public Command<object> BackCommand
        {
            get
            {
                return this.backCommand ?? (this.backCommand = new Command<object>(this.GoToBack));
            }
        }

        public Command<object> AddProductCommand
        {
            get
            {
                return this.addProductCommand ?? (this.addProductCommand = new Command<object>(this.AddProductClicked));
            }
        }

        private INavigationService _navigationService;

        /// <summary>
        /// Gets or sets a collction of value to be displayed in contacts list page.
        /// </summary>
        [DataMember(Name = "contactsPageList")]
        public ObservableCollection<Products> ContactList { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected from the contacts list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            // Do something
            LocalContext.ProductSelected = selectedItem as Products;
            await _navigationService.NavigateToAsync<DetailProductFormPageViewModel>();
        }

        private async void GoToBack(object obj)
        {
            // Do something
            await _navigationService.GoBackAsync();
        }

        private async void AddProductClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<AddProductFormPageViewModel>(null,true);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }
        #endregion
    }
}