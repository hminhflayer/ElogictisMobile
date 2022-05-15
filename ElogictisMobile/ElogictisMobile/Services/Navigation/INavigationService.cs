using System.Threading.Tasks;
using ElogictisMobile.ViewModels;

namespace ElogictisMobile.Services.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Navigation method to asynchonously navigate between Page Models,
        /// and optionally pass navigation Data.
        /// </summary>
        /// <typeparam name="TViewPage"></typeparam>
        /// <param name="navigationData"></param>
        /// <returns></returns>
        Task NavigateToAsync<TViewModel>(object navigationData = null, bool setRoot = false)
            where TViewModel : BaseViewModel;

        /// <summary>
        /// Pop navigation backstack
        /// </summary>
        /// <returns></returns>
        Task GoBackAsync();
        Task GoBackRootAsync();
    }
}
