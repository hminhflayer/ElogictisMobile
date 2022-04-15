using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using Newtonsoft.Json;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ElogictisMobile.ViewModels
{
    public class FailedProductPageViewModel : BaseViewModel
    {
        private string textFailed;
        private INavigationService _navigationService;

        public Command FailedProductCommand { get; set; }

        public string TextFailed
        {
            get
            {
                return this.textFailed;
            }

            set
            {
                if (this.textFailed == value)
                {
                    return;
                }

                this.SetProperty(ref this.textFailed, value);
            }
        }

        public FailedProductPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.FailedProductCommand = new Command(this.ConfirmClicked);
        }

        private async void ConfirmClicked(object obj)
        {
            Products products = LocalContext.ProductSelected;
            products.Status = 5;
            products.Status_ext = "GIAO KHÔNG THÀNH CÔNG";

            var upsert1 = await RealtimeFirebase.Instance.UpSert("DeliveryTracking/" + products.ID, "03", JsonConvert.SerializeObject(new ProductDeliveryTrackingModel
            {
                Date = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString(),
                ProgressValue = products.Status,
                Status = "Completed",
                StepStatus = StepStatus.Completed,
                Title = "GIAO HÀNG KHÔNG THÀNH CÔNG",
                TitleStatus = TextFailed
            }));

            var upsert = await RealtimeFirebase.Instance.UpSert("Products", products.ID, JsonConvert.SerializeObject(products));

            if (upsert)
            {
                await _navigationService.GoBackAsync();
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xác nhận đơn hàng giao không thành công!", "OK");

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Xác nhận thất bại!", "OK");
            }
        }
    }
}
