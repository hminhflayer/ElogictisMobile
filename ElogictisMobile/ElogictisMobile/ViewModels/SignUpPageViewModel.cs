using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Account;
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
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : LoginViewModel
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
        public SignUpPageViewModel(IAccountService accountService,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _accountService = accountService;

            this.InitializeProperties();
            this.AddValidationRules();
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
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
        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }
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
        /// Invoked when the Log in button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            // Do something
            await _navigationService.NavigateToAsync<LoginPageViewModel>();
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
                    var loginAttempt = await _accountService.SignUpAsync(Email.Value, Password.Item1.Value);
                    if (loginAttempt != "")
                    {
                        Profiles profiles = new Profiles()
                        {
                            Profile_CreateBy = Email.Value,
                            Profile_CreateTime = DateTime.Now.ToShortDateString(),
                            Profile_Email = Email.Value,
                            Profile_Name = Name.Value,
                            Profile_Id = loginAttempt,
                            Profile_IsDelete = false,
                            Profile_Address = "",
                            Profile_LastUpdateBy = "",
                            Profile_LastUpdateTime = "",
                            Profile_Phone = "",
                            Profile_Auth = 1,
                            Profile_Identity = ""
                        };
                        await RealtimeFirebase.Instance.UpSert("Profiles", loginAttempt, JsonConvert.SerializeObject(profiles)); 
                        await _navigationService.NavigateToAsync<AlertSignUpPageViewModel>();
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
                if(ex.Message.Contains("already"))
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Địa chỉ email đã được sử dụng", "Đóng");
                }   
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
                } 
            }
        }

        #endregion
    }
}