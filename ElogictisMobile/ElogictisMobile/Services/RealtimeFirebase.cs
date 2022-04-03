using ElogictisMobile.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElogictisMobile.Services
{
    public class RealtimeFirebase
    {
        FirebaseClient client;
        private static RealtimeFirebase instance;

        public static RealtimeFirebase Instance
        {
            get { if (instance == null) instance = new RealtimeFirebase(); return RealtimeFirebase.instance; }
            private set { RealtimeFirebase.instance = value; }
        }

        public RealtimeFirebase()
        {
            client = new FirebaseClient("https://elogictismobile-default-rtdb.firebaseio.com/");
        }

        public Task<bool> UpSert(string collections = null, string key = null,string data = null)
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
            catch(Exception ex)
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
            if(string.IsNullOrEmpty(collection))
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

        //Method for Profiles
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
                  Avatar = item.Object.Avatar
              }).ToList();
        }
        public async Task<Profiles> GetProfiles(string key)
        {
            
            var allPersons = await GetAllProfiles();
            return allPersons.Where(a => a.Id == key).FirstOrDefault();
        }

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
        //Method for Notifi
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

        public ObservableCollection<Profiles> GetAllProfileWithAgency()
        {
            ObservableCollection<Profiles> profiles = new ObservableCollection<Profiles>();
            var collection = client
                .Child("Products")
                .OrderByKey()
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
        //Status product
        public ObservableCollection<Products> GetProductWithStatus(int status = 0)
        {
            if (status <= 0 || status > 4)
            {
                return null;
            }

            var product = client
                .Child("Products")
                .AsObservable<Products>()
                .AsObservableCollection()
                .Where<Products>(a => a.Status == status && a.CreateBy == LocalContext.Current.AccountSettings.Email);
            
            return (ObservableCollection<Products>)product;
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

        public ObservableCollection<Products> GetAllNewProduct()
        {
            ObservableCollection<Products> products = new ObservableCollection<Products>();
            var collection = client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .Subscribe((pro) =>
                {
                    if(pro.Object.Status == 1)
                    {
                        products.Add(pro.Object);
                    }
                });
            return products;
        }
        public ObservableCollection<Products> GetAllProductWithAgency()
        {
            ObservableCollection<Products> products = new ObservableCollection<Products>();
            var collection = client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .Subscribe((pro) =>
                {
                    if (pro.Object.AgencyId == LocalContext.Current.AccountSettings.Id)
                    {
                        products.Add(pro.Object);
                    }
                });
            return products;
        }

        public ObservableCollection<Products> GetAllProductGeted()
        {
            ObservableCollection<Products> products = new ObservableCollection<Products>();
            var collection = client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .Subscribe((pro) =>
                {
                    if (pro.Object.Holder == LocalContext.Current.AccountSettings.Id)
                    {
                        products.Add(pro.Object);
                    }
                });
            return products;
        }

        public ObservableCollection<Products> GetAllProductCreated()
        {
            ObservableCollection<Products> products = new ObservableCollection<Products>();
            var collection = client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .Subscribe((pro) =>
                {
                    if (pro.Object.CreateBy == LocalContext.Current.AccountSettings.Id)
                    {
                        products.Add(pro.Object);
                    }
                });
            return products;
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
            catch(Exception ex)
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
            catch(Exception ex)
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

                return pricelist.Object;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
                return new PriceList();
            }

        }
    }
}
