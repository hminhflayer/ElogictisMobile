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
        public Profiles Profiles { get; set; }
        public ObservableCollection<Profiles> ProfilesCollection { get; set; } = RealtimeFirebase.Instance.GetAll<Profiles>("Profiles");

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

            var profiles = new Profiles()
            {
                Id = LocalContext.AgencySelected.ManagerId,
                Name = LocalContext.AgencySelected.ManagerName
            };

            Profiles = profiles;
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

            var profiles = new Profiles()
            {
                Id = LocalContext.AgencySelected.ManagerId,
                Name = LocalContext.AgencySelected.ManagerName
            };

            Profiles = profiles;
            this.Name.Value = LocalContext.AgencySelected.Name;
            this.Address.Value = LocalContext.AgencySelected.Address;
            this.Province.Id = LocalContext.AgencySelected.Province;
            this.Province.Name = LocalContext.AgencySelected.Province_ext;
            this.District.Id = LocalContext.AgencySelected.District;
            this.District.Name = LocalContext.AgencySelected.District_ext;
            this.Town.Id = LocalContext.AgencySelected.Id;
            this.Town.Name = LocalContext.AgencySelected.Id;
            this.Profiles.Id = LocalContext.AgencySelected.ManagerId;
            this.Profiles.Name = LocalContext.AgencySelected.ManagerName;
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
                Agency temp = LocalContext.AgencySelected;
                temp.Name = Name.Value;
                temp.Address = Address.Value;
                temp.ManagerId = Profiles.Id;
                temp.ManagerName = Profiles.Name;
                temp.UpdateBy = LocalContext.Current.AccountSettings.Id;
                temp.UpdateTime = DateTime.Now.ToShortDateString();

                var ups = await RealtimeFirebase.Instance.UpSert("Agencies", temp.Id, JsonConvert.SerializeObject(temp));
                if(ups)
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
