using System;
using ElogictisMobile.PageModels.Base;
using ElogictisMobile.Services.Account;
using ElogictisMobile.Services.Firebase;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.ViewModels;
using ElogictisMobile.ViewModels.Buttons;
using Xamarin.Forms;

namespace ElogictisMobile.PageModels
{
    public class LoginPageModel : PageModelBase
    {
        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private string _username;
        private string _password;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }


        public ButtonModel ForgotPasswordModel { get; set; }
        public ButtonModel LogInModel { get; set; }
        public ButtonModel UsePhoneModel { get; set; }

        public ButtonModel SignUpModel { get; set; }
        public ButtonModel ResetPasswordModel { get; set; }

        private IAccountService _accountService;
        private INavigationService _navigationService;

        public LoginPageModel(INavigationService navigationService,
            IAccountService accountService)
        {
            _accountService = accountService;
            _navigationService = navigationService;

            //ForgotPasswordModel = new ButtonModel("forgot password", OnForgotPassword);
            LogInModel = new ButtonModel("LOG IN", OnLogin);
            SignUpModel = new ButtonModel("Sign Up", OnSignUp);
            ResetPasswordModel = new ButtonModel("Quên mật khẩu", OnForgotPassword);
            //UsePhoneModel = new ButtonModel("USE PHONE NUMBER", GoToPhoneLogin);
        }

        private async void OnSignUp()
        {
            await _navigationService.NavigateToAsync<SignUpPageModel>();
        }

        private async void OnForgotPassword()
        {
            await _navigationService.NavigateToAsync<ForgotPasswordPageModel>();
        }

        private async void OnLogin()
        {
            var loginAttempt = await _accountService.LoginAsync(Username, Password);
            if (loginAttempt)
            {
                await _navigationService.NavigateToAsync<DashboardPageModel>();
            }
            else
            {
                await _navigationService.NavigateToAsync<SignUpPageModel>();
            }
        }


        //await _navigationService.NavigateToAsync<DashboardPageModel>();
        //var loginAttempt = await _accountService.LoginAsync(EmailEntryViewModel.Text, PasswordEntryViewModel.Text);
        //if (loginAttempt)
        //{
        //    // navigate to the Dashboard.

        //}
        //else
        //{
        //    // TODO: Display an Alert for Failure!
        //}
    //}


        private void GoToPhoneLogin()
        {
            //_navigationService.NavigateToAsync<LoginPhonePageModel>();
        }
    }
}
