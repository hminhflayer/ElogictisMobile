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
        public static ObservableCollection<Category> ProvinceList { get; set; }
        public static ObservableCollection<Agency> AgencyList { get; set; }
        public static ObservableCollection<PriceList> PriceLists { get; set; }
        public static ObservableCollection<HealthCare> ProductStatistical { get; set; }
        public static Products ProductSelected { get; set; }
        public static Profiles ProfileSelected { get; set; }
        public static Category CategorySelected { get; set; }
        public static PriceList PriceListSelected { get; set; }
        public static Agency AgencySelected { get; set; }
        public static Category ProvinceSelected { get; set; }
        public static District DistrictSelected { get; set; }
        public static Town TownSelected { get; set; }
        public static TypeShipProduct TypeShipProductSelected { get; set; }
        public static bool IsManager { get; set; }
        public static bool IsShipper { get; set; }
        public static bool IsAdmin { get; set; }
        public static bool IsEdit { get; set; }
        public static string AdminId { get; private set; } = "2xxO3k41qcQrviWdPWnQnJJ1b1z2";
        public static List<District> Districts { get; set; }
        public static List<Town> Towns { get; set; }
        public static List<Category> ListTypeProduct { get; set; }
        public static List<TypeShipProduct> ListTypeShipProductCollection { get; set; }
        public static List<Profiles> ListProfiles { get; set; } = new List<Profiles>();

        public static bool IsFromAddress { get; set; }
        public static Products TmpProduct { get; set; } = new Products();

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

        public bool GetPermission(int per)
        {
            switch (per)
            {
                case 2:
                    {
                        return int.Parse(LocalContext.Current.AccountSettings.Auth) == 2;
                    }
                case 3:
                    {
                        return int.Parse(LocalContext.Current.AccountSettings.Auth) == 3;
                    }
                case 4:
                    {
                        return int.Parse(LocalContext.Current.AccountSettings.Auth) == 4;
                    }
            }

            return false;
        }

    }
}
