using ElogictisMobile.Models;
using ElogictisMobile.Services.Account;
using ElogictisMobile.Services.Navigation;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Fields

        private INavigationService _navigationService;
        private IAccountService _accountService;
        #endregion
        #region Properties
        public bool IsManage { get; set; } = LocalContext.IsAdmin? LocalContext.IsAdmin:LocalContext.IsManager? LocalContext.IsManager: false;
        public bool IsAdmin { get; set; } = LocalContext.IsAdmin;
        public bool IsShipper { get; set; } = LocalContext.IsShipper;
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPageViewModel" /> class
        /// </summary>
        public SettingsPageViewModel(IAccountService accountService,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            this.EditProfileCommand = new Command(this.EditProfileClicked);
            this.ChangePasswordCommand = new Command(this.ChangePasswordClicked);
            this.LinkAccountCommand = new Command(this.LinkAccountClicked);
            this.HelpCommand = new Command(this.HelpClicked);
            this.TermsCommand = new Command(this.TermsServiceClicked);
            this.PolicyCommand = new Command(this.PrivacyPolicyClicked);
            this.FAQCommand = new Command(this.FAQClicked);
            this.ManageProductsCommand = new Command(this.ManageProductsClicked);
            this.ManageProfilesCommand = new Command(this.ManageProfilesClicked);
            this.CategoryTypeProductCommand = new Command(this.CategoryTypeProdcutClicked);
            this.CategoryPriceListCommand = new Command(this.CategoryPriceListClicked);
            this.ProductNewCommand = new Command(this.ManageProductNewClicked);
            this.AgencyCommand = new Command(this.ManageAgencyClicked);
            this.ProvinceCommand = new Command(this.ManageProvinceClicked);
            this.DistrictCommand = new Command(this.ManageDistrictClicked);
            this.TownCommand = new Command(this.ManageTownClicked);
            this.LogoutCommand = new Command(this.LogoutClicked);
        }


        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command is executed when the edit profile option is clicked.
        /// </summary>
        public Command EditProfileCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the change password option is clicked.
        /// </summary>
        public Command ChangePasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the account link option is clicked.
        /// </summary>
        public Command LinkAccountCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the help option is clicked.
        /// </summary>
        public Command HelpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the terms of service option is clicked.
        /// </summary>
        public Command TermsCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the privacy policy option is clicked.
        /// </summary>
        public Command PolicyCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the FAQ option is clicked.
        /// </summary>
        public Command FAQCommand { get; set; }

        public Command ManageProductsCommand { get; set; }

        public Command ManageProfilesCommand { get; set; }

        public Command CategoryTypeProductCommand { get; set; }

        public Command CategoryPriceListCommand { get; set; }
        public Command ProductNewCommand { get; set; }
        public Command AgencyCommand { get; set; }
        public Command ProvinceCommand { get; set; }
        public Command DistrictCommand { get; set; }
        public Command TownCommand { get; set; }


        /// <summary>
        /// Gets or sets the command is executed when the logout is clicked.
        /// </summary>
        public Command LogoutCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the edit profile option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void EditProfileClicked(object obj)
        {
            try
            {
                await _navigationService.NavigateToAsync<UpdateProfilePageViewModel>();
                // Do something
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            }
        }

        /// <summary>
        /// Invoked when the change password clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void ChangePasswordClicked(object obj)
        {
            try
            {
                // Do something
                await _navigationService.NavigateToAsync<ResetPasswordPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            }
        }

        /// <summary>
        /// Invoked when the account link clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void LinkAccountClicked(object obj)
        {
            // Do something
            //try
            //{
            //    await _navigationService.NavigateToAsync<AddProductFormPageViewModel>();
            //}
            //catch (Exception ex)
            //{
            //    await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            //}
        }

        /// <summary>
        /// Invoked when the terms of service clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void TermsServiceClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<ManageProductsPageViewModel>(null, false);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "Đóng");
            }
        }

        /// <summary>
        /// Invoked when the privacy and policy clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void PrivacyPolicyClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the FAQ clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void FAQClicked(object obj)
        {
            // Do something
        }

        private async void ManageProductsClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<ManageProductsPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void ManageProfilesClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<ManageProfilesPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void CategoryTypeProdcutClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<CategoryTypeProductPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void CategoryPriceListClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<CategoryPriceListPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void ManageProductNewClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<ManageProductNewPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void ManageAgencyClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<ManageAgencyPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void ManageProvinceClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<ManageProvincePageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void ManageDistrictClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<ManageDistrictPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void ManageTownClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<ManageTownPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        /// <summary>
        /// Invoked when the help option is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void HelpClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the logout button is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void LogoutClicked(object obj)
        {
            // Do something
            try
            {
                if (_accountService.SignOut())
                {
                    LocalContext.Current.AccountSettings = null;
                    await _navigationService.NavigateToAsync<LoginPageViewModel>(null,true);
                }    
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Data + ex.Source + ex.Message + ex, "Đóng");
            }
        }

        #endregion
    }
}
