using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for catalog page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class CategoryPriceListPageViewModel : BaseViewModel
    {
        #region Fields

        private Command<object> itemTappedCommand;
        private Command<object> backCommand;
        private Command<object> addPriceListCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProductsPageViewModel"/> class.
        /// </summary>
        public CategoryPriceListPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            PricesList = RealtimeFirebase.Instance.GetAllCategory<PriceList>("PricesList");
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

        public Command<object> AddPriceListCommand
        {
            get
            {
                return this.addPriceListCommand ?? (this.addPriceListCommand = new Command<object>(this.AddPriceListClicked));
            }
        }

        private INavigationService _navigationService;

        /// <summary>
        /// Gets or sets a collction of value to be displayed in contacts list page.
        /// </summary>\
        public ObservableCollection<PriceList> PricesList { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected from the contacts list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            // Do something
            LocalContext.PriceListSelected = selectedItem as PriceList;
            await _navigationService.NavigateToAsync<DetailCategoryPricesListPageViewModel>();
        }

        private async void GoToBack(object obj)
        {
            // Do something
            await _navigationService.GoBackAsync();
        }

        private async void AddPriceListClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<AddCategoryPriceListPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }
        #endregion
    }
}