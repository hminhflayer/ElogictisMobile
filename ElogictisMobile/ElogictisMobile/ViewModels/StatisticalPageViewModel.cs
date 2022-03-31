using ElogictisMobile.Models;
using ElogictisMobile.Services;
using System;
using System.Collections.ObjectModel;
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

        /// <summary>
        /// To store the health care data collection.
        /// </summary>
        private ObservableCollection<HealthCare> healthCareCardItems;

        /// <summary>
        /// To store the health care data collection.
        /// </summary>
        private ObservableCollection<HealthCare> healthCareListItems;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="StatisticalPageViewModel" /> class.
        /// </summary>
        public StatisticalPageViewModel()
        {
            this.healthCareCardItems = new ObservableCollection<HealthCare>()
            {
                new HealthCare()
                {
                    Category = "CHỜ XÁC NHẬN",
                    CategoryValue = 0 +" đơn",
                    BackgroundGradientStart = "#f59083",
                    BackgroundGradientEnd = "#fae188",
                },
                new HealthCare()
                {
                    Category = "ĐANG GIAO HÀNG",
                    CategoryValue = 0 +" đơn",
                    BackgroundGradientStart = "#ff7272",
                    BackgroundGradientEnd = "#f650c5",
                },
                new HealthCare()
                {
                    Category = "GIAO THÀNH CÔNG",
                    CategoryValue = 0 +" đơn",
                    BackgroundGradientStart = "#5e7cea",
                    BackgroundGradientEnd = "#1dcce3",
                },
                new HealthCare()
                {
                    Category = "ĐÃ HỦY",
                    CategoryValue = 0 +" đơn",
                    BackgroundGradientStart = "#255ea6",
                    BackgroundGradientEnd = "#b350d1",
                },
            };

            this.healthCareListItems = new ObservableCollection<HealthCare>()
            {
                new HealthCare()
                {
                    Category = "Tổng chi phí vận chuyển",
                    CategoryValue = "0 VNĐ",
                    BackgroundGradientStart = "#8691ff",
                },
                new HealthCare()
                {
                    Category = "Tổng chi phí vận chuyển trong tháng",
                    CategoryValue = "0 VNĐ",
                    BackgroundGradientStart = "#ff9686",
                },
                new HealthCare()
                {
                    Category = "Số tiền trong tài khoản",
                    CategoryValue = LocalContext.Current.AccountSettings.Money +" VNĐ",
                    BackgroundGradientStart = "#cf86ff",
                },
            };

            this.ProfileImage = App.ImageServerPath + "ProfileImage1.png";
            this.MenuCommand = new Command(this.MenuButtonClicked);
            this.ProfileSelectedCommand = new Command(this.ProfileImageClicked);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the profile image path.
        /// </summary>
        public string ProfileImage { get; set; }

        /// <summary>
        /// Gets the health care items collection.
        /// </summary>
        public ObservableCollection<HealthCare> HealthCareCardItems
        {
            get
            {
                return this.healthCareCardItems;
            }

            private set
            {
                this.SetProperty(ref this.healthCareCardItems, value);
            }
        }

        /// <summary>
        /// Gets the health care items collection.
        /// </summary>
        public ObservableCollection<HealthCare> HealthCareListItems
        {
            get
            {
                return this.healthCareListItems;
            }

            private set
            {
                this.SetProperty(ref this.healthCareListItems, value);
            }
        }

        #endregion

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the menu button is clicked.
        /// </summary>
        public Command MenuCommand { get; set; }

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
        private void MenuButtonClicked(object obj)
        {
            // Do something
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
