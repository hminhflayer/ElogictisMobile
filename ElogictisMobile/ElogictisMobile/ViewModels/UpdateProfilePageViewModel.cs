﻿using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
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

            this.FullName.Value = LocalContext.Profiles.Profile_Name;
            this.Email.Value = LocalContext.Profiles.Profile_Email;
            this.PhoneNumber = LocalContext.Profiles.Profile_Phone;
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
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã cập nhật thông tin tài khoản thành công", "OK");
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

        #endregion
    }
}
