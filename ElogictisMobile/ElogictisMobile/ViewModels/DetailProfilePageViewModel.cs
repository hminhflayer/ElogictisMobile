using ElogictisMobile.DataService;
using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for Business Registration Form page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class DetailProfilePageViewModel : LoginViewModel
    {
        #region Fields
        private Profiles Profiles { get; set; }
        public bool IsVisible { get; set; }
        public bool IsManager { get; set; }
        public bool IsAdmin { get; set; }
        public string Money { get; set; }
        public bool IsConfirm { get; set; } = !LocalContext.ProfileSelected.IsConfirm;

        private ValidatableObject<string> fullName;

        private ValidatableObject<string> email;

        private string phoneNumber;

        private bool isDelete;

        private Command<object> updateProfileCommand;

        private Command<object> addProfileCommand;

        private Command<object> deleteProfileCommand;
        private Command<object> manageMoneyCommand;
        private Command<object> confirmCommand;
        private INavigationService _navigationService;
       

        #endregion

        #region Constructor

        /// <summary>
        /// Initializing the profile details.
        /// </summary>
        public DetailProfilePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            IsAdmin = true;

            this.InitializeProperties();
            this.AddValidationRules();

            if (LocalContext.IsManager || LocalContext.IsAdmin)
            {
                if (LocalContext.IsAdmin)
                {
                    PermissionCollection = ContentData.PermissionCollection;
                }
                if (LocalContext.IsManager)
                {
                    PermissionCollection = ContentData.PermissionCollectionAgency;
                }
                IsManager = true;
            }
            else
            {
                IsManager = false;
            }

            if (LocalContext.ProfileSelected.Id == LocalContext.Current.AccountSettings.Id && LocalContext.Current.AccountSettings.Auth == "3")
            {
                IsVisible = false;
            }
            else
            {
                IsVisible = true;
            }

            if(LocalContext.IsManager == true && LocalContext.ProfileSelected.Id == LocalContext.Current.AccountSettings.Id)
            {
                IsAdmin = false;
                PermissionCollection = ContentData.PermissionAgency;
            }    
                
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
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

        public bool IsDelete
        {
            get
            {
                return this.isDelete;
            }

            set
            {
                if (this.isDelete == value)
                {
                    return;
                }

                this.SetProperty(ref this.isDelete, value);
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

        public Category Auth { get; set; }

        public ObservableCollection<Category> PermissionCollection { get; set; }
        #endregion

        #region Command

        /// <summary>
        /// Gets the command that will be executed when an add profile button is clicked.
        /// </summary>
        public Command<object> AddProfileCommand
        {
            get
            {
                return this.addProfileCommand ?? (this.addProfileCommand = new Command<object>(this.AddProfileClicked));
            }
        }

        public Command<object> ConfirmCommand
        {
            get
            {
                return this.confirmCommand ?? (this.confirmCommand = new Command<object>(this.ConfirmClicked));
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

        public Command<object> DeleteProfileCommand
        {
            get
            {
                return this.deleteProfileCommand ?? (this.deleteProfileCommand = new Command<object>(this.DeleteProfileClicked));
            }
        }

        public Command<object> ManageMoneyCommand
        {
            get
            {
                return this.manageMoneyCommand ?? (this.manageMoneyCommand = new Command<object>(this.ManageMoneyClicked));
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

            this.FullName.Value = LocalContext.ProfileSelected.Name;
            this.Email.Value = LocalContext.ProfileSelected.Email;
            this.PhoneNumber = LocalContext.ProfileSelected.Phone;
            this.IsDelete = LocalContext.ProfileSelected.IsDelete;
            var auth = new Category()
            {
                Id = LocalContext.ProfileSelected.Auth,
                Name = LocalContext.ProfileSelected.Auth_ext
            };
            this.Auth = auth;
            this.Money = LocalContext.ProfileSelected.Money.ToString();
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
                Profiles profiles = LocalContext.ProfileSelected;
                profiles.Name = FullName.Value;
                profiles.LastUpdateBy = LocalContext.ProfileSelected.Email;
                profiles.LastUpdateTime = DateTime.Now.ToString();
                profiles.Phone = PhoneNumber;
                profiles.Auth = Auth.Id;
                profiles.Auth_ext = Auth.Name;

                // Do Something
                if(await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.ProfileSelected.Id, JsonConvert.SerializeObject(profiles)))
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Đã cập nhật thông tin thành viên thành công", "OK");
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thông tin thành viên không thành công", "OK");
                }
            }
        }

        /// <summary>
        /// Invoked when add profile button is clicked from the add profile page.
        /// </summary>
        /// <param name="obj">Selected item from the list view.</param>
        private void AddProfileClicked(object obj)
        {
            // Do something
        }

        private async void DeleteProfileClicked(object obj)
        {
            try
            {
                if (LocalContext.ProfileSelected.Money > 0)
                {
                    await App.Current.MainPage.DisplayAlert("Không thể xóa thành viên", "Tiền của thành viên còn trong tài khoản\nHãy rút hết tiền trước khi xóa!", "OK");
                    return;
                }
                // Do something
                var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa thông tin thành viên này?", "Đúng", "Không");
                if (action)
                {
                    Profiles profiles = LocalContext.ProfileSelected;
                    profiles.IsDelete = true;
                    profiles.LastUpdateBy = LocalContext.ProfileSelected.Email;
                    profiles.LastUpdateTime = DateTime.Now.ToString();

                    // Do Something
                    //await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.ProfileSelected.Id, JsonConvert.SerializeObject(profiles));
                    await RealtimeFirebase.Instance.Delete("Profiles", LocalContext.ProfileSelected.Id);

                    await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xóa thông tin thành viên thành công", "OK");
                    await _navigationService.GoBackAsync();
                    isDelete = true;
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            
        }

        private async void ConfirmClicked(object obj)
        {
            // Do something
            Profiles profiles = LocalContext.ProfileSelected;
            profiles.IsConfirm = true;
            this.IsConfirm = !this.IsConfirm;

            // Do Something
            var up = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.ProfileSelected.Id, JsonConvert.SerializeObject(profiles));
            if(up)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã duyệt tài khoản thành công", "OK");
                await _navigationService.GoBackAsync();
            }    
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", "Duyệt tài khoản không thành công", "OK");
                await _navigationService.GoBackAsync();
            }    
            
        }

        private async void ManageMoneyClicked(object obj)
        {
            await _navigationService.NavigateToAsync<MoneyManagePageViewModel>();
        }

        #endregion
    }
}