using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using Plugin.Media;
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
    public class UpdateProfilePageViewModel : BaseViewModel
    {
        #region Fields

        private ValidatableObject<string> fullName;

        private ValidatableObject<string> email;
        private ValidatableObject<string> address;
        private Category province;
        private District district;
        private Town town;
        private string avatar;

        private string phoneNumber;

        private Command<object> updateProfileCommand;

        private Command<object> addProfileCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializing the profile details.
        /// </summary>
        public UpdateProfilePageViewModel()
        {
            this.InitializeProperties();
            this.AddValidationRules();
            ProvinceCollection = new ObservableCollection<Category>();
            DistrictCollection = new ObservableCollection<District>();
            TownCollection = new ObservableCollection<Town>();

            ProvinceCollection = LocalContext.ProvinceList;
            Province = new Category();
            District = new District();
            Town = new Town();
            this.ProvinceCommand = new Command(this.ProvinceChangeClicked);
            this.DistrictCommand = new Command(this.DistrictChangeClicked);
            this.TownCommand = new Command(this.TownChangeClicked);
        }

        #endregion

        #region Property

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
        public District District
        {
            get
            {
                return this.district;
            }

            set
            {
                if (this.district == value)
                {
                    return;
                }

                this.SetProperty(ref this.district, value);
            }
        }
        public Town Town
        {
            get
            {
                return this.town;
            }

            set
            {
                if (this.town == value)
                {
                    return;
                }

                this.SetProperty(ref this.town, value);
            }
        }
        public ObservableCollection<Category> ProvinceCollection { get; set; }
        public ObservableCollection<District> DistrictCollection { get; set; }
        public ObservableCollection<Town> TownCollection { get; set; }
        public List<District> DistrictColl { get; set; }
        public List<Town> TownColl { get; set; }

        public string Avatar
        {
            get
            {
                return this.avatar;
            }

            set
            {
                if (this.avatar == value)
                {
                    return;
                }

                this.SetProperty(ref this.avatar, value);
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
        public ValidatableObject<string> FullName
        {
            get
            {
                return this.fullName;
            }

            set
            {
                if (this.fullName == value)
                {
                    return;
                }

                this.SetProperty(ref this.fullName, value);
            }
        }

        /// <summary>
        /// Gets or sets the LastName.
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

        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }

            set
            {
                if (this.phoneNumber == value)
                {
                    return;
                }

                this.SetProperty(ref this.phoneNumber, value);
            }
        }

        #endregion

        #region Command

        public Command ProvinceCommand { get; set; }
        public Command DistrictCommand { get; set; }
        public Command TownCommand { get; set; }
        public Command<object> AddProfileCommand
        {
            get
            {
                return this.addProfileCommand ?? (this.addProfileCommand = new Command<object>(this.AddProfileClicked));
            }
        }

        /// <summary>
        /// Gets the command that will be executed when an add contact button is clicked.
        /// </summary>
        public Command<object> UpdateProfileCommand
        {
            get
            {
                return this.updateProfileCommand ?? (this.updateProfileCommand = new Command<object>(this.UpdateProfileClicked));
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Check the names are null or empty.
        /// </summary>
        /// <returns>Returns the names are valid or not</returns>
        public bool AreNamesValid()
        {
            bool isFirstNameValid = this.FullName.Validate();
            bool isLastNameValid = this.Email.Validate();
            return isFirstNameValid && isLastNameValid;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.FullName = new ValidatableObject<string>();
            this.Email = new ValidatableObject<string>();
            this.Address = new ValidatableObject<string>();

            this.FullName.Value = LocalContext.Current.AccountSettings.Name;
            this.Email.Value = LocalContext.Current.AccountSettings.Email;
            this.PhoneNumber = LocalContext.Current.AccountSettings.Phone;
            this.Avatar = LocalContext.Current.AccountSettings.Avatar;
            this.Address.Value = LocalContext.Current.AccountSettings.Address;

            this.Province = new Category()
            {
                Id = LocalContext.Current.AccountSettings.Province,
                Name = LocalContext.Current.AccountSettings.Province_ext
            };

            this.District = new District()
            {
                Id = LocalContext.Current.AccountSettings.District,
                Name = LocalContext.Current.AccountSettings.District_ext,
                ProvinceId = LocalContext.Current.AccountSettings.Province,
                ProvinceName = LocalContext.Current.AccountSettings.Province_ext
            };

            this.Town = new Town()
            {
                Id = LocalContext.Current.AccountSettings.Town,
                Name = LocalContext.Current.AccountSettings.Town_ext,
                DistrictId = LocalContext.Current.AccountSettings.District,
                DistrictName = LocalContext.Current.AccountSettings.District_ext,
                ProvinceId = LocalContext.Current.AccountSettings.Province,
                ProvinceName = LocalContext.Current.AccountSettings.Province_ext,
                FullAddress = LocalContext.Current.AccountSettings.Address
            };
        }

        /// <summary>
        /// Validation rules for name fields
        /// </summary>
        private void AddValidationRules()
        {
            this.FullName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Họ và tên không được trống" });
            this.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email không được trống" });
        }

        /// <summary>
        /// Invoked when add contact button is clicked from the add profile page.
        /// </summary>
        /// <param name="obj">Selected item from the list view.</param>
        private async void UpdateProfileClicked(object obj)
        {
            if (this.AreNamesValid())
            {
                IsLoading = true;
                Profiles profiles = LocalContext.Current.AccountSettings;
                profiles.Name = FullName.Value;
                profiles.LastUpdateBy = LocalContext.Profiles.Email;
                profiles.LastUpdateTime = DateTime.Now.ToString();
                profiles.Phone = PhoneNumber;
                profiles.Province = Province.Id;
                profiles.Province_ext = Province.Name;
                profiles.District = District.Id;
                profiles.District_ext = District.Name;
                profiles.Town = Town.Id;
                profiles.Town_ext = Town.Name;
                profiles.Address = Address.Value;
                profiles.Avatar = Avatar;

                // Do Something
                var upsert = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.Profiles.Id, JsonConvert.SerializeObject(profiles));
                if(upsert)
                {
                    IsLoading = false;
                    LocalContext.Current.AccountSettings = profiles;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Đã cập nhật thông tin tài khoản thành công", "OK");
                }    
                else
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo lỗi", "Cập nhật thông tin không thành công", "OK");
                }
            }
        }

        /// <summary>
        /// Invoked when add profile button is clicked from the add profile page.
        /// </summary>
        /// <param name="obj">Selected item from the list view.</param>
        private async void AddProfileClicked(object obj)
        {
            // Do something
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;
            var filepath = file.Path;

            Avatar = ImageSource.FromStream(() => file.GetStream()).ToString();
        }

        public async void ProvinceChangeClicked()
        {
            try
            {
                if(Province == null)
                {
                    return;
                }    
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
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }

        }
        public async void DistrictChangeClicked()
        {
            try
            {
                if(District == null)
                {
                    return;
                }    
                List<Town> towns = new List<Town>();
                var district = District.Id;

                Town = new Town();

                TownCollection.Clear();

                towns = await RealtimeFirebase.Instance.GetListTown(district);
                foreach (var x in towns)
                {
                    TownCollection.Add(x);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }

        }
        public async void TownChangeClicked()
        {
            try
            {
                if(Town == null)
                {
                    return;
                }    
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
