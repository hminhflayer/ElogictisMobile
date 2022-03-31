using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using Plugin.Media;
using System;
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
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public ImageSource Avatar { get; set; }
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

            this.FullName.Value = LocalContext.Current.AccountSettings.Name;
            this.Email.Value = LocalContext.Current.AccountSettings.Email;
            this.PhoneNumber = LocalContext.Current.AccountSettings.Phone;
            this.Avatar = LocalContext.Current.AccountSettings.Avatar;
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
                Profiles profiles = LocalContext.Current.AccountSettings;
                profiles.Name = FullName.Value;
                profiles.LastUpdateBy = LocalContext.Profiles.Email;
                profiles.LastUpdateTime = DateTime.Now.ToString();
                profiles.Phone = PhoneNumber;

                // Do Something
                await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.Profiles.Id, JsonConvert.SerializeObject(profiles));
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã cập nhật thông tin tài khoản thành công", "OK");

                LocalContext.Current.AccountSettings = profiles;
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
            await App.Current.MainPage.DisplayAlert("File Location", file.AlbumPath, "OK");
            var filepath = file.Path;

            var tmp = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();

                //file.Dispose();
                return stream;
            });

            Avatar = tmp;
        }

        #endregion
    }
}
