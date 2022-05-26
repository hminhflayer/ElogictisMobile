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
    /// ViewModel for add profile page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AddDistrictPageViewModel : BaseViewModel
    {
        #region Constructor
        public ValidatableObject<string> id;
        public ValidatableObject<string> name;
        private INavigationService _navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public AddDistrictPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            this.SubmitCommand = new Command(this.SubmitClicked);
            this.DeleteCommand = new Command(this.DeleteClicked);
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
        public ObservableCollection<Category> ProvinceCollection { get; set; } = LocalContext.ProvinceList;
        #endregion 

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command SubmitCommand { get; set; }
        public Command DeleteCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializzing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.Id = new ValidatableObject<string>();
            this.Province = LocalContext.ProvinceSelected;

            if (LocalContext.IsEdit)
            {
                Id.Value = LocalContext.DistrictSelected.Id;
                Name.Value = LocalContext.DistrictSelected.Name;
                Category provi = new Category()
                {
                    Id = LocalContext.DistrictSelected.ProvinceId,
                    Name = LocalContext.DistrictSelected.ProvinceName
                };
                this.Province = provi;
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
                var district = new District();
                if (IsEdit)
                {
                    district = LocalContext.DistrictSelected;
                    district.Name = Name.Value;
                    district.Id = Id.Value;
                    district.ProvinceName = Province.Name;
                    district.ProvinceId = Province.Id;
                    mess = "Cập nhật thông tin Quận/Huyện thành công!";
                }    
                else
                {
                    district.Name = Name.Value;
                    district.Id = Id.Value;
                    district.ProvinceName = Province.Name;
                    district.ProvinceId = Province.Id;
                    mess = "Thêm Quận/Huyện thành công!";
                }    

                var task = await RealtimeFirebase.Instance.UpSert("Categories/District/"+ Province.Id, Id.Value, JsonConvert.SerializeObject(district));
                if (task)
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", mess, "OK");
                }
                else
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm/Cập nhật thông tin Quận/Huyện không thành công!", "OK");
                }
            }
        }

        private async void DeleteClicked(object obj)
        {
            // Do something
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa Quận/Huyện "+ Name.Value +" ?", "Đúng", "Không");
                if (action)
                {
                    var del = await RealtimeFirebase.Instance.Delete("Categories/District/"+Province.Id, Id.Value);
                    if (del)
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xóa Quận/Huyện " + Name.Value + " thành công", "OK");
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

        #endregion
    }
}
