using System;
using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for add card page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AddCategoryTypeProductPageViewModel : BaseViewModel
    {
        #region Fields

        private ValidatableObject<string> name;

        private Command<object> addCategoryTypeProductCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializing the profile details.
        /// </summary>
        public AddCategoryTypeProductPageViewModel()
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
        public Command<object> AddCategoryTypeProductCommand
        {
            get
            {
                return this.addCategoryTypeProductCommand ?? (this.addCategoryTypeProductCommand = new Command<object>(this.AddCategoryTypeProductClicked));
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
            return isNameValid ;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
        }

        /// <summary>
        /// Validation rules for name fields
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Tên loại hàng không được trống" });
        }

        /// <summary>
        /// Invoked when add contact button is clicked from the add profile page.
        /// </summary>
        /// <param name="obj">Selected item from the list view.</param>
        private async void AddCategoryTypeProductClicked(object obj)
        {
            try
            {
                if (this.AreNamesValid())
                {
                    var key = GeneralKey.Instance.General("CTP");
                    Category category = new Category()
                    {
                        Name = Name.Value,
                        Id = key
                    };

                    IsLoading = true;
                    // Do Something
                    var upSert = await RealtimeFirebase.Instance.UpSert("Categories/TypeProduct", key, JsonConvert.SerializeObject(category));
                    if (upSert)
                    {
                        IsLoading = false;
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đã thêm loại đơn hàng thành công", "OK");
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
