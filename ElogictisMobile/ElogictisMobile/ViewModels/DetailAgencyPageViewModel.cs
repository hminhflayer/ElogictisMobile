using ElogictisMobile.DataService;
using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for no item page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class DetailAgencyPageViewModel : BaseViewModel
    {
        #region Field
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

        public Category TypeProduct { get; set; }
        public ObservableCollection<Category> TypeProductCollection { get; set; } = ContentData.TypeProductCollection;

        #endregion
        #region Constructor 
        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public DetailAgencyPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            this.UpdateProductCommand = new Command(this.SubmitClicked);
            this.DeleteProductCommand = new Command(this.DeleteClicked);
        }

        #endregion

        #region Properties
        public bool IsManage { get; set; } = LocalContext.IsManager;
        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the From Full Name from user.
        /// </summary>
        public string Status { get; set; } = LocalContext.ProductSelected.Status_ext;

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
        public Command UpdateProductCommand { get; set; }
        public Command DeleteProductCommand { get; set; }

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

            var tmp = new Category()
            {
                Id = LocalContext.ProductSelected.Type,
                Name = LocalContext.ProductSelected.Type_ext
            };
            this.FromFullName.Value = LocalContext.ProductSelected.From_FullName;
            this.FromPhone.Value = LocalContext.ProductSelected.From_PhoneNumber;
            this.FromAddress.Value = LocalContext.ProductSelected.From_Address;
            this.ToFullName.Value = LocalContext.ProductSelected.To_FullName;
            this.ToPhone.Value = LocalContext.ProductSelected.To_PhoneNumber;
            this.ToAddress.Value = LocalContext.ProductSelected.To_Address;
            this.Weight.Value = double.Parse(LocalContext.ProductSelected.Weight);
            this.Quanlity.Value = int.Parse(LocalContext.ProductSelected.Quanlity);
            this.Desciption.Value = LocalContext.ProductSelected.Description;
            this.Money.Value = LocalContext.ProductSelected.Money;
            this.TypeProduct = tmp;
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
                Products temp = LocalContext.ProductSelected;
                temp.Description = Desciption.Value;
                temp.From_Address = FromAddress.Value;
                temp.From_FullName = FromFullName.Value;
                temp.From_PhoneNumber = FromPhone.Value;
                temp.LastUpdateBy = LocalContext.Current.AccountSettings.Id;
                temp.LastUpdateTime = DateTime.Now.ToShortDateString();
                temp.Money = Money.Value;
                temp.Quanlity = Quanlity.Value.ToString();
                temp.To_Address = ToAddress.Value;
                temp.To_FullName = ToFullName.Value;
                temp.To_PhoneNumber = ToPhone.Value;
                temp.Type = TypeProduct.Id;
                temp.Type_ext = TypeProduct.Name;
                temp.Weight = Weight.Value.ToString();
                temp.Holder = "";

                await RealtimeFirebase.Instance.UpSert("Products", temp.ID, JsonConvert.SerializeObject(temp));
                await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thành công!", "OK");
                await _navigationService.GoBackAsync();
            }
        }

        /// <summary>
        /// Invoked when the Delete button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void DeleteClicked(object obj)
        {
            // Do something
            var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa thông tin đơn hàng này?", "Đúng", "Không");
            if (action)
            {
                Products products = LocalContext.ProductSelected;
                products.IsDelete = true;
                products.LastUpdateBy = LocalContext.Profiles.Email;
                products.LastUpdateTime = DateTime.Now.ToString();

                // Do Something
                await RealtimeFirebase.Instance.UpSert("Products", products.ID, JsonConvert.SerializeObject(products));
                //await RealtimeFirebase.Instance.Delete("Products", products.ID);
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xóa thông tin đơn hàng thành công", "OK");
            }
        }

        #endregion
    }
}
