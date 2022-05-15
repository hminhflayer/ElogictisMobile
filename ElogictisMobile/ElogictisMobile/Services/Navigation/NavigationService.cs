using System.Threading.Tasks;
using ElogictisMobile.ViewModels;
using ElogictisMobile.ViewModels.Base;
using Xamarin.Forms;

namespace ElogictisMobile.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public Task GoBackRootAsync()
        {
            if (App.Current.MainPage is TabbedPage tabbedPage)
            {
                if (tabbedPage.CurrentPage is NavigationPage nPage)
                {
                    return nPage.PopToRootAsync();
                }
            }
            return Task.CompletedTask;
        }
        public Task GoBackAsync()
        {
            if (App.Current.MainPage is TabbedPage tabbedPage)
            {
                if (tabbedPage.CurrentPage is NavigationPage nPage)
                {
                    return nPage.PopAsync();
                }
            }
            return Task.CompletedTask;
        }

        public async Task NavigateToAsync<TViewModel>(object navigationData = null, bool setRoot = false)
            where TViewModel : BaseViewModel
        {
            Page page = ViewModelLocator.CreatePageFor<TViewModel>();

            if (setRoot)
            {
                if (page is TabbedPage tabbedPage)
                {
                    App.Current.MainPage = tabbedPage;
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(page);
                }
            }
            else
            {
                if (page is TabbedPage tabPage)
                {
                    App.Current.MainPage = tabPage;
                }
                else if (App.Current.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
                else if (App.Current.MainPage is TabbedPage tabbedPage)
                {
                    if (tabbedPage.CurrentPage is NavigationPage nPage)
                    {
                        await nPage.PushAsync(page);
                    }
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(page);
                }
            }

            if (page.BindingContext is BaseViewModel pmBase)
            {
                await pmBase.InitializeAsync(navigationData);
            }
        }
    }
}
