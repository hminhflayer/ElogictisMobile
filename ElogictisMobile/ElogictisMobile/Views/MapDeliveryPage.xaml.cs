using ElogictisMobile.Models;
using ElogictisMobile.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapDeliveryPage : ContentPage
	{
		private string destination = "";

        // add the polyline to the map's MapElements collection
		public MapDeliveryPage ()
		{
			InitializeComponent ();

            LoadMap();

        }

		public async void LoadMap()
        {
			try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    map.MoveToRegion(new MapSpan(new Position(location.Latitude, location.Longitude), 0.1, 0.1));
                    var lngtmp = location.Longitude.ToString().Replace(',', '.');
                    var lattmp = location.Latitude.ToString().Replace(',', '.');
                    destination += lngtmp + "," + lattmp + ";";
                };
                if (LocalContext.ProductsDelivery.Count > 0)
				{
					foreach (var item in LocalContext.ProductsDelivery)
					{
						if (item.LatToAddress != 0 && item.LngToAddress != 0)
						{
                            var lng = item.LngToAddress.ToString().Replace(',', '.');
                            var lat = item.LatToAddress.ToString().Replace(',', '.');
                            destination += lng + "," + lat + ";";
						}
					}

					destination = destination.Substring(0, destination.Length-1);
                    var client = new HttpClient();
                    var key = "pk.eyJ1IjoiaG1pbmhmbGF5ZXIiLCJhIjoiY2wzOXY1dHZ6MDl4aTNpcGVybXh6ZzAwNiJ9.T0Uoz4Rx97j_fyHVjZYWOQ";
                    var uri = "https://api.mapbox.com/optimized-trips/v1/mapbox/driving/"+destination+"?source=first&roundtrip=true&access_token="+key;
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(uri)
                    };
                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<MapBoxJson>(body);
						var poli = GeneralKey.Instance.DecodePolyline(data.Trips[0].Geometry);
                        Polyline polyline = new Polyline
                        {
                            StrokeColor = Color.Blue,
                            StrokeWidth = 12
                        };

                        foreach(var item in poli)
                        {
                            polyline.Geopath.Add(item);
                        }

                        foreach(var item in LocalContext.ProductsDelivery)
                        {
                            map.Pins.Add(new Pin()
                            {
                                Label = "Đơn: " +item.Name + "\nPhí: " +item.Money +" VNĐ",
                                Position = new Position(item.LatToAddress, item.LngToAddress),
                                Type = PinType.SavedPin
                        });
                        }
                        map.MapElements.Add(polyline);
                    }
                }
			}
            catch (FeatureNotSupportedException fnsEx)
            {
                DisplayAlert("Lỗi", "Thiết bị không hỗ trợ GPS", "OK");
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                DisplayAlert("Lỗi", "Thiết bị chưa bật GPS", "OK");
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                DisplayAlert("Lỗi", "Ứng dụng không có quyền truy cập GPS", "OK");
                // Handle permission exception
            }
            catch (Exception ex)
            {
				DisplayAlert("Lỗi", ex.Message, "OK");
            }			
        }
	}
}