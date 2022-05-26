using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class AddCategoryPriceListPageViewModel : BaseViewModel
    {
        #region Fields

        private ValidatableObject<string> fromKilometer;
        private ValidatableObject<string> toKilometer;
        private ValidatableObject<string> fromWeight;
        private ValidatableObject<string> toWeight;
        private ValidatableObject<string> price;
        public bool update = false;

        private Command<object> addPriceListCommand;
        private Command<object> deleteCommand;

        #endregion

        #region Property
        public Category TypeProduct { get; set; }
        public ObservableCollection<Category> TypeProductCollection { get; set; }

        public TypeShipProduct TypeShipProduct { get; set; }

        private INavigationService _navigationService;

        public List<TypeShipProduct> TypeShipProducts { get; set; }
        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        /// 
        public Command<object> DeleteCommand
        {
            get
            {
                return this.deleteCommand ?? (this.deleteCommand = new Command<object>(this.DeleteClicked));
            }
        }
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
        public Command<object> AddPriceListCommand
        {
            get
            {
                return this.addPriceListCommand ?? (this.addPriceListCommand = new Command<object>(this.AddPriceListClicked));
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
            this.FromKilometer = new ValidatableObject<string>();
            this.ToKilometer = new ValidatableObject<string>();
            this.FromWeight = new ValidatableObject<string>();
            this.ToWeight = new ValidatableObject<string>();
            this.Price = new ValidatableObject<string>();

            if(update)
            {
                this.FromKilometer.Value = LocalContext.PriceListSelected.From_Kilometer;
                this.ToKilometer.Value = LocalContext.PriceListSelected.To_Kilometer;
                this.FromWeight.Value = LocalContext.PriceListSelected.From_Weight;
                this.ToWeight.Value = LocalContext.PriceListSelected.To_Weight;
                this.Price.Value = LocalContext.PriceListSelected.Price;

                Category typeProduct = new Category()
                {
                    Id = LocalContext.PriceListSelected.TypeProduct,
                    Name = LocalContext.PriceListSelected.TypeProduct_ext,
                };

                TypeProduct = typeProduct;

                foreach (var item in TypeShipProducts)
                {
                    if(item.Id == LocalContext.PriceListSelected.TypeShipProduct && item.Name == LocalContext.PriceListSelected.TypeShipProduct_ext)
                    {
                        TypeShipProduct = item;
                        break;
                    }    
                }    

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
        private async void AddPriceListClicked(object obj)
        {
            try
            {
                if (this.AreNamesValid())
                {
                    if(update)
                    {
                        PriceList priceList_tmp = LocalContext.PriceListSelected;
                        PriceList priceList = new PriceList()
                        {
                            Id = priceList_tmp.Id,
                            From_Kilometer = this.FromKilometer.Value,
                            To_Kilometer = this.ToKilometer.Value,
                            From_Weight = this.FromWeight.Value,
                            To_Weight = this.ToWeight.Value,
                            Price = this.Price.Value,
                            IsDelete = false,
                            TypeProduct = this.TypeProduct.Id,
                            TypeProduct_ext = this.TypeProduct.Name,
                            TypeShipProduct = this.TypeShipProduct.Id,
                            TypeShipProduct_ext = this.TypeShipProduct.Name
                        };

                        IsLoading = true;
                        // Do Something
                        var upSert = await RealtimeFirebase.Instance.UpSert("Categories/PricesList", priceList_tmp.Id, JsonConvert.SerializeObject(priceList));
                        if (upSert)
                        {
                            IsLoading = false;
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật bảng giá thành công", "OK");
                            await _navigationService.GoBackAsync();
                        }
                    }   
                    else
                    {
                        var key = GeneralKey.Instance.General("PRL");
                        PriceList priceList = new PriceList()
                        {
                            Id = key,
                            From_Kilometer = this.FromKilometer.Value,
                            To_Kilometer = this.ToKilometer.Value,
                            From_Weight = this.FromWeight.Value,
                            To_Weight = this.ToWeight.Value,
                            Price = this.Price.Value,
                            IsDelete = false,
                            TypeProduct = this.TypeProduct.Id,
                            TypeProduct_ext = this.TypeProduct.Name,
                            TypeShipProduct = this.TypeShipProduct.Id,
                            TypeShipProduct_ext = this.TypeShipProduct.Name
                        };

                        IsLoading = true;
                        // Do Something
                        var upSert = await RealtimeFirebase.Instance.UpSert("Categories/PricesList", key, JsonConvert.SerializeObject(priceList));
                        if (upSert)
                        {
                            IsLoading = false;
                            await App.Current.MainPage.DisplayAlert("Thông báo", "Thêm bảng giá thành công", "OK");
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
                var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa cấu hình bảng giá này?", "Đúng", "Không");
                if (action)
                {
                    var del = await RealtimeFirebase.Instance.Delete("Categories/PricesList", LocalContext.PriceListSelected.Id);
                    if (del)
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Đã bảng giá xóa thành công", "OK");
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

        #region Constructor

        /// <summary>
        /// Initializing the profile details.
        /// </summary>
        public AddCategoryPriceListPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            TypeShipProducts = new List<TypeShipProduct>();
            TypeProductCollection = new ObservableCollection<Category>();
            TypeProduct = new Category();
            TypeShipProduct = new TypeShipProduct();
            TypeProductCollection = RealtimeFirebase.Instance.GetAllCategory<Category>("TypeProduct");

            TypeShipProducts = LocalContext.ListTypeShipProductCollection;

            if(!(LocalContext.PriceListSelected == null))
            {
                if (!string.IsNullOrEmpty(LocalContext.PriceListSelected.Id))
                {
                    update = true;
                }
                else
                {
                    update = false;
                }    
                
            }   
            else
            {
                update = false;
                if (TypeShipProducts.Count != 0)
                {
                    TypeShipProduct = TypeShipProducts[0];
                }

                if (TypeProductCollection.Count != 0)
                {
                    TypeProduct = TypeProductCollection[0];
                }
            }
            this.InitializeProperties();
            this.AddValidationRules();

        }

        #endregion

    }
}
