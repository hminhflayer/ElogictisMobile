using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        public List<PriceList> priceLists;

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
        public List<PriceList> PriceLists
        {
            get
            {
                return this.priceLists;
            }

            set
            {
                if (this.priceLists == value)
                {
                    return;
                }

                this.SetProperty(ref this.priceLists, value);
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
                LocalContext.TmpProduct.DataDirections = await GetDataDirections(LocalContext.TmpProduct.From_Address, LocalContext.TmpProduct.To_Address);
                LocalContext.TmpProduct.DistanceEstimate = GetDistance(LocalContext.TmpProduct.DataDirections);

                await _navigationService.NavigateToAsync<AddProductFormPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }

        private double GetDistance(string direction)
        {
            var stuff = JsonConvert.DeserializeObject<MapDirections>(direction);

            var meters = stuff.Response.Directions.Routes[0].DistanceMeters;
            double distance = (meters / 1000.0);
            return distance;
        }

        private async Task<string> GetDataDirections(string from, string to)
        {
            var client = new HttpClient();
            var origin = from;
            var destination = to;
            var uri = "https://google-maps-directions.p.rapidapi.com/directions?origin="+ origin +"&destination="+ destination +"&distance_units=auto&avoid_routes=tolls%2Cferries&include_timed_distance=false&expand_routes=true&language=vi&region=vi";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Headers =
                {
                    { "X-RapidAPI-Host", "google-maps-directions.p.rapidapi.com"
                    },
                    { "X-RapidAPI-Key", "287d14cc02mshc91c6b64caf1e1ep1ee03ejsn96644ab367b9"
                    },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }

        private async void TypeShipProductChange(object obj)
        {
            try
            {
                PriceLists.Clear();
                LocalContext.TmpProduct.TypeShip = TypeShipProduct.Id;
                LocalContext.TmpProduct.TypeShip_ext = TypeShipProduct.Name;
                LocalContext.TmpProduct.ProductPrioritize = TypeShipProduct.Prioritize;
                PriceLists = await RealtimeFirebase.Instance.GetPriceListWithTypeShip(TypeShipProduct.Id);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }
    }
}
