using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for stock overview page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class StatisticalPageViewModel : BaseViewModel
    {
        #region Field

        private int valueWait;
        private int valueShipped;
        private int valueSuccess;
        private int valueFail;
        private double valueTotalRevenue;
        private double valueMonthRevenue;
        private double valueMoney;
        private string textTotalRevenue;
        private string textMonthRevenue;
        private string textDetail;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="StatisticalPageViewModel" /> class.
        /// </summary>
        public StatisticalPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.MenuCommand = new Command(this.MenuButtonClicked);
            this.HistoryOrderCommand = new Command(this.HistoryButtonClicked);

            TextTotalRevenue = "Tổng doanh thu: ";
            TextMonthRevenue = "Doanh thu trong tháng: ";

            IsLoading = true;
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                if(LocalContext.Current.AccountSettings == null)
                {
                    return false;
                }    
                if(string.IsNullOrEmpty(LocalContext.Current.AccountSettings.Id))
                {
                    return false;
                }    
                else
                {
                    Device.BeginInvokeOnMainThread(() => LoadStatistical());
                    return true;
                }
                
            });
        }

        public async void LoadStatistical()
        {
            LocalContext.ListTypeProduct = await RealtimeFirebase.Instance.GetTypeProduct();

            if(LocalContext.IsAdmin)
            {
                this.ValueWait = await RealtimeFirebase.Instance.GetStatisticalAdmin(1);
                this.ValueShipped = await RealtimeFirebase.Instance.GetStatisticalAdmin(3);
                this.ValueSuccess = await RealtimeFirebase.Instance.GetStatisticalAdmin(4);
                this.ValueFail = await RealtimeFirebase.Instance.GetStatisticalAdmin(5);
                this.ValueTotalRevenue = await RealtimeFirebase.Instance.TotalRevenueAdmin(true);
                this.ValueMonthRevenue = await RealtimeFirebase.Instance.TotalRevenueAdmin(false);
            }   
            else if(LocalContext.IsManager)
            {
                this.ValueWait = await RealtimeFirebase.Instance.GetStatisticalAgency(1);
                this.ValueShipped = await RealtimeFirebase.Instance.GetStatisticalAgency(3);
                this.ValueSuccess = await RealtimeFirebase.Instance.GetStatisticalAgency(4);
                this.ValueFail = await RealtimeFirebase.Instance.GetStatisticalAgency(5);
                this.ValueTotalRevenue = await RealtimeFirebase.Instance.TotalRevenueAgency(true);
                this.ValueMonthRevenue = await RealtimeFirebase.Instance.TotalRevenueAgency(false);
            }   
            else if(LocalContext.IsShipper)
            {
                this.ValueWait = await RealtimeFirebase.Instance.GetStatisticalShipper(2);
                this.ValueShipped = await RealtimeFirebase.Instance.GetStatisticalShipper(3);
                this.ValueSuccess = await RealtimeFirebase.Instance.GetStatisticalShipper(4);
                this.ValueFail = await RealtimeFirebase.Instance.GetStatisticalShipper(5);
                this.ValueTotalRevenue = await RealtimeFirebase.Instance.TotalRevenueShipper(true);
                this.ValueMonthRevenue = await RealtimeFirebase.Instance.TotalRevenueShipper(false);
            }   
            else
            {
                TextTotalRevenue = "Tổng chi phí vận chuyển: ";
                TextMonthRevenue = "Chi phí vận chuyển trong tháng: ";

                this.ValueWait = await RealtimeFirebase.Instance.GetStatisticalUser(1);
                this.ValueShipped = await RealtimeFirebase.Instance.GetStatisticalUser(3);
                this.ValueSuccess = await RealtimeFirebase.Instance.GetStatisticalUser(4);
                this.ValueFail = await RealtimeFirebase.Instance.GetStatisticalUser(5);
                this.ValueTotalRevenue = await RealtimeFirebase.Instance.TotalRevenueUser(true);
                this.ValueMonthRevenue = await RealtimeFirebase.Instance.TotalRevenueUser(false);
            }

            LocalContext.Current.AccountSettings = await RealtimeFirebase.Instance.GetProfiles(LocalContext.Current.AccountSettings.Id);
            this.ValueMoney = LocalContext.Current.AccountSettings.Money;
            IsLoading = false;

        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the profile image path.
        /// </summary>
        public bool IsNotUser { get; set; } = (LocalContext.IsAdmin || LocalContext.IsManager || LocalContext.IsShipper);
        public bool IsShipper { get; set; } = !(LocalContext.IsShipper);
        public string TextDetail
        {
            get
            {
                return this.textDetail;
            }

            private set
            {
                this.SetProperty(ref this.textDetail, value);
            }
        }

        /// <summary>
        /// Gets the health care items collection.
        /// </summary>
        public int ValueWait
        {
            get
            {
                return this.valueWait;
            }

            private set
            {
                this.SetProperty(ref this.valueWait, value);
            }
        }
        public int ValueShipped
        {
            get
            {
                return this.valueShipped;
            }

            private set
            {
                this.SetProperty(ref this.valueShipped, value);
            }
        }
        public int ValueSuccess
        {
            get
            {
                return this.valueSuccess;
            }

            private set
            {
                this.SetProperty(ref this.valueSuccess, value);
            }
        }
        public int ValueFail
        {
            get
            {
                return this.valueFail;
            }

            private set
            {
                this.SetProperty(ref this.valueFail, value);
            }
        }
        public double ValueTotalRevenue
        {
            get
            {
                return this.valueTotalRevenue;
            }

            private set
            {
                this.SetProperty(ref this.valueTotalRevenue, value);
            }
        }
        public string TextTotalRevenue
        {
            get
            {
                return this.textTotalRevenue;
            }

            private set
            {
                this.SetProperty(ref this.textTotalRevenue, value);
            }
        }
        public string TextMonthRevenue
        {
            get
            {
                return this.textMonthRevenue;
            }

            private set
            {
                this.SetProperty(ref this.textMonthRevenue, value);
            }
        }
        public double ValueMonthRevenue
        {
            get
            {
                return this.valueMonthRevenue;
            }

            private set
            {
                this.SetProperty(ref this.valueMonthRevenue, value);
            }
        }
        public double ValueMoney
        {
            get
            {
                return this.valueMoney;
            }

            private set
            {
                this.SetProperty(ref this.valueMoney, value);
            }
        }

        private INavigationService _navigationService;

        #endregion

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the menu button is clicked.
        /// </summary>
        public Command MenuCommand { get; set; }
        public Command HistoryOrderCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the profile image is clicked.
        /// </summary>
        public Command ProfileSelectedCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the menu button is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void MenuButtonClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<DetailRevenuePageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void HistoryButtonClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<HistoryOrderPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        /// <summary>
        /// Invoked when the profile image is clicked.
        /// </summary>
        /// <param name="obj">The object</param>
        private void ProfileImageClicked(object obj)
        {
            // Do something
        }

        #endregion
    }
}
