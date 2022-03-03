using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ElogictisMobile.PageModels.Base;
using ElogictisMobile.Services.Navigation;
using Xamarin.Forms;

namespace ElogictisMobile.PageModels
{
    public class ProfilePageModel : PageModelBase
    {
        private ICommand _profileInfo_Tapped;
        private ICommand _signOut_Tapped;
        private ICommand _resetPassword_Tapped;
        private INavigationService _navigationService;

        public ICommand ProfileInfo_Tapped
        {
            get => _profileInfo_Tapped;
        }
        public ICommand SignOut_Tapped
        {
            get => _signOut_Tapped;
        }

        public ICommand ResetPassword_Tapped
        {
            get => _resetPassword_Tapped;
        }

        public ProfilePageModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _profileInfo_Tapped = new Command(GoToProfileInfoPage);
            _signOut_Tapped = new Command(SignOut);
            _resetPassword_Tapped = new Command(ResetPassword);

        }
        public async void GoToProfileInfoPage(object sender)
        {
            await _navigationService.NavigateToAsync<ProfileInfoPageModel>();
        }

        public async void SignOut(object sender)
        {
            await _navigationService.NavigateToAsync<LoginPageModel>();
        }

        public async void ResetPassword(object sender)
        {
            await _navigationService.NavigateToAsync<ResetPasswordPageModel>();
        }
    }
}
