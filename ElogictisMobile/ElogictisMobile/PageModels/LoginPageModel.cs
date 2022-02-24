using System;
using ElogictisMobile.PageModels.Base;
using ElogictisMobile.Services.Account;
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


        public ButtonModel ForgotPasswordModel { get; set; }
        public ButtonModel LogInModel { get; set; }
        public ButtonModel UsePhoneModel { get; set; }

        private IAccountService _accountService;
        private INavigationService _navigationService;

        public LoginPageModel(INavigationService navigationService,
            IAccountService accountService)
        {
            _accountService = accountService;
            _navigationService = navigationService;

            //ForgotPasswordModel = new ButtonModel("forgot password", OnForgotPassword);
            LogInModel = new ButtonModel("LOG IN", OnLogin);
            //UsePhoneModel = new ButtonModel("USE PHONE NUMBER", GoToPhoneLogin);
        }

        private async void OnLogin()
        {
            await _navigationService.NavigateToAsync<DashboardPageModel>();
            //var loginAttempt = await _accountService.LoginAsync(EmailEntryViewModel.Text, PasswordEntryViewModel.Text);
            //if (loginAttempt)
            //{
            //    // navigate to the Dashboard.
                
            //}
            //else
            //{
            //    // TODO: Display an Alert for Failure!
            //}
        }

        private void OnForgotPassword()
        {

        }

        private void GoToPhoneLogin()
        {
            //_navigationService.NavigateToAsync<LoginPhonePageModel>();
        }
    }
}
