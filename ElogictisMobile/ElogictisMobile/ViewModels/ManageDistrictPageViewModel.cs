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
    public class ManageDistrictPageViewModel : BaseViewModel
    {
        #region Fields

        private Command<object> itemTappedCommand;
        private Command<object> backCommand;
        private Command<object> addProductCommand;
        private Command<string> textChangedCommand;

        private INavigationService _navigationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProductsPageViewModel"/> class.
        /// </summary>
        public ManageDistrictPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            DistrictCollection = new ObservableCollection<District>();
            this.ProvinceCommand = new Command(this.ProvinceChangeClicked);
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
        public Category Province { get; set; }
        public ObservableCollection<Category> ProvinceCollection { get; set; } = LocalContext.ProvinceList;

        /// <summary>
        /// Gets or sets a collction of value to be displayed in contacts list page.
        /// </summary>
        [DataMember(Name = "contactsPageList")]
        public ObservableCollection<District> DistrictCollection { get; set; }

        #endregion
        #region Commands
        public Command ProvinceCommand { get; set; }
        #endregion
        #region Methods

        /// <summary>
        /// Invoked when an item is selected from the contacts list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            // Do something
            LocalContext.DistrictSelected = selectedItem as District;
            LocalContext.IsEdit = true;
            await _navigationService.NavigateToAsync<AddDistrictPageViewModel>();
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
                LocalContext.ProvinceSelected = Province;
                LocalContext.IsEdit = false;
                await _navigationService.NavigateToAsync<AddDistrictPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        public async void ProvinceChangeClicked()
        {
            try
            {
                List<District> districts = new List<District>();
                IsLoading = true;
                DistrictCollection.Clear();
                var province = Province.Id;
                districts = await RealtimeFirebase.Instance.GetListDistrict(province);
                LocalContext.Districts = districts;
                foreach(var x in districts)
                {
                    DistrictCollection.Add(x);
                }    
                IsLoading = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                IsLoading = false;
            }

        }

        private void SearchTextChanged(string search)
        {
            // Do something
            try
            {
                List<District> tmp = LocalContext.Districts;
                if (search == null || search == "")
                {
                    foreach (var item in tmp)
                    {
                        DistrictCollection.Add(item);
                    }
                    return;
                }

                this.DistrictCollection.Clear();
                foreach (var item in tmp)
                {
                    if (item.Name.ToLower().Contains(search.ToLower()))
                    {
                        DistrictCollection.Add(item);
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
