using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for add profile page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AddProvincePageViewModel : BaseViewModel
    {
        #region Constructor
        public ValidatableObject<string> id;
        public ValidatableObject<string> name;
        private INavigationService _navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public AddProvincePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            this.SubmitCommand = new Command(this.SubmitClicked);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the From Full Name from user.
        /// </summary>


        public ValidatableObject<string> Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.SetProperty(ref this.name, value);
            }
        }
        public ValidatableObject<string> Id
        {
            get
            {
                return this.id;
            }

            set
            {
                if (this.id == value)
                {
                    return;
                }

                this.SetProperty(ref this.id, value);
            }
        }

        #endregion 

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializzing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.Id = new ValidatableObject<string>();
        }

        /// <summary>
        /// Validation rule for name
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Tên tỉnh không được trống" });
            this.Id.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Mã tỉnh không được trống" });
        }

        /// <summary>
        /// Check name is valid or not
        /// </summary>
        /// <returns>Returns the fields are valid or not</returns>
        private bool AreFieldsValid()
        {
            bool isName = this.Name.Validate();
            bool isAddress = this.Id.Validate();

            return isName && isAddress;
        }

        /// <summary>
        /// Invoked when the Submit button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void SubmitClicked(object obj)
        {
            if (this.AreFieldsValid())
            {
                // Do Something
                IsLoading = true;
                var task = await RealtimeFirebase.Instance.UpSert("Categories/Province", Id.Value, JsonConvert.SerializeObject(new Category
                {
                    Id = Id.Value,
                    Name = Name.Value
                }));
                if (task)
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm thành công!", "OK");
                }
                else
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm không thành công!", "OK");
                }
            }
        }

        public void BindingShipMoney()
        {

        }

        #endregion
    }
}
