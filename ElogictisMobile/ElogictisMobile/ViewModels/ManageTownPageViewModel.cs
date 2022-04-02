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
    public class ManageTownPageViewModel : BaseViewModel
    {
        #region Fields

        private Command<object> itemTappedCommand;
        private Command<object> backCommand;
        private Command<object> addTownCommand;
        private Command<string> textChangedCommand;
        public List<District> districts = LocalContext.Districts;
        private INavigationService _navigationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProductsPageViewModel"/> class.
        /// </summary>
        public ManageTownPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            DistrictCollection = new ObservableCollection<District>();
            TownCollection = new ObservableCollection<Town>();
            this.ProvinceCommand = new Command(this.ProvinceChangeClicked);
            this.DistrictCommand = new Command(this.DistrictChangeClicked);
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

        public Command<object> AddTownCommand
        {
            get
            {
                return this.addTownCommand ?? (this.addTownCommand = new Command<object>(this.AddTownClicked));
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
        public District District { get; set; }
        public ObservableCollection<Category> ProvinceCollection { get; set; } = LocalContext.ProvinceList;
        public ObservableCollection<District> DistrictCollection { get; set; }
        public ObservableCollection<Town> TownCollection { get; set; }

        #endregion
        #region Commands
        public Command ProvinceCommand { get; set; }
        public Command DistrictCommand { get; set; }
        #endregion
        #region Methods

        /// <summary>
        /// Invoked when an item is selected from the contacts list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            // Do something
            LocalContext.TownSelected = selectedItem as Town;
            LocalContext.IsEdit = true;
            await _navigationService.NavigateToAsync<AddTownPageViewModel>();
        }

        private async void GoToBack(object obj)
        {
            // Do something
            await _navigationService.GoBackAsync();
        }

        private async void AddTownClicked(object obj)
        {
            // Do something
            try
            {
                LocalContext.Districts = null;
                LocalContext.IsEdit = false;
                await _navigationService.NavigateToAsync<AddTownPageViewModel>();
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
                districts = new List<District>();
                var province = Province.Id;
                if (Province.Id == "")
                {
                    return;
                }
                District = new District();

                DistrictCollection.Clear();
                TownCollection.Clear();

                districts = await RealtimeFirebase.Instance.GetListDistrict(province);
                foreach (var x in districts)
                {
                    DistrictCollection.Add(x);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }

        }
        public async void DistrictChangeClicked()
        {
            try
            {
                List<Town> towns = new List<Town>();
                var district = District.Id;
                if(District.Id == "")
                {
                    return;
                }
                TownCollection.Clear();
                towns = await RealtimeFirebase.Instance.GetListTown(district);
                LocalContext.Towns = towns;
                foreach (var x in towns)
                {
                    TownCollection.Add(x);
                }
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
                List<Town> tmp = LocalContext.Towns;
                if (search == null || search == "")
                {
                    foreach (var item in tmp)
                    {
                        TownCollection.Add(item);
                    }
                    return;
                }

                this.TownCollection.Clear();
                foreach (var item in tmp)
                {
                    if (item.Name.ToLower().Contains(search.ToLower()))
                    {
                        TownCollection.Add(item);
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
