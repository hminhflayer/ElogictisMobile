using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ElogictisMobile.ViewModels
{
    public class DetailCategoryPricesListPageViewModel : BaseViewModel
    {
        #region Fields

        private ValidatableObject<string> fromKilometer;
        private ValidatableObject<string> toKilometer;
        private ValidatableObject<string> fromWeight;
        private ValidatableObject<string> toWeight;
        private ValidatableObject<string> price;

        private Command<object> updateCommand;

        private Command<object> deleteCommand;

        #endregion

        #region Constructor

        public DetailCategoryPricesListPageViewModel()
        {
            this.InitializeProperties();
            this.AddValidationRules();
        }

        #endregion

        #region Property
        public Category TypeProduct { get; set; }
        public ObservableCollection<Category> TypeProductCollection { get; set; } = RealtimeFirebase.Instance.GetAllCategory<Category>("TypeProduct");
        public ValidatableObject<string> FromKilometer
        {
            get
            {
                return this.fromKilometer;
            }

            set
            {
                if (this.fromKilometer == value)
                {
                    return;
                }

                this.SetProperty(ref this.fromKilometer, value);
            }
        }
        public ValidatableObject<string> ToKilometer
        {
            get
            {
                return this.toKilometer;
            }

            set
            {
                if (this.toKilometer == value)
                {
                    return;
                }

                this.SetProperty(ref this.toKilometer, value);
            }
        }
        public ValidatableObject<string> FromWeight
        {
            get
            {
                return this.fromWeight;
            }

            set
            {
                if (this.fromWeight == value)
                {
                    return;
                }

                this.SetProperty(ref this.fromWeight, value);
            }
        }
        public ValidatableObject<string> ToWeight
        {
            get
            {
                return this.toWeight;
            }

            set
            {
                if (this.toWeight == value)
                {
                    return;
                }

                this.SetProperty(ref this.toWeight, value);
            }
        }
        public ValidatableObject<string> Price
        {
            get
            {
                return this.price;
            }

            set
            {
                if (this.price == value)
                {
                    return;
                }

                this.SetProperty(ref this.price, value);
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

        public bool AreNamesValid()
        {
            bool isFromKilometerValid = this.FromKilometer.Validate();
            bool isToKilometerValid = this.ToKilometer.Validate();
            bool isFromWeightValid = this.FromWeight.Validate();
            bool isToWeightValid = this.ToWeight.Validate();
            bool iPriceValid = this.Price.Validate();
            return isFromKilometerValid && isToKilometerValid && isFromWeightValid && isToWeightValid && iPriceValid;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            try
            {
                this.FromKilometer = new ValidatableObject<string>();
                this.ToKilometer = new ValidatableObject<string>();
                this.FromWeight = new ValidatableObject<string>();
                this.ToWeight = new ValidatableObject<string>();
                this.Price = new ValidatableObject<string>();

                this.FromKilometer.Value = LocalContext.PriceListSelected.From_Kilometer;
                this.ToKilometer.Value = LocalContext.PriceListSelected.To_Kilometer;
                this.FromWeight.Value = LocalContext.PriceListSelected.From_Weight;
                this.ToWeight.Value = LocalContext.PriceListSelected.To_Weight;
                this.Price.Value = LocalContext.PriceListSelected.Price;
                var obj = new Category()
                {
                    Id = LocalContext.PriceListSelected.TypeProduct,
                    Name = LocalContext.PriceListSelected.TypeProduct_ext
                };
                TypeProduct = obj;
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            
        }

        /// <summary>
        /// Validation rules for name fields
        /// </summary>
        private void AddValidationRules()
        {
            this.FromKilometer.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Khoảng cách tối thiểu không được trống" });
            this.ToKilometer.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Khoảng cách tối đa không được trống" });
            this.FromWeight.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Trọng lượng tối thiểu không được trống" });
            this.ToWeight.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Trọng lượng tối đa không được trống" });
            this.Price.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Giá tiền không được trống" });
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
                    PriceList priceList = LocalContext.PriceListSelected;
                    priceList.From_Kilometer = FromKilometer.Value;
                    priceList.To_Kilometer = ToKilometer.Value;
                    priceList.From_Weight = FromWeight.Value;
                    priceList.To_Weight = ToWeight.Value;
                    priceList.Price = Price.Value;
                    priceList.TypeProduct = TypeProduct.Id;
                    priceList.TypeProduct_ext = TypeProduct.Name;

                    IsLoading = true;
                    // Do Something
                    var upSert = await RealtimeFirebase.Instance.UpSert("Categories/PricesList", priceList.Id, JsonConvert.SerializeObject(priceList));
                    if (upSert)
                    {
                        IsLoading = false;
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đã cập nhật thành công", "OK");
                    }
                    else
                    {
                        IsLoading = false;
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Có lỗi khi cập nhật", "OK");
                    }
                }
            }
            catch (Exception ex)
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
                    var del = await RealtimeFirebase.Instance.Delete("Categories/PricesList", LocalContext.PriceListSelected.Id);
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

        #endregion
    }
}
