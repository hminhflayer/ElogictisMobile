using System;
using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
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
    public class AddCategoryTypeShipperPageViewModel : BaseViewModel
    {
        #region Fields

        private ValidatableObject<string> name;
        private ValidatableObject<int> maxReviced;
        private ValidatableObject<int> timeHold;
        private bool prioritize;

        public bool update = false;

        private Command<object> addCategoryTypeProductCommand;
        private INavigationService _navigationService;
        private Command<object> deleteCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializing the profile details.
        /// </summary>
        public AddCategoryTypeShipperPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            IsLoading = false;

            if(LocalContext.TypeShipProductSelected == null)
            {
                update = false;
            }    
            else
            {
                if (string.IsNullOrEmpty(LocalContext.TypeShipProductSelected.Id))
                {
                    update = false;
                }
                else
                {
                    update = true;
                    this.Name.Value = LocalContext.TypeShipProductSelected.Name;
                    this.TimeHold.Value = LocalContext.TypeShipProductSelected.TimeHold;
                    this.Prioritize = LocalContext.TypeShipProductSelected.Prioritize;
                }    
                
            }    
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

        public ValidatableObject<int> MaxReviced
        {
            get
            {
                return this.maxReviced;
            }

            set
            {
                if (this.maxReviced == value)
                {
                    return;
                }

                this.SetProperty(ref this.maxReviced, value);
            }
        }

        public ValidatableObject<int> TimeHold
        {
            get
            {
                return this.timeHold;
            }

            set
            {
                if (this.timeHold == value)
                {
                    return;
                }

                this.SetProperty(ref this.timeHold, value);
            }
        }
        public bool Prioritize
        {
            get
            {
                return this.prioritize;
            }

            set
            {
                if (this.prioritize == value)
                {
                    return;
                }

                this.SetProperty(ref this.prioritize, value);
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
    public Command<object> AddCategoryTypeProductCommand
        {
            get
            {
                return this.addCategoryTypeProductCommand ?? (this.addCategoryTypeProductCommand = new Command<object>(this.AddCategoryTypeShipClicked));
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
            bool isTimeHoldValid = this.TimeHold.Validate();
            bool isMaxRevicedValid = this.MaxReviced.Validate();
            return isNameValid && isTimeHoldValid && isMaxRevicedValid;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.TimeHold = new ValidatableObject<int>();
            this.MaxReviced = new ValidatableObject<int>();
        }

        /// <summary>
        /// Validation rules for name fields
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Tên hình thức giao không được trống" });
            this.TimeHold.Validations.Add(new IsNotNullOrEmptyRule<int> { ValidationMessage = "Thời gian tối đa không được trống" });
            this.MaxReviced.Validations.Add(new IsNotNullOrEmptyRule<int> { ValidationMessage = "Số lượng đơn giữ cùng lúc không được trống" });
        }

        /// <summary>
        /// Invoked when add contact button is clicked from the add profile page.
        /// </summary>
        /// <param name="obj">Selected item from the list view.</param>
        private async void AddCategoryTypeShipClicked(object obj)
        {
            try
            {
                if (this.AreNamesValid())
                {
                    if(update)
                    {
                        IsLoading = true;

                        TypeShipProduct category = LocalContext.TypeShipProductSelected;
                        category.Prioritize = prioritize;
                        category.Name = Name.Value;
                        category.TimeHold = TimeHold.Value;
                        category.CreateTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();

                        // Do Something
                        var upSert = await RealtimeFirebase.Instance.UpSert("Categories/TypeShip", category.Id, JsonConvert.SerializeObject(category));
                        if (upSert)
                        {
                            IsLoading = false;
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thông tin hình thức giao hàng thành công", "OK");
                        }
                    }
                    else
                    {
                        IsLoading = true;
                        var key = GeneralKey.Instance.General("CTS");
                        TypeShipProduct category = new TypeShipProduct()
                        {
                            Name = Name.Value,
                            Id = key,
                            TimeHold = TimeHold.Value,
                            CreateTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString(),
                            Prioritize = prioritize
                        };
                        // Do Something
                        var upSert = await RealtimeFirebase.Instance.UpSert("Categories/TypeShip", key, JsonConvert.SerializeObject(category));
                        if (upSert)
                        {
                            IsLoading = false;
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Đã thêm hình thức giao hàng thành công", "OK");
                            await _navigationService.GoBackAsync();
                        }
                    }  
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            
        }

        private async void DeleteClicked(object obj)
        {
            // Do something
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa hình thức giao hàng này?", "Đúng", "Không");
                if (action)
                {
                    var del = await RealtimeFirebase.Instance.Delete("Categories/TypeShip", LocalContext.TypeShipProductSelected.Id);
                    if (del)
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xóa hình thức giao hàng thành công", "OK");
                        await _navigationService.GoBackAsync();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Có lỗi khi xóa bảng giá", "OK");
                    }

                    await _navigationService.GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        #endregion
    }
}
