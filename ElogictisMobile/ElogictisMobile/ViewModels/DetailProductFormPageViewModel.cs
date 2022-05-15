using ElogictisMobile.DataService;
using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for Business Registration Form page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class DetailProductFormPageViewModel : LoginViewModel
    {
        #region Field
        public ValidatableObject<string> nameProduct;
        public ValidatableObject<string> fromFullName;
        public ValidatableObject<string> fromPhone;
        public ValidatableObject<string> fromAddress;
        public ValidatableObject<string> toFullName;
        public ValidatableObject<string> toPhone;
        public ValidatableObject<string> toAddress;
        public ValidatableObject<int> quanlity;
        public ValidatableObject<double> weight;
        public ValidatableObject<string> desciption;
        public ValidatableObject<double> money;
        private INavigationService _navigationService;


        #endregion
        #region Constructor 
        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductFormPageViewModel" /> class
        /// </summary>
        public DetailProductFormPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.InitializeProperties();
            this.AddValidationRules();
            this.TrackingProductCommand = new Command(this.TrackingClicked);
            this.UpdateProductCommand = new Command(this.SubmitClicked);
            this.DeleteProductCommand = new Command(this.DeleteClicked);
            this.CancelProductCommand = new Command(this.CancelClicked);
            this.RefundProductCommand = new Command(this.RefundClicked);
            this.FailedProductCommand = new Command(this.FailedClicked);
            this.TypeProductCollection = LocalContext.ListTypeProduct;
            switch (LocalContext.ProductSelected.Status)
            {
                case 2:
                    {
                        TextButton = "LẤY HÀNG THÀNH CÔNG";
                        break;
                    }
                case 3:
                    {
                        TextButton = "ĐÃ GIAO HÀNG";
                        IsFailed = true;
                        break;
                    }
                default:
                    {
                        TextButton = "CẬP NHẬT";
                        break;
                    }
            }    

            if(LocalContext.ProductSelected.Status >= 2)
            {
                IsCancel = false;
            }
            else
            {
                if(LocalContext.IsShipper)
                {
                    IsCancel = false;
                }
                else
                {
                    IsCancel = true;
                }
            }
        }

        #endregion

        #region Properties
        public Category TypeProduct { get; set; }
        public string TextButton { get; set; }
        public List<Category> TypeProductCollection { get; set; }
        public bool IsFailed { get; set; } = LocalContext.ProductSelected.Status == 3;
        public bool IsManage { get; set; } = LocalContext.IsManager;
        public bool IsRefund { get; set; } = LocalContext.ProductSelected.Status == 5 && !LocalContext.IsShipper;
        public bool IsAdmin { get; set; } = LocalContext.IsAdmin && LocalContext.ProductSelected.Status >= 4;
        public bool IsShipper { get; set; } = LocalContext.IsShipper && LocalContext.ProductSelected.Status < 4;
        public bool IsNotShipper { get; set; } = !LocalContext.IsShipper && LocalContext.ProductSelected.Status < 4;
        public bool IsCancel { get; set; }
        public string Status { get; set; } = LocalContext.ProductSelected.Status_ext;
        public bool IsShowTracking { get; set; } = (LocalContext.ProductSelected.Status >= 2);
        public ValidatableObject<string> NameProduct
        {
            get
            {
                return this.nameProduct;
            }

            set
            {
                if (this.nameProduct == value)
                {
                    return;
                }

                this.SetProperty(ref this.nameProduct, value);
            }
        }
        public ValidatableObject<string> FromFullName
        {
            get
            {
                return this.fromFullName;
            }

            set
            {
                if (this.fromFullName == value)
                {
                    return;
                }

                this.SetProperty(ref this.fromFullName, value);
            }
        }
        public ValidatableObject<string> FromPhone
        {
            get
            {
                return this.fromPhone;
            }

            set
            {
                if (this.fromPhone == value)
                {
                    return;
                }

                this.SetProperty(ref this.fromPhone, value);
            }
        }
        public ValidatableObject<string> FromAddress
        {
            get
            {
                return this.fromAddress;
            }

            set
            {
                if (this.fromAddress == value)
                {
                    return;
                }

                this.SetProperty(ref this.fromAddress, value);
            }
        }
        public ValidatableObject<string> ToFullName
        {
            get
            {
                return this.toFullName;
            }

            set
            {
                if (this.toFullName == value)
                {
                    return;
                }

                this.SetProperty(ref this.toFullName, value);
            }
        }
        public ValidatableObject<string> ToPhone
        {
            get
            {
                return this.toPhone;
            }

            set
            {
                if (this.toPhone == value)
                {
                    return;
                }

                this.SetProperty(ref this.toPhone, value);
            }
        }
        public ValidatableObject<string> ToAddress
        {
            get
            {
                return this.toAddress;
            }

            set
            {
                if (this.toAddress == value)
                {
                    return;
                }

                this.SetProperty(ref this.toAddress, value);
            }
        }
        public ValidatableObject<int> Quanlity
        {
            get
            {
                return this.quanlity;
            }

            set
            {
                if (this.quanlity == value)
                {
                    return;
                }

                this.SetProperty(ref this.quanlity, value);
            }
        }
        public ValidatableObject<double> Weight
        {
            get
            {
                return this.weight;
            }

            set
            {
                if (this.weight == value)
                {
                    return;
                }

                this.SetProperty(ref this.weight, value);
            }
        }
        public ValidatableObject<string> Desciption
        {
            get
            {
                return this.desciption;
            }

            set
            {
                if (this.desciption == value)
                {
                    return;
                }

                this.SetProperty(ref this.desciption, value);
            }
        }
        public ValidatableObject<double> Money
        {
            get
            {
                return this.money;
            }

            set
            {
                if (this.money == value)
                {
                    return;
                }

                this.SetProperty(ref this.money, value);
            }
        }

        #endregion 

        #region Comments

        /// <summary>
        /// Gets or sets the command is executed when the Submit button is clicked.
        /// </summary>
        public Command UpdateProductCommand { get; set; }
        public Command DeleteProductCommand { get; set; }
        public Command CancelProductCommand { get; set; }
        public Command TrackingProductCommand { get; set; }
        public Command FailedProductCommand { get; set; }
        public Command RefundProductCommand { get; set; }

        #endregion

        #region Methods
        private void InitializeProperties()
        {
            this.NameProduct = new ValidatableObject<string>();
            this.FromFullName = new ValidatableObject<string>();
            this.FromPhone = new ValidatableObject<string>();
            this.FromAddress = new ValidatableObject<string>();
            this.ToFullName = new ValidatableObject<string>();
            this.ToPhone = new ValidatableObject<string>();
            this.ToAddress = new ValidatableObject<string>();
            this.Quanlity = new ValidatableObject<int>();
            this.Weight = new ValidatableObject<double>();
            this.Desciption = new ValidatableObject<string>();
            this.Money = new ValidatableObject<double>();

            var tmp = new Category()
            {
                Id = LocalContext.ProductSelected.Type,
                Name = LocalContext.ProductSelected.Type_ext
            };
            this.NameProduct.Value = LocalContext.ProductSelected.Name;
            this.FromFullName.Value = LocalContext.ProductSelected.From_FullName;
            this.FromPhone.Value = LocalContext.ProductSelected.From_PhoneNumber;
            this.FromAddress.Value = LocalContext.ProductSelected.From_Address;
            this.ToFullName.Value = LocalContext.ProductSelected.To_FullName;
            this.ToPhone.Value = LocalContext.ProductSelected.To_PhoneNumber;
            this.ToAddress.Value = LocalContext.ProductSelected.To_Address;
            this.Weight.Value = LocalContext.ProductSelected.Weight;
            this.Quanlity.Value = LocalContext.ProductSelected.Quanlity;
            this.Desciption.Value = LocalContext.ProductSelected.Description;
            this.Money.Value = LocalContext.ProductSelected.Money;
            this.TypeProduct = tmp;
        }
        private void AddValidationRules()
        {
            this.FromFullName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Họ và tên người gửi không được trống" });
            this.FromPhone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Số điện thoại người gửi không được trống" });
            this.FromAddress.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Địa chỉ người gửi không được trống" });
            this.ToFullName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Họ và tên người nhận không được trống" });
            this.ToPhone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Số điện thoại người nhận không được trống" });
            this.ToAddress.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Địa chỉ người nhận không được trống" });
            this.Quanlity.Validations.Add(new IsNotNullOrEmptyRule<int> { ValidationMessage = "Số lượng kiện hàng không được trống" });
            this.Weight.Validations.Add(new IsNotNullOrEmptyRule<double> { ValidationMessage = "Tổng trọng lượng không được trống" });
            this.Money.Validations.Add(new IsNotNullOrEmptyRule<double> { ValidationMessage = "Số tiền thu hộ không được trống" });
        }
        private bool AreFieldsValid()
        {
            bool isFromFullName = this.FromFullName.Validate();
            bool isFromPhone = this.FromPhone.Validate();
            bool isFromAddress = this.FromAddress.Validate();
            bool isToFullName = this.ToFullName.Validate();
            bool isToPhone = this.ToPhone.Validate();
            bool isToAddress = this.ToAddress.Validate();
            bool isQuanlity = this.Quanlity.Validate() && this.Quanlity.Value >= 0;
            bool isWeight = this.Weight.Validate() && this.Weight.Value >= 0;
            bool isMoney = this.Money.Validate() && this.Money.Value >= 0 && this.Money.Value <= 2000000;

            return isFromFullName && isFromPhone && isFromAddress
                && isToFullName && isToPhone && isToAddress && isQuanlity
                && isWeight && isMoney;
        }
        public Products UpdateStatusProduct(string text, int status)
        {
            Products temp = LocalContext.ProductSelected;
            temp.LastUpdateBy = LocalContext.Current.AccountSettings.Id;
            temp.LastUpdateTime = DateTime.Now.ToShortDateString();
            temp.Status = status;
            temp.Status_ext = text;
            return temp;
        }
        public Products UpdateInfoProduct()
        {
            Products temp = LocalContext.ProductSelected;
            temp.Description = Desciption.Value;
            temp.From_Address = FromAddress.Value;
            temp.From_FullName = FromFullName.Value;
            temp.From_PhoneNumber = FromPhone.Value;
            temp.LastUpdateBy = LocalContext.Current.AccountSettings.Id;
            temp.LastUpdateTime = DateTime.Now.ToShortDateString();
            temp.Money = Money.Value;
            temp.Quanlity = Quanlity.Value;
            temp.To_Address = ToAddress.Value;
            temp.To_FullName = ToFullName.Value;
            temp.To_PhoneNumber = ToPhone.Value;
            temp.Type = TypeProduct.Id;
            temp.Type_ext = TypeProduct.Name;
            temp.Weight = Weight.Value;

            return temp;
        }
        private Products FeatureOfShipper()
        {
            var mess = "ĐÃ LẤY HÀNG/ĐANG GIAO";
            Products temp = new Products();

            if (LocalContext.IsShipper)
            {
                if (LocalContext.ProductSelected.Status == 2)
                {
                    temp = UpdateStatusProduct(mess, 3);
                }
                else if (LocalContext.ProductSelected.Status == 3)
                {
                    mess = "GIAO THÀNH CÔNG";
                    temp = UpdateStatusProduct(mess, 4);
                }
                else
                {
                    mess = "GIAO KHÔNG THÀNH CÔNG";
                    temp = UpdateStatusProduct(mess, 5);
                }
            }
            else
            {
                temp = UpdateInfoProduct();
            }

            return temp;
        }
        private async void SubmitClicked(object obj)
        {
            if (this.AreFieldsValid())
            {
                bool updateUser = false;
                var keyNoti = GeneralKey.Instance.General("NOTI");
                Products temp = new Products();

                temp = FeatureOfShipper();

                if(LocalContext.IsShipper)
                {
                    var upsert1 = await RealtimeFirebase.Instance.UpSert("DeliveryTracking/" + temp.ID, "0"+ (temp.Status - 1).ToString(), JsonConvert.SerializeObject(new ProductDeliveryTrackingModel
                    {
                        OrderDate = "",
                        OrderStatus = "",
                        Date = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString(),
                        ProgressValue = temp.Status,
                        Status = temp.Status == 3? "InProgress":"Completed",
                        StepStatus = temp.Status == 3?StepStatus.InProgress:StepStatus.Completed,
                        Title = temp.Status == 3?"ĐƠN HÀNG ĐANG GIAO":"ĐÃ GIAO THÀNH CÔNG",
                        TitleStatus = temp.Status == 3? "Đơn hàng đã được lấy và đang đi giao":"Đơn hàng đã được bên vận chuyển giao thành công"
                    }));

                    //Shipper xác nhận lấy đơn hàng thì trừ tiền trong tài khoản khách hàng
                    if (temp.Status == 3)
                    {
                        string key = GeneralKey.Instance.General("REVENUE");
                        DetailRevenue detailRevenueShipper = new DetailRevenue
                        {
                            Id = key,
                            CreateDate = DateTime.Now.ToShortDateString(),
                            CreateTime = DateTime.Now.ToShortTimeString(),
                            ProductId = LocalContext.ProductSelected.ID,
                            ProductMoney = LocalContext.ProductSelected.Money,
                            ProductName = LocalContext.ProductSelected.Name,
                            ProfilesId = temp.Holder,
                            Profit = Money.Value
                        };
                        await RealtimeFirebase.Instance.UpSert("HistoryOrder/" + temp.CreateBy, key, JsonConvert.SerializeObject(detailRevenueShipper));
                        updateUser = await RealtimeFirebase.Instance.UpdateMoneyUser(Money.Value, false, temp.CreateBy);
                    }

                    //Shipper giao hàng thành công thì phân chia lợi nhuận
                    if (temp.Status == 4)
                    {
                        var moneyShipper = LocalContext.ProductSelected.Money * 0.8;
                        var moneyAgency = LocalContext.ProductSelected.Money * 0.05;
                        var moneyAdmin = LocalContext.ProductSelected.Money * 0.15;
                        string key1 = GeneralKey.Instance.General("REVENUE");
                        string key2 = GeneralKey.Instance.General("REVENUE");
                        string key3 = GeneralKey.Instance.General("REVENUE");
                        DetailRevenue detailRevenueShipper = new DetailRevenue
                        {
                            Id = key1,
                            CreateDate = DateTime.Now.ToShortDateString(),
                            CreateTime = DateTime.Now.ToShortTimeString(),
                            ProductId = LocalContext.ProductSelected.ID,
                            ProductMoney = LocalContext.ProductSelected.Money,
                            ProductName = LocalContext.ProductSelected.Name,
                            ProfilesId = temp.Holder,
                            Profit = moneyShipper
                        };
                        DetailRevenue detailRevenueAdmin = new DetailRevenue
                        {
                            Id = key2,
                            CreateDate = DateTime.Now.ToShortDateString(),
                            CreateTime = DateTime.Now.ToShortTimeString(),
                            ProductId = LocalContext.ProductSelected.ID,
                            ProductMoney = LocalContext.ProductSelected.Money,
                            ProductName = LocalContext.ProductSelected.Name,
                            ProfilesId = LocalContext.AdminId,
                            Profit = moneyAdmin
                        };
                        DetailRevenue detailRevenueAgency = new DetailRevenue
                        {
                            Id = key3,
                            CreateDate = DateTime.Now.ToShortDateString(),
                            CreateTime = DateTime.Now.ToShortTimeString(),
                            ProductId = LocalContext.ProductSelected.ID,
                            ProductMoney = LocalContext.ProductSelected.Money,
                            ProductName = LocalContext.ProductSelected.Name,
                            ProfilesId = temp.AgencyId,
                            Profit = moneyAgency
                        };

                        var shipper = await RealtimeFirebase.Instance.UpdateMoneyUser(moneyShipper, true, temp.Holder);
                        var agency = await RealtimeFirebase.Instance.UpdateMoneyAgency(moneyAgency, true, temp.AgencyId);
                        var admin = await RealtimeFirebase.Instance.UpdateMoneyUser(moneyAdmin, true, LocalContext.AdminId);
                        updateUser = shipper && agency && admin;

                        if (updateUser)
                        {
                            var allPersons = await RealtimeFirebase.Instance.GetProfilesAgency(temp.AgencyId);

                            await RealtimeFirebase.Instance.UpSert("Revenue/" + temp.Holder, key1, JsonConvert.SerializeObject(detailRevenueShipper));
                            await RealtimeFirebase.Instance.UpSert("Revenue/" + allPersons.Id, key3, JsonConvert.SerializeObject(detailRevenueAgency));
                            await RealtimeFirebase.Instance.UpSert("Revenue/" + LocalContext.AdminId, key2, JsonConvert.SerializeObject(detailRevenueAdmin));
                        }
                    }

                }
                else
                {
                    updateUser = true;
                }    

                var upsert = await RealtimeFirebase.Instance.UpSert("Products", temp.ID, JsonConvert.SerializeObject(temp));
                
                if (upsert && updateUser)
                {
                    //await RealtimeFirebase.Instance.UpSert("Notifications", keyNoti, JsonConvert.SerializeObject(new TransactionHistory
                    //{
                    //    IdProduct = temp.ID,
                    //    TransactionDescription = temp.Status_ext,
                    //    Date = DateTime.Now.ToShortDateString(),
                    //    Time = DateTime.Now.ToShortTimeString(),
                    //    Email = LocalContext.Current.AccountSettings.Email,
                    //    ProfileId = LocalContext.ProductSelected.CreateBy
                    //}));
                    await _navigationService.GoBackAsync();
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật trạng thái đơn hàng thành công!", "OK");
    
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật trạng thái đơn hàng không thành công!", "OK");
                }
            }
        }
        private async void RefundClicked(object obj)
        {
            if (this.AreFieldsValid())
            {
                Products temp = LocalContext.ProductSelected;
                var updateUser = false;
                temp.Status = 6;
                temp.Status_ext = "ĐÃ HOÀN ĐƠN";
                //Shipper giao hàng thành công thì phân chia lợi nhuận
                if (temp.Status == 6)
                {
                    var moneyShipper = LocalContext.ProductSelected.Money * 0.8;
                    var moneyAgency = LocalContext.ProductSelected.Money * 0.05;
                    var moneyAdmin = LocalContext.ProductSelected.Money * 0.15;
                    string key1 = GeneralKey.Instance.General("REVENUE");
                    string key2 = GeneralKey.Instance.General("REVENUE");
                    string key3 = GeneralKey.Instance.General("REVENUE");
                    DetailRevenue detailRevenueShipper = new DetailRevenue
                    {
                        Id = key1,
                        CreateDate = DateTime.Now.ToShortDateString(),
                        CreateTime = DateTime.Now.ToShortTimeString(),
                        ProductId = LocalContext.ProductSelected.ID,
                        ProductMoney = LocalContext.ProductSelected.Money,
                        ProductName = LocalContext.ProductSelected.Name,
                        ProfilesId = temp.Holder,
                        Profit = moneyShipper
                    };
                    DetailRevenue detailRevenueAdmin = new DetailRevenue
                    {
                        Id = key2,
                        CreateDate = DateTime.Now.ToShortDateString(),
                        CreateTime = DateTime.Now.ToShortTimeString(),
                        ProductId = LocalContext.ProductSelected.ID,
                        ProductMoney = LocalContext.ProductSelected.Money,
                        ProductName = LocalContext.ProductSelected.Name,
                        ProfilesId = LocalContext.AdminId,
                        Profit = moneyAdmin
                    };
                    DetailRevenue detailRevenueAgency = new DetailRevenue
                    {
                        Id = key3,
                        CreateDate = DateTime.Now.ToShortDateString(),
                        CreateTime = DateTime.Now.ToShortTimeString(),
                        ProductId = LocalContext.ProductSelected.ID,
                        ProductMoney = LocalContext.ProductSelected.Money,
                        ProductName = LocalContext.ProductSelected.Name,
                        ProfilesId = temp.AgencyId,
                        Profit = moneyAgency
                    };

                    var shipper = await RealtimeFirebase.Instance.UpdateMoneyUser(moneyShipper, true, temp.Holder);
                    var agency = await RealtimeFirebase.Instance.UpdateMoneyAgency(moneyAgency, true, temp.AgencyId);
                    var admin = await RealtimeFirebase.Instance.UpdateMoneyUser(moneyAdmin, true, LocalContext.AdminId);
                    updateUser = shipper && agency && admin;

                    if (updateUser)
                    {
                        var allPersons = await RealtimeFirebase.Instance.GetProfilesAgency(temp.AgencyId);

                        await RealtimeFirebase.Instance.UpSert("Revenue/" + temp.Holder, key1, JsonConvert.SerializeObject(detailRevenueShipper));
                        await RealtimeFirebase.Instance.UpSert("Revenue/" + allPersons.Id, key3, JsonConvert.SerializeObject(detailRevenueAgency));
                        await RealtimeFirebase.Instance.UpSert("Revenue/" + LocalContext.AdminId, key2, JsonConvert.SerializeObject(detailRevenueAdmin));
                    }
                }

                var upsert = await RealtimeFirebase.Instance.UpSert("Products", temp.ID, JsonConvert.SerializeObject(temp));

                if (upsert && updateUser)
                {
                    await _navigationService.GoBackAsync();
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật trạng thái đơn hàng thành công!", "OK");

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật trạng thái đơn hàng không thành công!", "OK");
                }
            }
        }
        private async void DeleteClicked(object obj)
        {
            // Do something
            var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn xóa thông tin đơn hàng này?", "Đúng", "Không");
            if (action)
            {
                Products products = LocalContext.ProductSelected;
                products.IsDelete = true;
                products.LastUpdateBy = LocalContext.Profiles.Email;
                products.LastUpdateTime = DateTime.Now.ToString();

                // Do Something
                //await RealtimeFirebase.Instance.UpSert("Products", products.ID, JsonConvert.SerializeObject(products));
                var dele = await RealtimeFirebase.Instance.Delete("Products", products.ID);
                if(dele)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Đã xóa thông tin đơn hàng thành công", "OK");
                    await _navigationService.GoBackAsync();
                }    
                else
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Xóa thông tin đơn hàng không thành công", "OK");
                }    
            }
        }
        private async void CancelClicked(object obj)
        {
            // Do something
            var action = await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn có thực sự muốn HỦY ĐƠN này?", "Đúng", "Không");
            if (action)
            {
                Products products = LocalContext.ProductSelected;
                products.Status = 7;
                products.Status_ext = "ĐÃ HỦY ĐƠN";
                products.LastUpdateBy = LocalContext.Profiles.Email;
                products.LastUpdateTime = DateTime.Now.ToString();

                // Do Something
                await RealtimeFirebase.Instance.UpSert("Products", products.ID, JsonConvert.SerializeObject(products));
                //await RealtimeFirebase.Instance.Delete("Products", products.ID);
                await App.Current.MainPage.DisplayAlert("Thông báo", "Đã hủy đơn hàng thành công", "OK");
            }
        }
        public async void TrackingClicked()
        {
            await _navigationService.NavigateToAsync<ProductDeliveryTrackingPageViewModel>();
        }

        private async void FailedClicked(object obj)
        {
            var u = await App.Current.MainPage.DisplayAlert("Thông báo", "Xác nhận đơn hàng đã giao không thành công?", "Đúng", "Không");
            if(u)
            {
                await _navigationService.NavigateToAsync<FailedProductPageViewModel>();
            }    
        }
        #endregion
    }
}