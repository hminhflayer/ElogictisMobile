using ElogictisMobile.Models;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ElogictisMobile.ViewModels
{
    public class MapExtendPageViewModel :BaseViewModel
    {
        private Command<object> mapClickedCommand;
        private Command<object> saveClickedCommand;
        private Command<object> findClickedCommand;
        private Command<object> clearCommand;

        private string address;

        public ObservableCollection<Pin> MapLocations { get; set; }

        private INavigationService _navigationService;

        public MapExtendPageViewModel(INavigationService navigationService)
        {
            MapLocations = new ObservableCollection<Pin>();

            _navigationService = navigationService;
        }

        public string Address
        {
            get
            {
                return this.address;
            }

            set
            {
                if (this.address == value)
                {
                    return;
                }

                this.SetProperty(ref this.address, value);
            }
        }

        public Command<object> ClearCommand
        {
            get
            {
                return this.clearCommand ?? (this.clearCommand = new Command<object>(this.ClearCLicked));
            }
        }

        public Command<object> FindClickedCommand
        {
            get
            {
                return this.findClickedCommand ?? (this.findClickedCommand = new Command<object>(this.FindClick));
            }
        }

        public Command<object> SaveClickedCommand
        {
            get
            {
                return this.saveClickedCommand ?? (this.saveClickedCommand = new Command<object>(this.SaveClicked));
            }
        }
        public Command<object> MapClickedCommand
        {
            get
            {
                return this.mapClickedCommand ?? (this.mapClickedCommand = new Command<object>(this.MapClicked));
            }
        }

        private double GetDistanceOnMap()
        {
            double distance = 0;
            return distance;
        }

        private async void FindClick(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Address))
                {
                    await App.Current.MainPage.DisplayAlert("Lỗi", "Không thể tìm địa chỉ rỗng!", "OK");
                    return;
                }

                Geocoder geoCoder = new Geocoder();
                IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(Address);
                Position position = approximateLocations.FirstOrDefault();
                Pin pin = new Pin()
                {
                    Label = Address,
                    Position = position
                };
                MapLocations.Clear();
                MapLocations.Add(pin);
            }   
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }    

        private async void SaveClicked(object obj)
        {
            if(!string.IsNullOrEmpty(Address))
            {
                if(LocalContext.IsFromAddress)
                {
                    LocalContext.TmpProduct.From_Address = Address;
                    LocalContext.TmpProduct.LngFromAddress = MapLocations[0].Position.Longitude;
                    LocalContext.TmpProduct.LatFromAddress = MapLocations[0].Position.Latitude;
                }   
                else
                {
                    LocalContext.TmpProduct.To_Address = Address;
                    LocalContext.TmpProduct.LngToAddress = MapLocations[0].Position.Longitude;
                    LocalContext.TmpProduct.LatToAddress = MapLocations[0].Position.Latitude;
                }

                await _navigationService.NavigateToAsync<MapCreateProductPageViewModel>();
            }    
            else
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", "Không thể lưu địa chỉ rỗng!", "OK");
            }    
        }

        private async void MapClicked(object obj)
        {
            Position position = (Position)obj;
            Pin pin = new Pin()
            {
                Label = position.ToString(),
                Position = position
            };

            if(string.IsNullOrEmpty(Address))
            {
                MapLocations.Clear();
                MapLocations.Add(pin);
                Position position1 = (Position)obj;
                Geocoder geoCoder = new Geocoder();
                IEnumerable<string> approximateLocations = await geoCoder.GetAddressesForPositionAsync(position1);
                Address = approximateLocations.FirstOrDefault();
            }    
            else
            {
                MapLocations.Clear();
                MapLocations.Add(pin);
            }    
        }

        private void ClearCLicked(object obj)
        {
            Address = "";
            MapLocations.Clear();
        }
    }
}
