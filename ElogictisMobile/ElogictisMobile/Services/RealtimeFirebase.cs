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

        public async Task UpSert(string collections = null, string key = null,string data = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(collections)
                    && !string.IsNullOrEmpty(key) 
                    && !string.IsNullOrEmpty(data))
                {
                    await client
                    .Child(collections)
                    .Child(key)
                    .PutAsync(data);
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
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
                .AsObservable<T>()
                .AsObservableCollection();
            return profiles;
        }

        public async Task Delete(string collections = null, string key = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(collections)
                    && !string.IsNullOrEmpty(key))
                {
                    await client
                    .Child(collections)
                    .Child(key)
                    .DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        //Method for Profiles
        public async Task<List<Profiles>> GetAllProfiles()
        {

            return (await client
              .Child("Profiles")
              .OnceAsync<Profiles>()).Select(item => new Profiles
              {
                  Profile_CreateBy = item.Object.Profile_CreateBy,
                  Profile_CreateTime = item.Object.Profile_CreateTime,
                  Profile_Email = item.Object.Profile_Email,
                  Profile_Name = item.Object.Profile_Name,
                  Profile_Id = item.Object.Profile_Id,
                  Profile_IsDelete = (bool)item.Object.Profile_IsDelete,
                  Profile_Address = item.Object.Profile_Address,
                  Profile_LastUpdateBy = item.Object.Profile_LastUpdateBy,
                  Profile_LastUpdateTime = item.Object.Profile_LastUpdateTime,
                  Profile_Phone = item.Object.Profile_Phone
              }).ToList();
        }
        public async Task<Profiles> GetProfiles(string key)
        {
            var allPersons = await GetAllProfiles();
            return allPersons.Where(a => a.Profile_Id == key).FirstOrDefault();
        }


        //Method for Notifi
        public ObservableCollection<TransactionHistory> GetAllNotifi()
        {

            var allNoti = client
              .Child("Notifications")
              .AsObservable<TransactionHistory>()
              .AsObservableCollection().ToList();

            return allNoti.Where(a => a.Email == LocalContext.Current.AccountSettings.Profile_Email) as ObservableCollection<TransactionHistory>;
        }
    }
}
