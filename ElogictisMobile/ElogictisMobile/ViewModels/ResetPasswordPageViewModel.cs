using ElogictisMobile.Services.Account;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using ElogictisMobile.ViewModels.Base;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for reset password page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ResetPasswordPageViewModel : BaseViewModel
    {
        #region Fields

        private ValidatablePair<string> password;
        private INavigationService _navigationService;
        private IAccountService _accountService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordPageViewModel" /> class.
        /// </summary>
        public ResetPasswordPageViewModel(INavigationService navigationService,
            IAccountService accountService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            this.InitializeProperties();
            this.AddValidationRules();
            this.SubmitCommand = new Command(this.SubmitClicked);
        }
        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }
        #endregion

        #region Public property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the new password from user in the reset password page.
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

        #region Methods

        /// <summary>
        /// Initialize whether fieldsvalue are true or false.
        /// </summary>
        /// <returns>true or false </returns>
        public bool AreFieldsValid()
        {
            bool isPassword = this.Password.Validate();
            bool isVerifyPassword = false;
            if (isPassword)
            {
                isVerifyPassword = this.Password.Item1.Value.Equals(this.Password.Item2.Value);
            }
            return isPassword;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Password = new ValidatablePair<string>();
        }

        /// <summary>
        /// Validation rule for password
        /// </summary>
        private void AddValidationRules()
        {
            this.Password.Item1.Validations.Add(new IsNotLength<string> { ValidationMessage = "Mật khẩu phải chứa ít nhất một chữ số, một ký tự viết hoa và ít nhất 8 kí tự" });
            this.Password.Item2.Validations.Add(new IsNotLength<string> { ValidationMessage = "Nhập lại mật khẩu phải chứa ít nhất một chữ số, một ký tự viết hoa và ít nhất 8 kí tự" });
        }

        /// <summary>
        /// Invoked when the Submit button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SubmitClicked(object obj)
        {
            try
            {
                if (this.AreFieldsValid())
                {
                    // Do something
                    var change = await _accountService.ChangePasswordAsync(Password.Item2.Value);
                    if (change)
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đổi mật khẩu thành công!", "Đóng");
                        await _navigationService.NavigateToAsync<DashboardPageViewModel>();
                    }    
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