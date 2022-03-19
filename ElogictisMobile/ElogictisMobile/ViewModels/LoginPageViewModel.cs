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
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields

        private ValidatableObject<string> password;
        private IAccountService _accountService;
        private INavigationService _navigationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel(INavigationService navigationService,
            IAccountService accountService)
        {
            _accountService = accountService;
            _navigationService = navigationService;

            this.InitializeProperties();
            this.AddValidationRules();
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
            this.ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            this.SocialMediaLoginCommand = new Command(this.SocialLoggedIn);
        }

        #endregion

        #region property

        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public ValidatableObject<string> Password
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

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Command SocialMediaLoginCommand { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Check the password is null or empty
        /// </summary>
        /// <returns>Returns the fields are valid or not</returns>
        public bool AreFieldsValid()
        {
            bool isEmailValid = this.Email.Validate();
            bool isPasswordValid = this.Password.Validate();
            return isEmailValid && isPasswordValid;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Password = new ValidatableObject<string>();
        }

        /// <summary>
        /// Validation rule for password
        /// </summary>
        private void AddValidationRules()
        {
            this.Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Mật khẩu không được trống" });
        }

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            try
            {
                if (this.AreFieldsValid())
                {
                    // Do Something
                    var loginAttempt = await _accountService.LoginAsync(Email.Value, Password.Value);
                    if (loginAttempt == true)
                    {
                        if(_accountService.CheckEmailVerified())
                        {
                            StaticClass.Profiles = await RealtimeFirebase.Instance.GetProfiles(_accountService.GetUidLogin());
                            if (StaticClass.Profiles.Profile_Id == _accountService.GetUidLogin())
                            {
                                await _navigationService.NavigateToAsync<DashboardPageViewModel>();
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Thông báo", "Hệ thống không lấy được thông tin đăng nhập của bạn!", "OK");
                            }
                        }    
                        else
                        {
                            var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Tài khoản của bạn chưa được xác thực\nBạn muốn nhận lại một mail xác thực khác?!", "Đúng", "Không");

                            if (action)
                            {
                                await _accountService.SendEmailVerified();
                            }
                        }    
                            
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Lỗi đăng nhập", "Tên đăng nhập hoặc mật khẩu không đúng!", "OK");
                    }
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            }
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            // Do Something
            await _navigationService.NavigateToAsync<SignUpPageViewModel>();
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            // Do something
            await _navigationService.NavigateToAsync<ForgotPasswordPageViewModel>();
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SocialLoggedIn(object obj)
        {
            // Do something
        }

        #endregion
    }
}