using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for add profile page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AddTownPageViewModel : BaseViewModel
    {
        #region Constructor
        public ValidatableObject<string> id;
        public ValidatableObject<string> name;
        private INavigationService _navigationService;
        public List<District> districts = LocalContext.Districts;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public AddTownPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            DistrictCollection = new ObservableCollection<District>();  
            
            this.SubmitCommand = new Command(this.SubmitClicked);
            this.DeleteCommand = new Command(this.DeleteClicked);
            this.ProvinceCommand = new Command(this.ProvinceChangeClicked);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the From Full Name from user.
        /// </summary>

        public bool IsEdit { get; set; } = LocalContext.IsEdit;
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
        public Category Province { get; set; }
        public District District { get; set; }
        public ObservableCollection<Category> ProvinceCollection { get; set; } = LocalContext.ProvinceList;
        public ObservableCollection<District> DistrictCollection { get; set; }
        #endregion 

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command ProvinceCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializzing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.Id = new ValidatableObject<string>();

            if(LocalContext.IsEdit)
            {
                Id.Value = LocalContext.TownSelected.Id;
                Name.Value = LocalContext.TownSelected.Name;
                District district = new District()
                {
                    Id = LocalContext.TownSelected.DistrictId,
                    Name = LocalContext.TownSelected.DistrictName,
                    ProvinceId = LocalContext.TownSelected.ProvinceId,
                    ProvinceName = LocalContext.TownSelected.ProvinceName
                };
                District = district;
                Category provi = new Category()
                {
                    Id = LocalContext.TownSelected.ProvinceId,
                    Name = LocalContext.TownSelected.ProvinceName
                };
                Province = provi;
            }    
        }

        /// <summary>
        /// Validation rule for name
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Tên Quận/Huyện không được trống" });
            this.Id.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Mã Quận/Huyện không được trống" });
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
                string mess;
                var town = new Town();
                if (IsEdit)
                {
                    town = LocalContext.TownSelected;
                    town.Name = Name.Value;
                    town.Id = Id.Value;
                    town.DistrictName = District.Name;
                    town.DistrictName = District.Id;
                    town.ProvinceName = Province.Name;
                    town.ProvinceId = Province.Id;
                    town.FullAddress = Name.Value + ", " + District.Name + ", " + Province.Name;
                    mess = "Cập nhật thành công!";
                }    
                else
                {
                    town.Name = Name.Value;
                    town.Id = Id.Value;
                    town.DistrictName = District.Name;
                    town.DistrictName = District.Id;
                    town.ProvinceName = Province.Name;
                    town.ProvinceId = Province.Id;
                    town.FullAddress = Name.Value + ", " + District.Name + ", " + Province.Name;
                    mess = "Thêm thành công!";
                }    
                

                var task = await RealtimeFirebase.Instance.UpSert("Categories/Town/"+ District.Id, Id.Value, JsonConvert.SerializeObject(town));
                if (task)
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", mess, "OK");
                }
                else
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm/Cập nhật không thành công!", "OK");
                }
            }
        }

        private async void DeleteClicked(object obj)
        {
            // Do something
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa tỉnh "+ Name.Value +" ?", "Đúng", "Không");
                if (action)
                {
                    var del = await RealtimeFirebase.Instance.Delete("Categories/Town/"+District.Id, Id.Value);
                    if (del)
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xóa thành công", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Có lỗi khi xóa", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        public async void ProvinceChangeClicked()
        {
            try
            {
                if(LocalContext.Districts != null)
                {
                    return;
                }    
                IsLoading = true;
                districts = new List<District>();
                var province = Province.Id;

                DistrictCollection.Clear();

                districts = await RealtimeFirebase.Instance.GetListDistrict(province);
                foreach (var x in districts)
                {
                    DistrictCollection.Add(x);
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                IsLoading = false;
            }

        }

        #endregion
    }
}
