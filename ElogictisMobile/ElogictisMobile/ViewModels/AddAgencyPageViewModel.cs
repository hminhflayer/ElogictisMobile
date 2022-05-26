using ElogictisMobile.DataService;
using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public Category Province { get; set; }
        public District District { get; set; }
        public Town Town { get; set; }
        public Profiles Profiles { get; set; }
        public ObservableCollection<Profiles> ProfilesCollection { get; set; } = RealtimeFirebase.Instance.GetAll<Profiles>("Profiles");
        public ObservableCollection<Category> ProvinceCollection { get; set; } = LocalContext.ProvinceList;
        public ObservableCollection<District> DistrictCollection { get; set; }
        public ObservableCollection<Town> TownCollection { get; set; }
        public List<District> DistrictColl { get; set; }
        public List<Town> TownColl { get; set; }


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
            this.DistrictCommand = new Command(this.DistrictChangeClicked);
            this.TownCommand = new Command(this.TownChangeClicked);
            DistrictCollection = new ObservableCollection<District>();
            TownCollection = new ObservableCollection<Town>();
        }

        #endregion

        #region Properties
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
        public Command TownCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializzing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.Address = new ValidatableObject<string>();
            if(ProvinceCollection.Count > 0)
            {
                Province = new Category()
                {
                    Id = "89",
                    Name = "An Giang"
                };
            }    
            
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
            if (this.AreFieldsValid())
            {
                var key = "AGENCY" + Province.Id + District.Id;
                // Do Something
                var task = await RealtimeFirebase.Instance.UpSert("Agencies", key, JsonConvert.SerializeObject(new Agency
                {
                    CreateBy = LocalContext.Current.AccountSettings.Id,
                    CreateTime = DateTime.Now.ToShortDateString(),
                    Address = Address.Value + ", " + Town.Name + ", " + District.Name + ", " + Province.Name,
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

                Profiles.ManageAgency = key;
                var task1 = await RealtimeFirebase.Instance.UpSert("Profiles", Profiles.Id, JsonConvert.SerializeObject(Profiles));
                if (task && task1)
                {
                    
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm thành công!", "OK");
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm không thành công!", "OK");
                }
            }
        }
        public async void ProvinceChangeClicked()
        {
            try
            {
                List<District> districts = new List<District>();
                var province = Province.Id;

                District = new District();
                Town = new Town();

                DistrictCollection.Clear();
                TownCollection.Clear();

                districts = await RealtimeFirebase.Instance.GetListDistrict(province);
                foreach (var x in districts)
                {
                    DistrictCollection.Add(x);
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            
        }
        public async void DistrictChangeClicked()
        {
            try
            {
                List<Town> towns = new List<Town>();
                if(District == null)
                {
                    return;
                }    
                var district = District.Id;

                Town = new Town();

                TownCollection.Clear();

                towns = await RealtimeFirebase.Instance.GetListTown(district);
                foreach (var x in towns)
                {
                    TownCollection.Add(x);
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            
        }
        public async void TownChangeClicked()
        {
            try
            {
                Address.Value = Town.FullAddress;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }

        }
        #endregion
    }
}