using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for empty cart page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ManageProvincePageViewModel : BaseViewModel
    {
        #region Fields

        private Command<object> itemTappedCommand;
        private Command<object> backCommand;
        private Command<object> addProductCommand;
        private Command<string> textChangedCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProductsPageViewModel"/> class.
        /// </summary>
        public ManageProvincePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.ProvinceList = RealtimeFirebase.Instance.GetListProvince();
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
        /// </summary>
        [DataMember(Name = "contactsPageList")]
        public ObservableCollection<Category> ProvinceList { get; set; }
        public List<Category> List { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected from the contacts list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            // Do something
            LocalContext.ProvinceSelected = selectedItem as Category;
            LocalContext.IsEdit = true;
            await _navigationService.NavigateToAsync<AddProvincePageViewModel>();
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
                LocalContext.IsEdit = false;
                await _navigationService.NavigateToAsync<AddProductFormPageViewModel>();
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
                ObservableCollection<Category> tmp = LocalContext.ProvinceList;
                if (search == null || search == "")
                {
                    foreach (var item in tmp)
                    {
                        ProvinceList.Add(item);
                    }
                    return;
                }    
                   
                this.ProvinceList.Clear();
                foreach(var item in tmp)
                {
                    if(item.Name.ToLower().Contains(search.ToLower()))
                    {
                        ProvinceList.Add(item);
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
