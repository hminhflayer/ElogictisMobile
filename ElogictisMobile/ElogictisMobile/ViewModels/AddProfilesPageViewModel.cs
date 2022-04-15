using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Account;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for Business Registration Form page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class AddProfilesPageViewModel : LoginViewModel
    {
        #region Fields

        private ValidatableObject<string> name;

        private ValidatablePair<string> password;
        private INavigationService _navigationService;
        private IAccountService _accountService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public AddProfilesPageViewModel(IAccountService accountService,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _accountService = accountService;

            this.InitializeProperties();
            this.AddValidationRules();
            this.SignUpCommand = new Command(this.SignUpClicked);
            this.ProvinceCommand = new Command(this.ProvinceChangeClicked);
            this.DistrictCommand = new Command(this.DistrictChangeClicked);
            this.TownCommand = new Command(this.TownChangeClicked);
            DistrictCollection = new ObservableCollection<District>();
            TownCollection = new ObservableCollection<Town>();
        }
        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
        public ValidatableObject<string> Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.SetProperty(ref this.name, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password from users in the Sign Up page.
        /// </summary>
        public ValidatablePair<string> Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.SetProperty(ref this.password, value);
            }
        }
        public ValidatableObject<string> Address { get; set; }
        public ValidatableObject<string> PhoneNumber { get; set; }
        public Category Province { get; set; }
        public District District { get; set; }
        public Town Town { get; set; }
        public ObservableCollection<Category> ProvinceCollection { get; set; } = LocalContext.ProvinceList;
        public ObservableCollection<District> DistrictCollection { get; set; }
        public ObservableCollection<Town> TownCollection { get; set; }
        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }
        public Command ProvinceCommand { get; }
        public Command DistrictCommand { get; }
        public Command TownCommand { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Initialize whether fieldsvalue are true or false.
        /// </summary>
        /// <returns>true or false </returns>
        public bool AreFieldsValid()
        {
            bool isEmail = this.Email.Validate();
            bool isNameValid = this.Name.Validate();
            bool isPasswordValid = this.Password.Validate();
            bool isVerifyPassword = false;
            if (isPasswordValid)
            {
                isVerifyPassword = this.Password.Item1.Value.Equals(this.Password.Item2.Value);
            }
            return isPasswordValid && isNameValid && isEmail && isVerifyPassword;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.Password = new ValidatablePair<string>();
            this.PhoneNumber = new ValidatableObject<string>();
            this.Address = new ValidatableObject<string>();
        }

        /// <summary>
        /// this method contains the validation rules
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Họ và tên không được trống" });
            this.Password.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Mật khẩu không được trống" });
            this.Password.Item1.Validations.Add(new IsNotLength<string> { ValidationMessage = "Mật khẩu phải chứa ít nhất một chữ số, một ký tự viết hoa và ít nhất 8 kí tự" });
            this.Password.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Nhập lại mật khẩu không được trống" });
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            try
            {
                if (this.AreFieldsValid())
                {
                    IsLoading = true;
                    var loginAttempt = await _accountService.SignUpAsync(Email.Value, Password.Item1.Value);
                    if (loginAttempt != "")
                    {
                        Profiles profiles = new Profiles()
                        {
                            CreateBy = Email.Value,
                            CreateTime = DateTime.Now.ToShortDateString(),
                            Email = Email.Value,
                            Name = Name.Value,
                            Id = loginAttempt,
                            IsDelete = false,
                            Address = Address.Value,
                            LastUpdateBy = "",
                            LastUpdateTime = "",
                            Phone = PhoneNumber.Value,
                            Auth = "1",
                            Auth_ext = "Người dùng bình thường",
                            Identity = "",
                            Money = 0,
                            IsConfirm = false,
                            Province = Province.Id,
                            Province_ext = Province.Name,
                            District = District.Id,
                            District_ext = District.Name,
                            Town = Town.Id,
                            Town_ext = Town.Name,
                            AgencyId = "AGENCY" + Province.Id + District.Id,
                        };
                        var upsert = await RealtimeFirebase.Instance.UpSert("Profiles", loginAttempt, JsonConvert.SerializeObject(profiles));
                        if (upsert)
                        {
                            IsLoading = false;
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm thành công", "Đóng");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Có lỗi xảy ra", "Đóng");
                        }
                        await _navigationService.GoBackAsync();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đăng ký không thành công!", "Đóng");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thông tin bạn đăng ký không hợp lệ!", "Đóng");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already"))
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Địa chỉ email đã được sử dụng", "Đóng");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
                }
            }
        }

        public async void ProvinceChangeClicked()
        {
            try
            {
                List<District> districts = new List<District>();
                var province = Province.Id;

                District = null;
                Town = null;

                DistrictCollection.Clear();
                TownCollection.Clear();

                districts = await RealtimeFirebase.Instance.GetListDistrict(province);
                foreach (var x in districts)
                {
                    DistrictCollection.Add(x);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }

        }
        public async void DistrictChangeClicked()
        {
            try
            {
                List<Town> towns = new List<Town>();
                var district = District.Id;

                Town = new Town();

                TownCollection.Clear();

                towns = await RealtimeFirebase.Instance.GetListTown(district);
                foreach (var x in towns)
                {
                    TownCollection.Add(x);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }

        }
        public async void TownChangeClicked()
        {
            try
            {
                if (Town.FullAddress != null)
                {
                    Address.Value = Town.FullAddress;
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }

        }
        #endregion
    }
}