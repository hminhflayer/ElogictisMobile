using ElogictisMobile.DataService;
using ElogictisMobile.Models;
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
    /// ViewModel for no item page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class DetailAgencyPageViewModel : BaseViewModel
    {
        #region Field
        public ValidatableObject<string> name;
        public ValidatableObject<string> address;
        private INavigationService _navigationService;
        
        public Category Province { get; set; }
        public District District { get; set; }
        public Town Town { get; set; }
        public Profiles profiles;
        public ObservableCollection<Profiles> ProfilesCollection { get; set; }

        #endregion
        #region Constructor 
        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public DetailAgencyPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            this.UpdateProductCommand = new Command(this.SubmitClicked);
            this.DeleteProductCommand = new Command(this.DeleteClicked);
            ProfilesCollection = LocalContext.ProfilesList;
        }

        #endregion

        #region Properties
        public Profiles Profiles
        {
            get
            {
                return this.profiles;
            }

            set
            {
                if (this.profiles == value)
                {
                    return;
                }

                this.SetProperty(ref this.profiles, value);
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
        public bool IsManage { get; set; } = LocalContext.IsManager;

        #endregion 

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command UpdateProductCommand { get; set; }
        public Command DeleteProductCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializzing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.Address = new ValidatableObject<string>();

            foreach (var item in LocalContext.ProfilesList)
            {
                if (item.Id == LocalContext.AgencySelected.ManagerId)
                {
                    this.profiles = item;
                    break;
                }
            }

            this.Name.Value = LocalContext.AgencySelected.Name;
            this.Address.Value = LocalContext.AgencySelected.Address;
            Category province = new Category
            {
                Id = LocalContext.AgencySelected.Province,
                Name = LocalContext.AgencySelected.Province_ext
            };
            District district = new District
            {
                Id = LocalContext.AgencySelected.District,
                Name = LocalContext.AgencySelected.District_ext,
                ProvinceId = LocalContext.AgencySelected.Province,
                ProvinceName = LocalContext.AgencySelected.Province_ext
            };

            Town town = new Town
            {
                Id = LocalContext.AgencySelected.Town,
                Name = LocalContext.AgencySelected.Town_ext,
                ProvinceId = LocalContext.AgencySelected.Province,
                ProvinceName = LocalContext.AgencySelected.Province_ext,
                DistrictId = LocalContext.AgencySelected.District,
                DistrictName = LocalContext.AgencySelected.District_ext,
                FullAddress = LocalContext.AgencySelected.Town_ext + ", " + LocalContext.AgencySelected.District_ext + ", " + LocalContext.AgencySelected.Province_ext
            };

            this.Province = province;
            this.District = district;
            this.Town = town;
        }

        /// <summary>
        /// Validation rule for name
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Tên đại lý không được trống" });
            this.Address.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Đại chỉ không được trống" });
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
                bool task1 = false, task2 = false;
                Agency temp = LocalContext.AgencySelected;
                temp.Name = Name.Value;
                temp.Address = Address.Value;
                temp.ManagerId = Profiles.Id;
                temp.ManagerName = Profiles.Name;
                temp.UpdateBy = LocalContext.Current.AccountSettings.Id;
                temp.UpdateTime = DateTime.Now.ToShortDateString();

                if(LocalContext.AgencySelected.ManagerId != Profiles.Id)
                {
                    Profiles profiles = new Profiles(); 
                    foreach(var item in ProfilesCollection)
                    {
                        if(item.Id == LocalContext.AgencySelected.ManagerId)
                        {
                            profiles = item;
                        }    
                    }

                    profiles.ManageAgency = "";
                    Profiles.ManageAgency = temp.Id;
                    task1 = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.AgencySelected.ManagerId, JsonConvert.SerializeObject(profiles));
                    task2 = await RealtimeFirebase.Instance.UpSert("Profiles", Profiles.Id, JsonConvert.SerializeObject(Profiles));
                }   
                
                var ups = await RealtimeFirebase.Instance.UpSert("Agencies", temp.Id, JsonConvert.SerializeObject(temp));
                if(ups && task1 && task2)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thành công!", "OK");
                    await _navigationService.GoBackAsync();
                }    
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật không thành công!", "OK");
                }
            }
        }

        /// <summary>
        /// Invoked when the Delete button clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void DeleteClicked(object obj)
        {
            // Do something
            var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa thông tin đơn hàng này?", "Đúng", "Không");
            if (action)
            {
                Products products = LocalContext.ProductSelected;
                products.IsDelete = true;
                products.LastUpdateBy = LocalContext.Profiles.Email;
                products.LastUpdateTime = DateTime.Now.ToString();

                // Do Something
                await RealtimeFirebase.Instance.UpSert("Products", products.ID, JsonConvert.SerializeObject(products));
                //await RealtimeFirebase.Instance.Delete("Products", products.ID);
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xóa thông tin đơn hàng thành công", "OK");
            }
        }

        #endregion
    }
}
