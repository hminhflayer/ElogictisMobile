using ElogictisMobile.Models;
using Firebase.Database;
using Firebase.Database.Query;
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

                    return tcs.Task; ;
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

        //Method for Notifi
        public ObservableCollection<TransactionHistory> GetAllNotifi()
        {

            var allNoti = client
              .Child("Notifications")
              .OrderBy("Time")
              .AsObservable<TransactionHistory>()
              .AsObservableCollection();

            return allNoti.Where(a => a.Email == LocalContext.Current.AccountSettings.Email) as ObservableCollection<TransactionHistory>;
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
            var list = (client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .AsObservableCollection())
                .Where<Products>(x => x.Status == 1);
            if(list.Count() == 0)
            {
                return new ObservableCollection<Products>();
            }    
            return (ObservableCollection<Products>)list;
        }

        public ObservableCollection<Products> GetAllProductGeted()
        {
            var list = (client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .AsObservableCollection())
                .Where<Products>(x => x.Holder == LocalContext.Current.AccountSettings.Id);

            return (ObservableCollection<Products>)list;
        }

        public ObservableCollection<Products> GetAllProductCreated()
        {
            var list = (client
                .Child("Products")
                .OrderByKey()
                .AsObservable<Products>()
                .AsObservableCollection())
                .Where<Products>(x => x.CreateBy == LocalContext.Current.AccountSettings.Id);

            return (ObservableCollection<Products>)list;
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

        public ObservableCollection<District> GetListDistrict(string category = null)
        {
            var list = client
                .Child("Categories")
                .Child("District")
                .Child(category)
                .AsObservable<District>()
                .AsObservableCollection();

            return list;
        }

        public ObservableCollection<Town> GetListTown(string district = null)
        {
            var list = client
                .Child("Categories")
                .Child("Town")
                .Child(district)
                .AsObservable<Town>()
                .AsObservableCollection();

            return list;
        }

    }
}
