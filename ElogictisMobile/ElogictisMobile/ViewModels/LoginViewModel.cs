using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginViewModel : BaseViewModel
    {
        #region Fields
        private string _passwordSignUp;
        private string _phoneNumberSignUp;
        private string _fullNameSignUp;
        private ValidatableObject<string> email;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginViewModel" /> class.
        /// </summary>
        public LoginViewModel()
        {
            this.InitializeProperties();
            this.AddValidationRules();
            IsLoading = false;
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the email ID from user in the login page.
        /// </summary>
        public ValidatableObject<string> Email
        {
            get
            {
                return this.email;
            }

            set
            {
                if (this.email == value)
                {
                    return;
                }

                this.SetProperty(ref this.email, value);
            }
        }

        public string PasswordSignUp
        {
            get => this._passwordSignUp;
            set => SetProperty(ref _passwordSignUp, value);
        }
        public string FullNameSignUp
        {
            get => this._fullNameSignUp;
            set => SetProperty(ref _fullNameSignUp, value);
        }
        public string PhoneNumberSignUp
        {
            get => this._phoneNumberSignUp;
            set => SetProperty(ref _phoneNumberSignUp, value);
        }
        #endregion

        #region Methods

        /// <summary>
        /// This method to validate the email
        /// </summary>
        /// <returns>returns bool value</returns>
        public bool IsEmailFieldValid()
        {
            bool isEmailValid = this.Email.Validate();
            return isEmailValid;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Email = new ValidatableObject<string>();
        }

        /// <summary>
        /// This method contains the validation rules
        /// </summary>
        private void AddValidationRules()
        {
            this.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email không được trống" });
            this.Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Email không hợp lệ" });
        }

        #endregion
    }
}
