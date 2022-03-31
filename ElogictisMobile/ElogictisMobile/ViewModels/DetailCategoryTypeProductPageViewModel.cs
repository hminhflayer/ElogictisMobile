using ElogictisMobile.Models;
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
    public class DetailCategoryTypeProductPageViewModel : BaseViewModel
    {
        #region Fields

        private ValidatableObject<string> name;

        private Command<object> updateCommand;

        private Command<object> deleteCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializing the profile details.
        /// </summary>
        public DetailCategoryTypeProductPageViewModel()
        {
            this.InitializeProperties();
            this.AddValidationRules();
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
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

        #endregion

        #region Command

        /// <summary>
        /// Gets the command that will be executed when an add profile button is clicked.
        /// </summary>
        public Command<object> DeleteCommand
        {
            get
            {
                return this.deleteCommand ?? (this.deleteCommand = new Command<object>(this.DeleteClicked));
            }
        }

        /// <summary>
        /// Gets the command that will be executed when an add contact button is clicked.
        /// </summary>
        public Command<object> UpdateCommand
        {
            get
            {
                return this.updateCommand ?? (this.updateCommand = new Command<object>(this.UpdateClicked));
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
            bool isNameValid = this.Name.Validate();
            return isNameValid;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();

            this.Name.Value = LocalContext.CategorySelected.Name;
        }

        /// <summary>
        /// Validation rules for name fields
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Tên danh mục không được trống" });
        }

        /// <summary>
        /// Invoked when add contact button is clicked from the add profile page.
        /// </summary>
        /// <param name="obj">Selected item from the list view.</param>
        private async void UpdateClicked(object obj)
        {
            try
            {
                if (this.AreNamesValid())
                {
                    Category category = LocalContext.CategorySelected;
                    category.Name = Name.Value;

                    // Do Something
                    var upSert = await RealtimeFirebase.Instance.UpSert("Categories/TypeProduct", category.Id, JsonConvert.SerializeObject(category));
                    if (upSert)
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đã cập nhật thành công", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Có lỗi khi cập nhật", "OK");
                    }
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            
        }

        /// <summary>
        /// Invoked when add profile button is clicked from the add profile page.
        /// </summary>
        /// <param name="obj">Selected item from the list view.</param>
        private async void DeleteClicked(object obj)
        {
            // Do something
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa thông tin đơn hàng này?", "Đúng", "Không");
                if (action)
                {
                    var del = await RealtimeFirebase.Instance.Delete("Categories/TypeProduct", LocalContext.CategorySelected.Id);
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
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        #endregion
    }
}
