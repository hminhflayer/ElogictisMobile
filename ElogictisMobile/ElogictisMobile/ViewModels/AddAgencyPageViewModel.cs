using ElogictisMobile.DataService;
using ElogictisMobile.Models;
using ElogictisMobile.Models.About;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel of AboutUs templates.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AddAgencyPageViewModel : BaseViewModel
    {
        #region Constructor
        public ValidatableObject<string> name;
        public ValidatableObject<string> address;
        private INavigationService _navigationService;
        private Category province;

        public District District { get; set; }
        public Town Town { get; set; }
        public Profiles Profiles { get; set; }
        public ObservableCollection<Profiles> ProfilesCollection { get; set; } = RealtimeFirebase.Instance.GetAll<Profiles>("Profiles");
        public ObservableCollection<Category> ProvinceCollection { get; set; } = LocalContext.ProvinceList;
        public ObservableCollection<District> DistrictCollection { get; set; } = RealtimeFirebase.Instance.GetListDistrict("89");
        public ObservableCollection<Town> TownCollection { get; set; } = RealtimeFirebase.Instance.GetListTown("883");


        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public AddAgencyPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            this.SubmitCommand = new Command(this.SubmitClicked);
            this.ProvinceCommand = new Command(this.ProvinceChangeClicked);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the From Full Name from user.
        /// </summary>

        public Category Province
        {
            get
            {
                return this.province;
            }

            set
            {
                if (this.province == value)
                {
                    return;
                }

                this.SetProperty(ref this.province, value);
            }
        }
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
        public ValidatableObject<string> Address
        {
            get
            {
                return this.address;
            }

            set
            {
                if (this.address == value)
                {
                    return;
                }

                this.SetProperty(ref this.address, value);
            }
        }

        #endregion 

        #region Commants

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }
        public Command ProvinceCommand { get; set; }
        public Command DistrictCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializzing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.Address = new ValidatableObject<string>();
        }

        /// <summary>
        /// Validation rule for name
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Tên đại lý không được trống" });
            this.Address.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Địa chỉ không được trống" });
        }

        /// <summary>
        /// Check name is valid or not
        /// </summary>
        /// <returns>Returns the fields are valid or not</returns>
        private bool AreFieldsValid()
        {
            bool isName = this.Name.Validate();
            bool isAddress = this.Address.Validate();

            return isName && isAddress;
        }

        /// <summary>
        /// Invoked when the Submit button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void SubmitClicked(object obj)
        {
            if (LocalContext.Current.AccountSettings.Money == 0)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn không còn tiền trong tài khoản!", "OK");
                return;
            }
            if (this.AreFieldsValid())
            {
                var key = GeneralKey.Instance.General("AGENCY");
                // Do Something
                var task = await RealtimeFirebase.Instance.UpSert("Agencies", key, JsonConvert.SerializeObject(new Agency
                {
                    CreateBy = LocalContext.Current.AccountSettings.Id,
                    CreateTime = DateTime.Now.ToShortDateString(),
                    Address = Address.Value,
                    Name = Name.Value,
                    Province = Province.Id,
                    Province_ext = Province.Name,
                    District = District.Id,
                    District_ext = District.Name,
                    Town = Town.Id,
                    Town_ext = Town.Name,
                    Id = key,
                    UpdateBy = "",
                    UpdateTime = "",
                    ManagerId = Profiles.Id,
                    ManagerName = Profiles.Name
                }));
                if (task)
                {
                    
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm thành công!", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm không thành công!", "OK");
                }
            }
        }

        public void ProvinceChangeClicked()
        {

        }

        #endregion
    }
}