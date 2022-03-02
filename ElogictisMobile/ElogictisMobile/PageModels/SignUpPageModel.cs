using ElogictisMobile.PageModels.Base;
using ElogictisMobile.Services.Account;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.ViewModels.Buttons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElogictisMobile.PageModels
{
    public class SignUpPageModel : PageModelBase
    {
        private IAccountService _accountService;
        private INavigationService _navigationService;
        private string _fullName;
        private string _email;
        private string _password;
        private string _verifyPassword;

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string VerifyPassword
        {
            get => _verifyPassword;
            set => SetProperty(ref _verifyPassword, value);
        }
        public ButtonModel SignUpModel { get; }

        public SignUpPageModel(INavigationService navigationService,
            IAccountService accountService)
        {
            _accountService = accountService;
            _navigationService = navigationService;

            SignUpModel = new ButtonModel("Đăng Ký", SignUpHandle);
        }

        private async void SignUpHandle()
        {
            var loginAttempt = await _accountService.SignUpAsync(Email, Password);
            if (loginAttempt != "")
            {
                await _navigationService.NavigateToAsync<LoginPageModel>();
            }
        }
    }
}
