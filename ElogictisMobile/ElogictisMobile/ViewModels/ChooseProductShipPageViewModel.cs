using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ElogictisMobile.ViewModels
{
    public class ChooseProductShipPageViewModel : BaseViewModel
    {
        public List<Products> products;
        private bool isChecked;
        public Command MapCommand { get; set; }

        private INavigationService _navigationService;

        public List<Products> selectedItems; // define `selectedItems` as the list of selected items.

        public ChooseProductShipPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            selectedItems = new List<Products>(); // init the `selectedItems`

            Products = new List<Products>();

            LoadData();
            this.MapCommand = new Command(this.MapClicked);
        }

        public async void LoadData()
        {
            Products = await RealtimeFirebase.Instance.GetAllProductGetedIsShiping();
        }

        private async void MapClicked(object obj)
        {
            try
            {
                LocalContext.ProductsDelivery = selectedItems;
                LocalContext.RoundTrip = this.isChecked;
                await _navigationService.NavigateToAsync<MapDeliveryPageViewModel>();
                // Do something
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            }
        }

        public List<Products> Products
        {
            get
            {
                return this.products;
            }

            set
            {
                if (this.products == value)
                {
                    return;
                }

                this.SetProperty(ref this.products, value);
            }
        }

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                if (this.isChecked == value)
                {
                    return;
                }

                this.SetProperty(ref this.isChecked, value);
            }
        }
    }
}
