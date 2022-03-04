using ElogictisMobile.Services.Account;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ElogictisMobile.ViewModels
{
    internal class AlertSignUpPageViewModel : BaseViewModel
    {
        private IAccountService _accountService;
        private INavigationService _navigationService;

        public AlertSignUpPageViewModel(INavigationService navigationService,
            IAccountService accountService)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            this.LoginCommand = new Command(this.LoginClicked);
        }

        public Command LoginCommand { get; set; }

        private async void LoginClicked(object obj)
        {
            try
            {
                await _navigationService.NavigateToAsync<LoginPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            }
        }
    }
}
