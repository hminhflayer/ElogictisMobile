using Akavache;
using ElogictisMobile.Models;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.ViewModels;
using ElogictisMobile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reactive.Linq;
using Xamarin.Forms.Xaml;
using System.IO;
using Newtonsoft.Json;
using ElogictisMobile.Services;

[assembly: ExportFont("Montserrat-Bold.ttf",Alias="Montserrat-Bold")]
     [assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
     [assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Montserrat-Regular")]
     [assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "Montserrat-SemiBold")]
     [assembly: ExportFont("UIFontIcons.ttf", Alias = "FontIcons")]
namespace ElogictisMobile
{
    public partial class App : Application
    {
        public static string ImageServerPath { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTk3NTI3QDMxMzkyZTM0MmUzMG1MVER4STVubHNKVkRxczEvRDRxSXlPLyt5My9hODNUazN0dXlTZktYVFU9");
            Akavache.Registrations.Start("KimiElogistics");
            InitializeComponent();
        }

        private Task InitNavigation()
        {
            LocalContext.ProvinceList = RealtimeFirebase.Instance.GetListProvince();
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            
            if (LocalContext.Current.AccountSettings != null)
            {
                if (LocalContext.Current.AccountSettings.Auth != null)
                {
                    LocalContext.IsAdmin = LocalContext.Current.GetPermission(4);
                    LocalContext.IsManager = LocalContext.Current.GetPermission(3);
                    LocalContext.IsShipper = LocalContext.Current.GetPermission(2);
                }
                return navigationService.NavigateToAsync<DashboardPageViewModel>();
            }
            return navigationService.NavigateToAsync<LoginPageViewModel>(null, true);
        }
        protected override async void OnStart()
        {
            base.OnStart();
            await InitNavigation();   
            base.OnResume();
        }

        protected override void OnSleep()
        {
            BlobCache.Shutdown().Wait();
        }
    }
}
