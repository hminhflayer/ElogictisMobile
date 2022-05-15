using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ElogictisMobile.ViewModels
{
    public class MapCreateProductPageViewModel : BaseViewModel
    {
        private Command<object> fromMapClickedCommand;
        private Command<object> toMapClickedCommand;
        private Command<object> typeShipeChangeCommand;
        private Command<object> submitCommand;

        public string fromAddress;
        public string toAddress;
        public ObservableCollection<Pin> FromLocations { get; set; }
        public ObservableCollection<Pin> ToLocations { get; set; }

        public List<TypeShipProduct> TypeShipProducts { get; set; }
        public TypeShipProduct TypeShipProduct { get; set; }

        public List<PriceList> PriceLists { get; set; }

        private INavigationService _navigationService;

        public MapCreateProductPageViewModel(INavigationService navigationService)
        {
            FromLocations = new ObservableCollection<Pin>();
            ToLocations = new ObservableCollection<Pin>();
            PriceLists = new List<PriceList>();
            TypeShipProducts = new List<TypeShipProduct>();

            TypeShipProducts = LocalContext.ListTypeShipProductCollection;
            if(TypeShipProducts.Count != 0)
            {
                TypeShipProduct = TypeShipProducts[0];
                TypeShipProductChange(null);
            }    

            _navigationService = navigationService;
            FromAddress = "Lấy hàng tại đâu?";
            ToAddress = "Giao đến đâu?";

            if (!string.IsNullOrEmpty(LocalContext.TmpProduct.From_Address))
            {
                FromAddress = LocalContext.TmpProduct.From_Address;
            }
            if (!string.IsNullOrEmpty(LocalContext.TmpProduct.To_Address))
            {
                ToAddress = LocalContext.TmpProduct.To_Address;
            }
        }
        public string FromAddress
        {
            get
            {
                return this.fromAddress;
            }

            set
            {
                if (this.fromAddress == value)
                {
                    return;
                }

                this.SetProperty(ref this.fromAddress, value);
            }
        }
        public string ToAddress
        {
            get
            {
                return this.toAddress;
            }

            set
            {
                if (this.toAddress == value)
                {
                    return;
                }

                this.SetProperty(ref this.toAddress, value);
            }
        }

        public Command<object> TypeShipeChangeCommand
        {
            get
            {
                return this.typeShipeChangeCommand ?? (this.typeShipeChangeCommand = new Command<object>(this.TypeShipProductChange));
            }
        }

        public Command<object> SubmitCommand
        {
            get
            {
                return this.submitCommand ?? (this.submitCommand = new Command<object>(this.SubmitClicked));
            }
        }

        public Command<object> FromMapClickedCommand
        {
            get
            {
                return this.fromMapClickedCommand ?? (this.fromMapClickedCommand = new Command<object>(this.FromMapClicked));
            }
        }

        public Command<object> ToMapClickedCommand
        {
            get
            {
                return this.toMapClickedCommand ?? (this.toMapClickedCommand = new Command<object>(this.ToMapClicked));
            }
        }

        private async void FromMapClicked(object obj)
        {
            try
            {
                LocalContext.IsFromAddress = true;
                await _navigationService.NavigateToAsync<MapExtendPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }

        private async void ToMapClicked(object obj)
        {
            try
            {
                LocalContext.IsFromAddress = false;
                await _navigationService.NavigateToAsync<MapExtendPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }

        private async void SubmitClicked(object obj)
        {
            try
            {
                if(string.IsNullOrEmpty(LocalContext.TmpProduct.From_Address) || 
                    string.IsNullOrEmpty(LocalContext.TmpProduct.To_Address))
                {
                    await App.Current.MainPage.DisplayAlert("Lỗi", "Chưa có thông tin địa chỉ lấy hàng hoặc nhận hàng!", "OK");
                    return;
                }    
                await _navigationService.NavigateToAsync<AddProductFormPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }

        private async void TypeShipProductChange(object obj)
        {
            try
            {
                PriceLists.Clear();
                PriceLists = await RealtimeFirebase.Instance.GetPriceListWithTypeShip(TypeShipProduct.Id);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }
    }
}
