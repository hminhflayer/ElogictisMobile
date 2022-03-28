using Akavache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElogictisMobile.Models
{
    public class LocalContext
    {
        public static Profiles Profiles { get { return Current.AccountSettings; } set { Current.AccountSettings = value; } }
        public static ObservableCollection<Profiles> ProfilesList { get; set; }
        public static ObservableCollection<Products> ProductsList { get; set; }
        public static ObservableCollection<Category> TypeProductList { get; set; }
        public static Products ProductSelected { get; set; }
        public static Profiles ProfileSelected { get; set; }
        public static Category CategorySelected { get; set; }
        public static PriceList PriceListSelected { get; set; }

        #region Singleton
        private static LocalContext _current;
        public static LocalContext Current => _current ?? (_current = new LocalContext());
        private readonly IBlobCache _userCache = BlobCache.UserAccount;
        private readonly ISecureBlobCache _secureCache = BlobCache.Secure;
        private LocalContext()
        {
        }
        #endregion

        #region Private methods

        async void AddOrUpdateValue<T>(string key, T val)
        {
            await _userCache.InsertObject(key, val);
        }

        T GetValueOrDefault<T>(string key, T defaultValue)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var obj = await _userCache.GetObject<T>(key);
                    return obj;
                }
                catch (KeyNotFoundException)
                {
                    await _userCache.InsertObject(key, defaultValue);
                    return defaultValue;
                }
            }).Result;
        }

        async void AddOrUpdateValueSecure<T>(string key, T val)
        {
            await _secureCache.InsertObject(key, val);
        }

        T GetValueOrDefaultSecure<T>(string key, T defaultValue)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var obj = await _secureCache.GetObject<T>(key);
                    return obj;
                }
                catch (KeyNotFoundException)
                {
                    await _secureCache.InsertObject(key, defaultValue);
                    return defaultValue;
                }
            }).Result;
        }
        #endregion

        private const string AccountSettingsKey = "AccountKey";
        public Profiles AccountSettings
        {
            get => GetValueOrDefaultSecure(AccountSettingsKey, new Profiles());
            set => AddOrUpdateValueSecure(AccountSettingsKey, value);
        }

    }
}
