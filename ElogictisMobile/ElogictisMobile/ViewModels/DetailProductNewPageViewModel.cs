using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using Newtonsoft.Json;
using Syncfusion.XForms.ProgressBar;
using System;
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
    public class DetailProductNewPageViewModel : BaseViewModel
    {
        #region Field
        public string FromFullName { get; set; }
        public string FromPhoneNumber { get; set; }
        public string FromAddress{ get; set; }
        public string ToFullName { get; set; }
        public string ToPhoneNumber { get; set; }
        public string ToAddress { get; set; }
        public int Quanlity { get; set; }
        public double TotalWeight { get; set; }
        public string Desciption { get; set; }
        public double Money { get; set; }
        public string NameProduct { get; set; }
        private INavigationService _navigationService;

        public string TypeProduct { get; set; }
        #endregion
        #region Constructor 
        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public DetailProductNewPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.ConfirmProductCommand = new Command(this.SubmitClicked);
        }

        #endregion

        #region Properties


        #endregion 

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command ConfirmProductCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializzing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.FromFullName = LocalContext.ProductSelected.From_FullName;
            this.FromPhoneNumber = LocalContext.ProductSelected.From_PhoneNumber;
            this.FromAddress = LocalContext.ProductSelected.From_Address;
            this.ToFullName = LocalContext.ProductSelected.To_FullName;
            this.ToPhoneNumber = LocalContext.ProductSelected.To_PhoneNumber;
            this.ToAddress = LocalContext.ProductSelected.To_Address;
            this.TotalWeight = LocalContext.ProductSelected.Weight;
            this.Quanlity = LocalContext.ProductSelected.Quanlity;
            this.Desciption = LocalContext.ProductSelected.Description;
            this.Money = LocalContext.ProductSelected.Money;
            this.TypeProduct = LocalContext.ProductSelected.Type_ext;
            this.NameProduct = LocalContext.ProductSelected.Name;
        }

        /// <summary>
        /// Invoked when the Submit button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void SubmitClicked(object obj)
        {
            try
            {
                var keyNoti = GeneralKey.Instance.General("NOTI");
                Products temp = LocalContext.ProductSelected;
                temp.Status = 2;
                temp.Status_ext = "CHỜ LẤY ĐƠN HÀNG";
                temp.IsConfirm = true;
                temp.Holder = LocalContext.Current.AccountSettings.Id;

                var upsert = await RealtimeFirebase.Instance.UpSert("Products", temp.ID, JsonConvert.SerializeObject(temp));
                var upsert1 = await RealtimeFirebase.Instance.UpSert("DeliveryTracking/" + temp.ID, "01", JsonConvert.SerializeObject(new ProductDeliveryTrackingModel
                {
                    Date = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString(),
                    ProgressValue = temp.Status,
                    Status = "NotStarted",
                    StepStatus = StepStatus.NotStarted,
                    Title = LocalContext.Current.AccountSettings.Name + ": ĐÃ NHẬN ĐƠN HÀNG",
                    TitleStatus = "Đơn hàng được Shipper nhận"
                }));
                if (upsert && upsert1)
                {
                    //await RealtimeFirebase.Instance.UpSert("Notifications", keyNoti, JsonConvert.SerializeObject(new TransactionHistory
                    //{
                    //    IdProduct = temp.ID,
                    //    TransactionDescription = "CHỜ LẤY ĐƠN HÀNG",
                    //    Date = DateTime.Now.ToShortDateString(),
                    //    Time = DateTime.Now.ToShortTimeString(),
                    //    Email = LocalContext.Profiles.Email,
                    //    ProfileId = LocalContext.Current.AccountSettings.Id
                    //}));
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Nhận đơn hàng thành công!", "OK");
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Nhận đơn hàng không thành công!", "OK");
                }    
            }
            catch
            (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        #endregion
    }
}
