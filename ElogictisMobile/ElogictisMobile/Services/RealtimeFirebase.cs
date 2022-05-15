using ElogictisMobile.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElogictisMobile.Services
{
    public class RealtimeFirebase
    {
        FirebaseClient client;
        
        private static RealtimeFirebase instance;

        ObservableCollection<Products> ProductsAgency = new ObservableCollection<Products>();
        ObservableCollection<Products> ProductsShipper = new ObservableCollection<Products>();
        ObservableCollection<Products> ProductsUser = new ObservableCollection<Products>();

        #region Properties

        #endregion

        public static RealtimeFirebase Instance
        {
            get { if (instance == null) instance = new RealtimeFirebase(); return RealtimeFirebase.instance; }
            private set { RealtimeFirebase.instance = value; }
        }

        public RealtimeFirebase()
        {
            client = new FirebaseClient("https://elogictismobile-default-rtdb.firebaseio.com/");
        }

        #region Default
        public Task<bool> UpSert(string collections = null, string key = null, string data = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                if (!string.IsNullOrEmpty(collections)
                    && !string.IsNullOrEmpty(key)
                    && !string.IsNullOrEmpty(data))
                {

                    client
                    .Child(collections)
                    .Child(key)
                    .PutAsync(data)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));

                    return tcs.Task;
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            return tcs.Task;
        }
        public async Task<string> GetOne<T>(string collections = null, string key = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(collections)
                    && !string.IsNullOrEmpty(key))
                {
                    var items = await client
                    .Child(collections)
                    .Child(key)
                    .OnceAsync<T>();

                    return items.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            return "";
        }
        public ObservableCollection<T> GetAll<T>(string collection = null)
        {
            if (string.IsNullOrEmpty(collection))
            {
                return null;
            }

            var profiles = client
                .Child(collection)
                .OrderByKey()
                .AsObservable<T>()
                .AsObservableCollection();
   
            return profiles;
        }
        public Task<bool> Delete(string collections = null, string key = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                if (!string.IsNullOrEmpty(collections)
                    && !string.IsNullOrEmpty(key))
                {
                    client
                   .Child(collections)
                   .Child(key)
                   .DeleteAsync()
                   .ContinueWith((task) => OnAuthCompleted(task, tcs));

                    return tcs.Task;
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            return tcs.Task;
        }
        #endregion

        #region Profiles
        public async Task<List<Profiles>> GetAllProfiles()
        {

            return (await client
              .Child("Profiles")
              .OnceAsync<Profiles>()).Select(item => new Profiles
              {
                  CreateBy = item.Object.CreateBy,
                  CreateTime = item.Object.CreateTime,
                  Email = item.Object.Email,
                  Name = item.Object.Name,
                  Id = item.Object.Id,
                  IsDelete = (bool)item.Object.IsDelete,
                  Address = item.Object.Address,
                  LastUpdateBy = item.Object.LastUpdateBy,
                  LastUpdateTime = item.Object.LastUpdateTime,
                  Phone = item.Object.Phone,
                  Money = item.Object.Money,
                  Auth = item.Object.Auth,
                  Auth_ext = item.Object.Auth_ext,
                  Avatar = item.Object.Avatar,
                  AgencyId = item.Object.AgencyId,
                  District = item.Object.District,
                  District_ext = item.Object.District_ext,
                  Identity = item.Object.Identity,
                  IsConfirm = item.Object.IsConfirm,
                  ManageAgency = item.Object.ManageAgency,
                  Province = item.Object.Province,
                  Province_ext = item.Object.Province_ext,
                  Town = item.Object.Town,
                  Town_ext = item.Object.Town_ext
              }).ToList();
        }
        public async Task<Profiles> GetProfiles(string key)
        {
            var allPersons = await GetAllProfiles();
            return allPersons.Where(a => a.Id == key).FirstOrDefault();
        }
        public async Task<Profiles> GetProfilesAgency(string agencyId)
        {
            var allPersons = await GetAllProfiles();
            return allPersons.Where(a => a.AgencyId == agencyId).FirstOrDefault();
        }
        public ObservableCollection<Profiles> GetAllProfileWithAgency()
        {
            ObservableCollection<Profiles> profiles = new ObservableCollection<Profiles>();
            var collection = client
                .Child("Profiles")
                .AsObservable<Profiles>()
                .Subscribe((pro) =>
                {
                    if (pro.Object.AgencyId == LocalContext.Current.AccountSettings.AgencyId)
                    {
                        profiles.Add(pro.Object);
                    }
                });

            return profiles;
        }
        #endregion

        #region Product
        public ObservableCollection<Products> GetAllNewProduct()
        {
            ObservableCollection<Products> products = new ObservableCollection<Products>();
            var notification = new NotificationRequest
            {
                BadgeNumber = 1,
                Description = "Có đơn hàng mới, hãy vào App nhận đơn ngay.",
                Title = "Thông báo!",
                NotificationId = 1337,
                ReturningData = "Dummy Dâta"
            };
            var collection = client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .Subscribe((pro) =>
                {
                    if (pro.Object.Status == 1)
                    {
                        products.Add(pro.Object);
                        NotificationCenter.Current.Show(notification);
                    }
                }); 
            return products;
        }
        public ObservableCollection<Products> GetAllProductWithAgency()
        {
            var collection = client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .Subscribe((pro) =>
                {
                    if (pro.Object.AgencyId == LocalContext.Current.AccountSettings.AgencyId)
                    {
                        ProductsAgency.Remove(pro.Object);
                        ProductsAgency.Add(pro.Object);
                    }
                });
            return ProductsAgency;
        }
        public ObservableCollection<Products> GetAllProductGeted()
        {
            var collection = client
                .Child("Products")
                .AsObservable<Products>()
                .Subscribe((pro) =>
                {
                    if (pro.Object.Holder == LocalContext.Current.AccountSettings.Id)
                    {
                        ProductsShipper.Remove(pro.Object);
                        ProductsShipper.Add(pro.Object);
                    }
                });
            return ProductsShipper;
        }
        public ObservableCollection<Products> GetAllProductCreated()
        {
            ObservableCollection<Products> products = new ObservableCollection<Products>();
            var collection = client
                .Child("Products")
                .AsObservable<Products>()
                .Subscribe((pro) =>
                {
                    if (pro.Object.CreateBy == LocalContext.Current.AccountSettings.Id)
                    {
                        var index = ProductsUser.IndexOf(pro.Object);
                        ProductsUser.Remove(pro.Object);
                        ProductsUser.Add(pro.Object);
                    }
                });
            return ProductsUser;
        }
        public ObservableCollection<ProductDeliveryTrackingModel> GetDelivery(string idProduct = null)
        {
            if (string.IsNullOrEmpty(idProduct))
            {
                return null;
            }

            var products = client
                .Child("DeliveryTracking")
                .Child(idProduct)
                .AsObservable<ProductDeliveryTrackingModel>()
                .AsObservableCollection();

            return products;
        }
        #endregion

        #region Categories
        public ObservableCollection<T> GetAllCategory<T>(string collection = null)
        {
            if (string.IsNullOrEmpty(collection))
            {
                return null;
            }

            var gets = client
                .Child("Categories")
                .Child(collection)
                .OrderByKey()
                .AsObservable<T>()
                .AsObservableCollection();
            return gets;
        }

        public ObservableCollection<Category> GetListProvince()
        {
            var list = client
                .Child("Categories")
                .Child("Province")
                .AsObservable<Category>()
                .AsObservableCollection();

            return list;
        }
        public async Task<List<District>> GetListDistrict(string province = null)
        {
            try
            {
                return (await client
                .Child("Categories")
                .Child("District")
                .Child(province)
                .OnceAsync<District>())
                .Select(item => new District()
                {
                    Id = item.Object.Id,
                    Name = item.Object.Name,
                    ProvinceId = item.Object.ProvinceId,
                    ProvinceName = item.Object.ProvinceName,
                }).ToList();

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return new List<District>();
            }
        }
        public async Task<List<Town>> GetListTown(string district = null)
        {
            try
            {
                return (await client
                .Child("Categories")
                .Child("Town")
                .Child(district)
                .OnceAsync<Town>())
                .Select(item => new Town()
                {
                    Id = item.Object.Id,
                    Name = item.Object.Name,
                    ProvinceId = item.Object.ProvinceId,
                    ProvinceName = item.Object.ProvinceName,
                    DistrictId = item.Object.DistrictId,
                    DistrictName = item.Object.DistrictName,
                    FullAddress = item.Object.FullAddress
                }).ToList();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return new List<Town>();
            }

        }
        public async Task<PriceList> GetPriceList(double weight = 0, double kilometer = 0)
        {
            try
            {
                var pricelist = (await client
                .Child("Categories")
                .Child("PricesList")
                .OnceAsync<PriceList>())
                .Where(x => double.Parse(x.Object.From_Kilometer) <= kilometer && double.Parse(x.Object.To_Kilometer) >= kilometer)
                .FirstOrDefault();

                return pricelist.Object != null ? pricelist.Object : new PriceList()
                {
                    Price = "50.000"
                };
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return new PriceList();
            }

        }

        public async Task<List<PriceList>> GetPriceListWithTypeShip(string id = null)
        {
            try
            {
                var pricelist = (await client
                .Child("Categories")
                .Child("PricesList")
                .OnceAsync<PriceList>())
                .Where(item => item.Object.TypeShipProduct == id)
                .Select(item => new PriceList()
                {
                    Id = item.Object.Id,
                    From_Kilometer = item.Object.From_Kilometer,
                    From_Weight = item.Object.From_Weight,
                    TypeShipProduct = item.Object.TypeShipProduct,
                    IsDelete = item.Object.IsDelete,
                    Price = item.Object.Price,
                    To_Kilometer = item.Object.To_Kilometer,
                    To_Weight = item.Object.To_Weight,
                    TypeProduct = item.Object.TypeProduct,
                    TypeProduct_ext = item.Object.TypeProduct_ext,
                    TypeShipProduct_ext = item.Object.TypeShipProduct_ext
                })
                .ToList();
                return pricelist;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return new List<PriceList>();
            }

        }

        public async Task<List<Category>> GetTypeProduct()
        {
            try
            {
                return (await client
                .Child("Categories")
                .Child("TypeProduct")
                .OnceAsync<Category>())
                .Select(item => new Category()
                {
                    Id = item.Object.Id,
                    Name = item.Object.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return new List<Category>();
            }
        }
        public async Task<List<TypeShipProduct>> GetTypeShipProduct()
        {
            try
            {
                return (await client
                .Child("Categories")
                .Child("TypeShip")
                .OnceAsync<TypeShipProduct>())
                .Select(item => new TypeShipProduct()
                {
                    Id = item.Object.Id,
                    Name = item.Object.Name,
                    CreateTime = item.Object.CreateTime,
                    Prioritize = item.Object.Prioritize,
                    TimeHold = item.Object.TimeHold
                }).ToList();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return new List<TypeShipProduct>();
            }
        }

        public async Task<TypeShipProduct> GetOneTypeShip(string id)
        {
            try
            {
                var types = (await client
                .Child("Categories")
                .Child("TypeShip")
                .OnceAsync<TypeShipProduct>())
                .Select(item => new TypeShipProduct()
                {
                    Id = item.Object.Id,
                    CreateTime = item.Object.CreateTime,
                    Prioritize = item.Object.Prioritize,
                    TimeHold = item.Object.TimeHold,
                    Name = item.Object.Name
                }).ToList();

                return types.Where(item => item.Id == id) as TypeShipProduct;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return new TypeShipProduct();
            }
        }

        #endregion

        #region Statistical
        public async Task<int> GetStatisticalUser(int status = 0)
        {
            int sum = 0;
            var collection = (await client
                .Child("Products")
                .OrderByKey()
                .OnceAsync<Products>())
                .Select(item => new Products()
                {
                    CreateBy = item.Object.CreateBy,
                    CreateTime = item.Object.CreateTime,
                    Description = item.Object.Description,
                    From_Address = item.Object.From_Address,
                    From_FullName = item.Object.From_FullName,
                    From_PhoneNumber = item.Object.From_PhoneNumber,
                    ID = item.Object.ID,
                    IsDelete = item.Object.IsDelete,
                    LastUpdateBy = item.Object.LastUpdateBy,
                    LastUpdateTime = item.Object.LastUpdateTime,
                    Money = item.Object.Money,
                    Quanlity = item.Object.Quanlity,
                    To_Address = item.Object.To_Address,
                    To_FullName = item.Object.To_FullName,
                    To_PhoneNumber = item.Object.To_PhoneNumber,
                    Type = item.Object.Type,
                    Type_ext = item.Object.Type_ext,
                    Weight = item.Object.Weight,
                    Status = item.Object.Status,
                    Holder = item.Object.Holder,
                    IsConfirm = item.Object.IsConfirm,
                    Status_ext = item.Object.Status_ext,
                    AgencyId = item.Object.AgencyId,
                    Name = item.Object.Name
                })
                .Where(item => item.CreateBy == LocalContext.Current.AccountSettings.Id)
                .ToList();
            foreach (var item in collection)
            {
                if (item.Status == status)
                {
                    sum += 1;
                }
            }
            return sum;
        }
        public async Task<int> GetStatisticalShipper(int status = 0)
        {
            int sum = 0;
            var collection = (await client
                .Child("Products")
                .OrderByKey()
                .OnceAsync<Products>())
                .Select(item => new Products()
                {
                    CreateBy = item.Object.CreateBy,
                    CreateTime = item.Object.CreateTime,
                    Description = item.Object.Description,
                    From_Address = item.Object.From_Address,
                    From_FullName = item.Object.From_FullName,
                    From_PhoneNumber = item.Object.From_PhoneNumber,
                    ID = item.Object.ID,
                    IsDelete = item.Object.IsDelete,
                    LastUpdateBy = item.Object.LastUpdateBy,
                    LastUpdateTime = item.Object.LastUpdateTime,
                    Money = item.Object.Money,
                    Quanlity = item.Object.Quanlity,
                    To_Address = item.Object.To_Address,
                    To_FullName = item.Object.To_FullName,
                    To_PhoneNumber = item.Object.To_PhoneNumber,
                    Type = item.Object.Type,
                    Type_ext = item.Object.Type_ext,
                    Weight = item.Object.Weight,
                    Status = item.Object.Status,
                    Holder = item.Object.Holder,
                    IsConfirm = item.Object.IsConfirm,
                    Status_ext = item.Object.Status_ext,
                    AgencyId = item.Object.AgencyId,
                    Name = item.Object.Name
                })
                .Where(item => item.Holder == LocalContext.Current.AccountSettings.Id)
                .ToList();
            foreach (var item in collection)
            {
                if (item.Status == status)
                {
                    sum += 1;
                }
            }
            return sum;
        }
        public async Task<int> GetStatisticalAgency(int status = 0)
        {
            int sum = 0;
            var collection = (await client
                .Child("Products")
                .OrderByKey()
                .OnceAsync<Products>())
                .Select(item => new Products()
                {
                    CreateBy = item.Object.CreateBy,
                    CreateTime = item.Object.CreateTime,
                    Description = item.Object.Description,
                    From_Address = item.Object.From_Address,
                    From_FullName = item.Object.From_FullName,
                    From_PhoneNumber = item.Object.From_PhoneNumber,
                    ID = item.Object.ID,
                    IsDelete = item.Object.IsDelete,
                    LastUpdateBy = item.Object.LastUpdateBy,
                    LastUpdateTime = item.Object.LastUpdateTime,
                    Money = item.Object.Money,
                    Quanlity = item.Object.Quanlity,
                    To_Address = item.Object.To_Address,
                    To_FullName = item.Object.To_FullName,
                    To_PhoneNumber = item.Object.To_PhoneNumber,
                    Type = item.Object.Type,
                    Type_ext = item.Object.Type_ext,
                    Weight = item.Object.Weight,
                    Status = item.Object.Status,
                    Holder = item.Object.Holder,
                    IsConfirm = item.Object.IsConfirm,
                    Status_ext = item.Object.Status_ext,
                    AgencyId = item.Object.AgencyId,
                    Name = item.Object.Name
                })
                .Where(item => item.AgencyId == LocalContext.Current.AccountSettings.ManageAgency)
                .ToList();
            foreach (var item in collection)
            {
                if (item.Status == status)
                {
                    sum += 1;
                }
            }
            return sum;
        }
        public async Task<int> GetStatisticalAdmin(int status = 0)
        {
            int sum = 0;
            var collection = (await client
                .Child("Products")
                .OrderByKey()
                .OnceAsync<Products>())
                .Select(item => new Products()
                {
                    CreateBy = item.Object.CreateBy,
                    CreateTime = item.Object.CreateTime,
                    Description = item.Object.Description,
                    From_Address = item.Object.From_Address,
                    From_FullName = item.Object.From_FullName,
                    From_PhoneNumber = item.Object.From_PhoneNumber,
                    ID = item.Object.ID,
                    IsDelete = item.Object.IsDelete,
                    LastUpdateBy = item.Object.LastUpdateBy,
                    LastUpdateTime = item.Object.LastUpdateTime,
                    Money = item.Object.Money,
                    Quanlity = item.Object.Quanlity,
                    To_Address = item.Object.To_Address,
                    To_FullName = item.Object.To_FullName,
                    To_PhoneNumber = item.Object.To_PhoneNumber,
                    Type = item.Object.Type,
                    Type_ext = item.Object.Type_ext,
                    Weight = item.Object.Weight,
                    Status = item.Object.Status,
                    Holder = item.Object.Holder,
                    IsConfirm = item.Object.IsConfirm,
                    Status_ext = item.Object.Status_ext,
                    AgencyId = item.Object.AgencyId,
                    Name = item.Object.Name
                })
                .ToList();
            foreach (var item in collection)
            {
                if (item.Status == status)
                {
                    sum += 1;
                }
            }
            return sum;
        }
        #endregion

        #region StatisticalRevenue
        public async Task<double> TotalRevenueUser(bool total = true)
        {
            double sum = 0;
            var collection = (await client
                .Child("Products")
                .OrderByKey()
                .OnceAsync<Products>())
                .Select(item => new Products()
                {
                    CreateBy = item.Object.CreateBy,
                    CreateTime = item.Object.CreateTime,
                    Description = item.Object.Description,
                    From_Address = item.Object.From_Address,
                    From_FullName = item.Object.From_FullName,
                    From_PhoneNumber = item.Object.From_PhoneNumber,
                    ID = item.Object.ID,
                    IsDelete = item.Object.IsDelete,
                    LastUpdateBy = item.Object.LastUpdateBy,
                    LastUpdateTime = item.Object.LastUpdateTime,
                    Money = item.Object.Money,
                    Quanlity = item.Object.Quanlity,
                    To_Address = item.Object.To_Address,
                    To_FullName = item.Object.To_FullName,
                    To_PhoneNumber = item.Object.To_PhoneNumber,
                    Type = item.Object.Type,
                    Type_ext = item.Object.Type_ext,
                    Weight = item.Object.Weight,
                    Status = item.Object.Status,
                    Holder = item.Object.Holder,
                    IsConfirm = item.Object.IsConfirm,
                    Status_ext = item.Object.Status_ext,
                    AgencyId = item.Object.AgencyId,
                    Name = item.Object.Name
                })
                .Where(item => item.CreateBy == LocalContext.Current.AccountSettings.Id && item.Status >= 3)
                .ToList();
            foreach(var item in collection)
            {
                if(total)
                {
                    sum += item.Money;
                }
                else
                {
                    var month = Convert.ToDateTime(item.CreateTime);
                    if(month.Month == DateTime.Now.Month)
                    {
                        sum += item.Money;
                    }    
                }    
            }    
            return sum;
        }
        public async Task<double> TotalRevenueAgency(bool total = true)
        {
            double sum = 0;
            var collection = (await client
                .Child("Products")
                .OrderByKey()
                .OnceAsync<Products>())
                .Select(item => new Products()
                {
                    CreateBy = item.Object.CreateBy,
                    CreateTime = item.Object.CreateTime,
                    Description = item.Object.Description,
                    From_Address = item.Object.From_Address,
                    From_FullName = item.Object.From_FullName,
                    From_PhoneNumber = item.Object.From_PhoneNumber,
                    ID = item.Object.ID,
                    IsDelete = item.Object.IsDelete,
                    LastUpdateBy = item.Object.LastUpdateBy,
                    LastUpdateTime = item.Object.LastUpdateTime,
                    Money = item.Object.Money,
                    Quanlity = item.Object.Quanlity,
                    To_Address = item.Object.To_Address,
                    To_FullName = item.Object.To_FullName,
                    To_PhoneNumber = item.Object.To_PhoneNumber,
                    Type = item.Object.Type,
                    Type_ext = item.Object.Type_ext,
                    Weight = item.Object.Weight,
                    Status = item.Object.Status,
                    Holder = item.Object.Holder,
                    IsConfirm = item.Object.IsConfirm,
                    Status_ext = item.Object.Status_ext,
                    AgencyId = item.Object.AgencyId,
                    Name = item.Object.Name
                })
                .Where(item => item.AgencyId == LocalContext.Current.AccountSettings.ManageAgency && item.Status >= 3)
                .ToList();
            foreach (var item in collection)
            {
                if (total)
                {
                    sum += (item.Money * 0.05);
                }
                else
                {
                    var month = Convert.ToDateTime(item.CreateTime);
                    if (month.Month == DateTime.Now.Month)
                    {
                        sum += (item.Money * 0.05);
                    }
                }
            }
            return sum;
        }
        public async Task<double> TotalRevenueShipper(bool total = true)
        {
            double sum = 0;
            var collection = (await client
                .Child("Products")
                .OrderByKey()
                .OnceAsync<Products>())
                .Select(item => new Products()
                {
                    CreateBy = item.Object.CreateBy,
                    CreateTime = item.Object.CreateTime,
                    Description = item.Object.Description,
                    From_Address = item.Object.From_Address,
                    From_FullName = item.Object.From_FullName,
                    From_PhoneNumber = item.Object.From_PhoneNumber,
                    ID = item.Object.ID,
                    IsDelete = item.Object.IsDelete,
                    LastUpdateBy = item.Object.LastUpdateBy,
                    LastUpdateTime = item.Object.LastUpdateTime,
                    Money = item.Object.Money,
                    Quanlity = item.Object.Quanlity,
                    To_Address = item.Object.To_Address,
                    To_FullName = item.Object.To_FullName,
                    To_PhoneNumber = item.Object.To_PhoneNumber,
                    Type = item.Object.Type,
                    Type_ext = item.Object.Type_ext,
                    Weight = item.Object.Weight,
                    Status = item.Object.Status,
                    Holder = item.Object.Holder,
                    IsConfirm = item.Object.IsConfirm,
                    Status_ext = item.Object.Status_ext,
                    AgencyId = item.Object.AgencyId,
                    Name = item.Object.Name
                })
                .Where(item => item.Holder == LocalContext.Current.AccountSettings.Id && item.Status >= 3)
                .ToList();
            foreach (var item in collection)
            {
                if (total)
                {
                    sum += (item.Money * 0.80);
                }
                else
                {
                    var month = Convert.ToDateTime(item.CreateTime);
                    if (month.Month == DateTime.Now.Month)
                    {
                        sum += (item.Money * 0.80);
                    }
                }
            }
            return sum;
        }
        public async Task<double> TotalRevenueAdmin(bool total = true)
        {
            double sum = 0;
            var collection = (await client
                .Child("Products")
                .OrderByKey()
                .OnceAsync<Products>())
                .Select(item => new Products()
                {
                    CreateBy = item.Object.CreateBy,
                    CreateTime = item.Object.CreateTime,
                    Description = item.Object.Description,
                    From_Address = item.Object.From_Address,
                    From_FullName = item.Object.From_FullName,
                    From_PhoneNumber = item.Object.From_PhoneNumber,
                    ID = item.Object.ID,
                    IsDelete = item.Object.IsDelete,
                    LastUpdateBy = item.Object.LastUpdateBy,
                    LastUpdateTime = item.Object.LastUpdateTime,
                    Money = item.Object.Money,
                    Quanlity = item.Object.Quanlity,
                    To_Address = item.Object.To_Address,
                    To_FullName = item.Object.To_FullName,
                    To_PhoneNumber = item.Object.To_PhoneNumber,
                    Type = item.Object.Type,
                    Type_ext = item.Object.Type_ext,
                    Weight = item.Object.Weight,
                    Status = item.Object.Status,
                    Holder = item.Object.Holder,
                    IsConfirm = item.Object.IsConfirm,
                    Status_ext = item.Object.Status_ext,
                    AgencyId = item.Object.AgencyId,
                    Name = item.Object.Name
                })
                .Where(item => item.Status >= 3)
                .ToList();
            foreach (var item in collection)
            {
                if (total)
                {
                    sum += (item.Money * 0.15);
                }
                else
                {
                    var month = Convert.ToDateTime(item.CreateTime);
                    if (month.Month == DateTime.Now.Month)
                    {
                        sum += (item.Money * 0.15);
                    }
                }
            }
            return sum;
        }
        #endregion
        public async Task<bool> UpdateMoneyCompany(double money, bool plus = true)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                double sum = 0;
                var money_now = (await client
                    .Child("Company")
                    .OnceAsync<Company>())
                    .ToList();
                if(plus)
                {
                    sum = money + money_now.First().Object.TotalMoney;
                }    
                else
                {
                    sum = money_now.First().Object.TotalMoney - money;
                }
                if(money_now != null)
                {
                    var json = JsonConvert.SerializeObject(new Company()
                    {
                        TotalMoney = sum
                    });
                    await client
                    .Child("Company")
                    .Child("TotalMoney")
                    .PutAsync(json)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));
                }  
                return await tcs.Task;
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return await tcs.Task;
            }
        }
        public async Task<bool> UpdateMoneyUser(double money = 0, bool plus = true, string Id = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                var allPersons = await GetAllProfiles();
                var profile = allPersons.Where(a => a.Id == Id)
                    .FirstOrDefault();
                if(profile == null)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Không tìm thấy thành viên này!", "OK");
                    return await tcs.Task;
                }    

                if(plus)
                {
                    profile.Money += money;
                }    
                else
                {
                    profile.Money -= money;
                }    

                var json = JsonConvert.SerializeObject(profile);

                await client
                    .Child("Profiles")
                    .Child(Id)
                    .PutAsync(json)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));

                return await tcs.Task;
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return await tcs.Task;
            }
        }
        public async Task<bool> UpdateMoneyAgency(double money = 0, bool plus = true, string Id = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                var allPersons = await GetAllProfiles();
                var profile = allPersons.Where(a => a.ManageAgency == Id)
                    .FirstOrDefault();
                if(profile is null)
                {
                    await App.Current.MainPage.DisplayAlert("Lỗi", "Không tìm thấy quản lý Đại lý ("+Id+")!", "OK");
                    return false;
                }    
                if (plus)
                {
                    profile.Money += money;
                }
                else
                {
                    profile.Money -= money;
                }

                var json = JsonConvert.SerializeObject(profile);

                await client
                    .Child("Profiles")
                    .Child(profile.Id)
                    .PutAsync(json)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));

                return await tcs.Task;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return await tcs.Task;
            }
        }
        public ObservableCollection<TransactionHistory> GetAllNotifi()
        {
            ObservableCollection<TransactionHistory> transactionHistories = new ObservableCollection<TransactionHistory>();
            var collection = client
                .Child("Notifications")
                .OrderByKey()
                .AsObservable<TransactionHistory>()
                .Subscribe((tran) =>
                {
                    if (tran.Object.ProfileId == LocalContext.Current.AccountSettings.Id)
                    {
                        transactionHistories.Add(tran.Object);
                    }
                });
            return transactionHistories;
       }
        public async Task<List<ProductDeliveryTrackingModel>> GetDeliveryTrackingModelsAsync(string id = null)
        {
            try
            {
                List<ProductDeliveryTrackingModel> collection = new List<ProductDeliveryTrackingModel>();
                collection = (await client
                    .Child("DeliveryTracking")
                    .Child(id)
                    .OnceAsync<ProductDeliveryTrackingModel>())
                    .Select(item => new ProductDeliveryTrackingModel()
                    {
                        Date = item.Object.Date == null?"": item.Object.Date,
                        TitleStatus = item.Object.TitleStatus == null ? "" : item.Object.TitleStatus,
                        Title = item.Object.Title == null ? "" : item.Object.Title,
                        OrderDate = item.Object.OrderDate == null ? "" : item.Object.OrderDate,
                        OrderStatus = item.Object.OrderStatus == null ? "" : item.Object.OrderStatus,
                        ProgressValue = item.Object.ProgressValue,
                        Status = item.Object.Status == null ? "" : item.Object.Status,
                        StepStatus = item.Object.StepStatus
                    })
                    .ToList();
                return collection;
            }
            catch(Exception ex)
            {
                return new List<ProductDeliveryTrackingModel>();
            }
            
        }
        private void OnAuthCompleted(Task task, TaskCompletionSource<bool> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // something went wrong
                tcs.SetResult(false);
                return;
            }
            // user is logged in
            tcs.SetResult(true);
        }
     
    }
}
