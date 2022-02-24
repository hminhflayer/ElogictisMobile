using ElogictisMobile.PageModels;
using ElogictisMobile.PageModels.Base;
using ElogictisMobile.Services.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ElogictisMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        private Task InitNavigation()
        {
            var navigationService = PageModelLocator.Resolve<INavigationService>();
            return navigationService.NavigateToAsync<LoginPageModel>(null, true);
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await InitNavigation();
            base.OnResume();
        }

        protected override void OnSleep()
        {
        }
    }
}
