using ElogictisMobile.Services.Account;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ElogictisMobile.ViewModels
{
    public class VerifyEmailPageViewModel : LoginViewModel
    {
        #region Fields

        private IAccountService _accountService;
        private INavigationService _navigationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordPageViewModel" /> class.
        /// </summary>
        public VerifyEmailPageViewModel(INavigationService navigationService,
            IAccountService accountService)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            this.SendCommand = new Command(this.SendClicked);
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Send button is clicked.
        /// </summary>
        public Command SendCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Send button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SendClicked(object obj)
        {
            try
            {
                if (this.IsEmailFieldValid())
                {
                    // Do something
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            }
        }

        #endregion
    }
}
