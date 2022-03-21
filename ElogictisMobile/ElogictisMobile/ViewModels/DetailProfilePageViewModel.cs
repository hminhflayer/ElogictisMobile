using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
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

        private ValidatableObject<string> fullName;

        private ValidatableObject<string> email;

        private string phoneNumber;

        private bool isDelete;

        private Command<object> updateProfileCommand;

        private Command<object> addProfileCommand;

        private Command<object> deleteProfileCommand;
        private INavigationService _navigationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializing the profile details.
        /// </summary>
        public DetailProfilePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
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

            this.FullName.Value = LocalContext.ProfileSelected.Profile_Name;
            this.Email.Value = LocalContext.ProfileSelected.Profile_Email;
            this.PhoneNumber = LocalContext.ProfileSelected.Profile_Phone;
            this.IsDelete = LocalContext.ProfileSelected.Profile_IsDelete;
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
                Profiles profiles = LocalContext.Profiles;
                profiles.Profile_Name = FullName.Value;
                profiles.Profile_LastUpdateBy = LocalContext.Profiles.Profile_Email;
                profiles.Profile_LastUpdateTime = DateTime.Now.ToString();
                profiles.Profile_Phone = PhoneNumber;

                // Do Something
                await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.Profiles.Profile_Id, JsonConvert.SerializeObject(profiles));
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã cập nhật thông tin thành viên thành công", "OK");
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
            // Do something
            var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa thông tin thành viên này?", "Đúng","Không");
            if(action)
            {
                Profiles profiles = LocalContext.Profiles;
                profiles.Profile_IsDelete = true;
                profiles.Profile_LastUpdateBy = LocalContext.Profiles.Profile_Email;
                profiles.Profile_LastUpdateTime = DateTime.Now.ToString();

                // Do Something
                await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.Profiles.Profile_Id, JsonConvert.SerializeObject(profiles));
                //await RealtimeFirebase.Instance.Delete("Profiles", LocalContext.ProfileSelected.Profile_Id);
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xóa thông tin thành viên thành công", "OK");
                isDelete = true;
            }    
        }

        #endregion
    }
}