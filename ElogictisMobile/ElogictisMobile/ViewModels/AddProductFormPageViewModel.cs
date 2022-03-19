using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for Business Registration Form page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class AddProductFormPageViewModel : BaseViewModel
    {
        #region Constructor
        public ValidatableObject<string> fromFullName;
        public ValidatableObject<string> fromPhone;
        public ValidatableObject<string> fromAddress;
        public ValidatableObject<string> toFullName;
        public ValidatableObject<string> toPhone;
        public ValidatableObject<string> toAddress;
        public ValidatableObject<int> quanlity;
        public ValidatableObject<double> weight;
        public ValidatableObject<string> desciption;
        public ValidatableObject<double> money;
        private INavigationService _navigationService;

        public string TypeProduct { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public AddProductFormPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            this.SubmitCommand = new Command(this.SubmitClicked);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the From Full Name from user.
        /// </summary>


        public ValidatableObject<string> FromFullName
        {
            get
            {
                return this.fromFullName;
            }

            set
            {
                if (this.fromFullName == value)
                {
                    return;
                }

                this.SetProperty(ref this.fromFullName, value);
            }
        }
        public ValidatableObject<string> FromPhone
        {
            get
            {
                return this.fromPhone;
            }

            set
            {
                if (this.fromPhone == value)
                {
                    return;
                }

                this.SetProperty(ref this.fromPhone, value);
            }
        }
        public ValidatableObject<string> FromAddress
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
        public ValidatableObject<string> ToFullName
        {
            get
            {
                return this.toFullName;
            }

            set
            {
                if (this.toFullName == value)
                {
                    return;
                }

                this.SetProperty(ref this.toFullName, value);
            }
        }
        public ValidatableObject<string> ToPhone
        {
            get
            {
                return this.toPhone;
            }

            set
            {
                if (this.toPhone == value)
                {
                    return;
                }

                this.SetProperty(ref this.toPhone, value);
            }
        }
        public ValidatableObject<string> ToAddress
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
        public ValidatableObject<int> Quanlity
        {
            get
            {
                return this.quanlity;
            }

            set
            {
                if (this.quanlity == value)
                {
                    return;
                }

                this.SetProperty(ref this.quanlity, value);
            }
        }
        public ValidatableObject<double> Weight
        {
            get
            {
                return this.weight;
            }

            set
            {
                if (this.weight == value)
                {
                    return;
                }

                this.SetProperty(ref this.weight, value);
            }
        }
        public ValidatableObject<string> Desciption
        {
            get
            {
                return this.desciption;
            }

            set
            {
                if (this.desciption == value)
                {
                    return;
                }

                this.SetProperty(ref this.desciption, value);
            }
        }
        public ValidatableObject<double> Money
        {
            get
            {
                return this.money;
            }

            set
            {
                if (this.money == value)
                {
                    return;
                }

                this.SetProperty(ref this.money, value);
            }
        }

        #endregion 

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializzing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.FromFullName = new ValidatableObject<string>();
            this.FromPhone = new ValidatableObject<string>();
            this.FromAddress = new ValidatableObject<string>();
            this.ToFullName = new ValidatableObject<string>();
            this.ToPhone = new ValidatableObject<string>();
            this.ToAddress = new ValidatableObject<string>();
            this.Quanlity = new ValidatableObject<int>();
            this.Weight = new ValidatableObject<double>();
            this.Desciption = new ValidatableObject<string>();
            this.Money = new ValidatableObject<double>();

            Weight.Value = 0;
            Quanlity.Value = 0;
            Money.Value = 0;
        }

        /// <summary>
        /// Validation rule for name
        /// </summary>
        private void AddValidationRules()
        {
            this.FromFullName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Họ và tên người gửi không được trống" });
            this.FromPhone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Số điện thoại người gửi không được trống" });
            this.FromAddress.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Địa chỉ người gửi không được trống" });
            this.ToFullName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Họ và tên người nhận không được trống" });
            this.ToPhone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Số điện thoại người nhận không được trống" });
            this.ToAddress.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Địa chỉ người nhận không được trống" });
            this.Quanlity.Validations.Add(new IsNotNullOrEmptyRule<int> { ValidationMessage = "Số lượng kiện hàng không được trống" });
            this.Weight.Validations.Add(new IsNotNullOrEmptyRule<double> { ValidationMessage = "Tổng trọng lượng không được trống" });
            this.Money.Validations.Add(new IsNotNullOrEmptyRule<double> { ValidationMessage = "Số tiền thu hộ không được trống" });
        }

        /// <summary>
        /// Check name is valid or not
        /// </summary>
        /// <returns>Returns the fields are valid or not</returns>
        private bool AreFieldsValid()
        {
            bool isFromFullName = this.FromFullName.Validate();
            bool isFromPhone = this.FromPhone.Validate();
            bool isFromAddress = this.FromAddress.Validate();
            bool isToFullName = this.ToFullName.Validate();
            bool isToPhone = this.ToPhone.Validate();
            bool isToAddress = this.ToAddress.Validate();
            bool isQuanlity = this.Quanlity.Validate() && this.Quanlity.Value >= 0;
            bool isWeight = this.Weight.Validate() && this.Weight.Value >= 0;
            bool isMoney = this.Money.Validate() && this.Money.Value >= 0 && this.Money.Value <= 2000000;

            return isFromFullName && isFromPhone && isFromAddress
                && isToFullName && isToPhone && isToAddress && isQuanlity
                && isWeight && isMoney;
        }

        /// <summary>
        /// Invoked when the Submit button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void SubmitClicked(object obj)
        {
            if (this.AreFieldsValid())
            {
                var key = GeneralKey.Instance.General("PRO");
                // Do Something
                await RealtimeFirebase.Instance.UpSert("Products", key, JsonConvert.SerializeObject(new Products
                {
                    Product_CreateBy = StaticClass.Profiles.Profile_Email,
                    Product_CreateTime = DateTime.Now.ToShortDateString(),
                    Product_Description = Desciption.Value,
                    Product_From_Address = FromAddress.Value,
                    Product_From_FullName = FromFullName.Value,
                    Product_From_PhoneNumber = FromPhone.Value,
                    Product_ID = key,
                    Product_IsDelete = false,
                    Product_LastUpdateBy = "",
                    Product_LastUpdateTime = "",
                    Product_Money = Money.Value.ToString(),
                    Product_Quanlity = Quanlity.Value.ToString(),
                    Product_To_Address = ToAddress.Value,
                    Product_To_FullName = ToFullName.Value,
                    Product_To_PhoneNumber = ToPhone.Value,
                    Product_Type = TypeProduct,
                    Product_Weight = Weight.Value.ToString()
                }));
                await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm đơn hàng thành công!", "OK");
                await _navigationService.GoBackAsync();
            }
        }

        #endregion
    }
}