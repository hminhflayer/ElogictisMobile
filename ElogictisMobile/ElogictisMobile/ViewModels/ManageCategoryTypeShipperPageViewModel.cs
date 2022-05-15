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
    public class ManageCategoryTypeShipperPageViewModel : BaseViewModel
    {
        #region Fields

        private Command<object> itemTappedCommand;
        private Command<object> backCommand;
        private Command<object> addTypeProductCommand;
        private Command<string> textChangedCommand;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProductsPageViewModel"/> class.
        /// </summary>
        public ManageCategoryTypeShipperPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            TypeProductList = new ObservableCollection<Category>();
            TypeProductList.Clear();
            TypeProductList = RealtimeFirebase.Instance.GetAllCategory<Category>("TypeShip");
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

        public Command<object> AddTypeProductCommand
        {
            get
            {
                return this.addTypeProductCommand ?? (this.addTypeProductCommand = new Command<object>(this.AddTypeShipClicked));
            }
        }
        public Command<string> TextChangedCommand
        {
            get
            {
                return this.textChangedCommand ?? (this.textChangedCommand = new Command<string>(this.SearchTextChanged));
            }
        }

        private INavigationService _navigationService;

        /// <summary>
        /// Gets or sets a collction of value to be displayed in contacts list page.
        /// </summary>\
        public ObservableCollection<Category> TypeProductList { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected from the contacts list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            // Do something
            LocalContext.TypeShipProductSelected = new TypeShipProduct();
            await _navigationService.NavigateToAsync<AddCategoryTypeShipperPageViewModel>();

        }

        private async void GoToBack(object obj)
        {
            // Do something
            await _navigationService.GoBackAsync();
        }

        private async void AddTypeShipClicked(object obj)
        {
            // Do something
            try
            {
                LocalContext.TypeShipProductSelected = obj as TypeShipProduct;
                await _navigationService.NavigateToAsync<AddCategoryTypeShipperPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }
        private void SearchTextChanged(string search)
        {
            // Do something
            try
            {
                ObservableCollection<Category> tmp = LocalContext.TypeProductList;
                if (search == null || search == "")
                {
                    foreach (var item in tmp)
                    {
                        TypeProductList.Add(item);
                    }
                    return;
                }

                this.TypeProductList.Clear();
                foreach (var item in tmp)
                {
                    if (item.Name.ToLower().Contains(search.ToLower()))
                    {
                        TypeProductList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }
        #endregion
    }
}