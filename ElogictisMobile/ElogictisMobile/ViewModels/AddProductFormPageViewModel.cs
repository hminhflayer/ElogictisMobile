using ElogictisMobile.DataService;
using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;

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

        private Command<object> fromMapClickedCommand;
        private Command<object> toMapClickedCommand;

        public Category TypeProduct { get; set; }
        public ObservableCollection<Category> TypeProductCollection { get; set; } = ContentData.TypeProductCollection;

        public ObservableCollection<Pin> FromLocations { get; set; }
        public ObservableCollection<Pin> ToLocations { get; set; }
        public ObservableCollection<Category> WeightCollection { get; set; } = ContentData.WeightCollection;


        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public AddProductFormPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();

            FromLocations = new ObservableCollection<Pin>();
            ToLocations = new ObservableCollection<Pin>();

            this.SubmitCommand = new Command(this.SubmitClicked);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the From Full Name from user.
        /// </summary>
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

        public Command<string> FromAddressChangedCommand
        {
            get
            {
                return this.fromAddressChangedCommand ?? (this.fromAddressChangedCommand = new Command<string>(this.FromAddressChangeClicked));
            }
        }

        public Command<string> ToAddressChangedCommand
        {
            get
            {
                return this.toAddressChangedCommand ?? (this.toAddressChangedCommand = new Command<string>(this.ToAddressChangeClicked));
            }
        }

        public Command<double> WeightChangedCommand
        {
            get
            {
                return this.weightChangedCommand ?? (this.weightChangedCommand = new Command<double>(this.WeightChangeClicked));
            }
        }
        #endregion 

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }
        private Command<string> fromAddressChangedCommand;
        private Command<string> toAddressChangedCommand;
        private Command<double> weightChangedCommand;
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
            this.FromFullName.Value = LocalContext.Profiles.Name;
            this.FromPhone.Value = LocalContext.Profiles.Phone;
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
            try
            {
                if (LocalContext.Current.AccountSettings.Money == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn không còn tiền trong tài khoản!", "OK");
                    return;
                }
                if (!LocalContext.Current.AccountSettings.IsConfirm)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Tài khoản chưa được duyệt!\nKhông thể tạo đơn hàng", "OK");
                    return;
                }
                if (LocalContext.Current.AccountSettings.Money == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn không còn tiền trong tài khoản!", "OK");
                    return;
                }
                if (this.AreFieldsValid())
                {
                    var key = GeneralKey.Instance.General("PRO");
                    var keyNoti = GeneralKey.Instance.General("NOTI");
                    // Do Something
                    var task = await RealtimeFirebase.Instance.UpSert("Products", key, JsonConvert.SerializeObject(new Products
                    {
                        CreateBy = LocalContext.Current.AccountSettings.Id,
                        CreateTime = DateTime.Now.ToShortDateString(),
                        Description = Desciption.Value,
                        From_Address = FromAddress.Value,
                        From_FullName = FromFullName.Value,
                        From_PhoneNumber = FromPhone.Value,
                        ID = key,
                        IsDelete = false,
                        LastUpdateBy = "",
                        LastUpdateTime = "",
                        Money = Money.Value,
                        Quanlity = Quanlity.Value,
                        To_Address = ToAddress.Value,
                        To_FullName = ToFullName.Value,
                        To_PhoneNumber = ToPhone.Value,
                        Type = TypeProduct.Id,
                        Type_ext = TypeProduct.Name,
                        Weight = Weight.Value,
                        Status = 1,
                        Holder = "",
                        IsConfirm = false,
                        Status_ext = "CHỜ NHẬN ĐƠN",
                        AgencyId = LocalContext.Current.AccountSettings.AgencyId,
                    }));
                    if (task)
                    {
                        await RealtimeFirebase.Instance.UpSert("Notifications", keyNoti, JsonConvert.SerializeObject(new TransactionHistory
                        {
                            IdProduct = key,
                            TransactionDescription = "CHỜ NHẬN ĐƠN",
                            Date = DateTime.Now.ToShortDateString(),
                            Time = DateTime.Now.ToShortTimeString(),
                            Email = LocalContext.Profiles.Email,
                            ProfileId = LocalContext.Current.AccountSettings.Id
                        })); ;
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm đơn hàng thành công!", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm đơn hàng không thành công!", "OK");
                    }
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo",ex.Message, "OK");
            }
            
        }

        private void FromMapClicked(object obj)
        {
            Position position = (Position)obj;
            Pin pin = new Pin()
            {
                Label = "Địa chỉ người gửi",
                Position = position
            };
            FromLocations.Clear();
            FromLocations.Add(pin);
        }

        private void ToMapClicked(object obj)
        {
            Position position = (Position)obj;
            Pin pin = new Pin()
            {
                Label = "Địa chỉ người nhận",
                Position = position
            };
            ToLocations.Clear();
            ToLocations.Add(pin);
        }

        public async void FromAddressChangeClicked(string address)
        {
            try
            {
                if(string.IsNullOrEmpty(address))
                {
                    return;
                }    
                IsLoading = true;
                Geocoder geoCoder = new Geocoder();
                IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
                Position position = approximateLocations.FirstOrDefault();
                Pin pin = new Pin()
                {
                    Label = "Địa chỉ người gửi",
                    Position = position
                };
                FromLocations.Clear();
                FromLocations.Add(pin);
                if(Weight.Value != 0 || !double.IsNaN(Weight.Value))
                {
                    WeightChangeClicked(Weight.Value);
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                IsLoading = false;
            }

        }

        public async void ToAddressChangeClicked(string address)
        {
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    return;
                }
                IsLoading = true;
                Geocoder geoCoder = new Geocoder();
                IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(address);
                Position position = approximateLocations.FirstOrDefault();
                Pin pin = new Pin()
                {
                    Label = "Địa chỉ người nhận",
                    Position = position
                };
                ToLocations.Clear();
                ToLocations.Add(pin);
                if (Weight.Value != 0 || !double.IsNaN(Weight.Value))
                {
                    WeightChangeClicked(Weight.Value);
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                IsLoading = false;
            }

        }

        public async void WeightChangeClicked(double weight)
        {
            try
            {
                if (weight <= 0 || FromLocations.Count == 0 || ToLocations.Count == 0)
                {
                    return;
                }
                IsLoading = true;
                Position positionfrom = FromLocations[0].Position;
                Position positionto = ToLocations[0].Position;
                Distance distance = Distance.BetweenPositions(positionfrom, positionto);

                PriceList priceList = await RealtimeFirebase.Instance.GetPriceList(weight, distance.Kilometers);

                Money.Value = double.Parse(priceList.Price);
                IsLoading = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                IsLoading = false;
            }

        }
        #endregion
    }
}