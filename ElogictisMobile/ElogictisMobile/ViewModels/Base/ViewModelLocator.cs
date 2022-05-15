using System;
using System.Collections.Generic;
using ElogictisMobile.Views;
using ElogictisMobile.Services.Account;
using ElogictisMobile.Services.Navigation;
using TinyIoC;
using Xamarin.Forms;

namespace ElogictisMobile.ViewModels.Base
{
    public class ViewModelLocator
    {
        static TinyIoCContainer _container;
        static Dictionary<Type, Type> _lookupTable;

        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();
            _lookupTable = new Dictionary<Type, Type>();

            // Register Page and Page Models

            Register<DashboardPageViewModel, DashboardPage>();
            Register<HomePageViewModel, HomePage>();

            Register<LoginPageViewModel, LoginPage>();
            Register<SignUpPageViewModel, SignUpPage>();
            Register<ForgotPasswordPageViewModel, ForgotPasswordPage>();
            Register<ResetPasswordPageViewModel, ResetPasswordPage>();
            Register<AlertSignUpPageViewModel, AlertSignUpPage>();

            Register<StatisticalPageViewModel, StatisticalPage>();
            Register<TransactionHistoryPageViewModel, TransactionHistoryPage>();
            Register<SettingsPageViewModel, SettingsPage>();

            Register<MapCreateProductPageViewModel, MapCreateProductPage>();
            Register<MapExtendPageViewModel, MapExtendPage>();

            Register<AddProfilesPageViewModel, AddProfilesPage>();
            Register<AddProductFormPageViewModel, AddProductFormPage>();
            Register<AddCategoryTypeProductPageViewModel, AddCategoryTypeProductPage>();
            Register<AddCategoryPriceListPageViewModel, AddCategoryPriceListPage>();
            Register<AddAgencyPageViewModel, AddAgencyPage>();
            Register<AddProvincePageViewModel, AddProvincePage>();
            Register<AddDistrictPageViewModel, AddDistrictPage>();
            Register<AddTownPageViewModel, AddTownPage>();
            Register<AddCategoryTypeShipperPageViewModel, AddCategoryTypeShipperPage>();

            Register<UpdateProfilePageViewModel, UpdateProfilePage>();

            Register<ManageProfilesPageViewModel, ManageProfilesPage>();
            Register<ManageProductsPageViewModel, ManageProductsPage>();
            Register<CategoryTypeProductPageViewModel, CategoryTypeProductPage>();
            Register<CategoryPriceListPageViewModel, CategoryPriceListPage>();
            Register<ManageCategoryTypeShipperPageViewModel, ManageCategoryTypeShipperPage>();
            Register<ManageProductNewPageViewModel, ManageProductNewPage>();
            Register<ManageAgencyPageViewModel, ManageAgencyPage>();
            Register<ManageProvincePageViewModel, ManageProvincePage>();
            Register<ManageDistrictPageViewModel, ManageDistrictPage>();
            Register<ManageTownPageViewModel, ManageTownPage>();
            Register<HistoryTransactionPageViewModel, HistoryTransactionPage>();
            Register<DetailRevenuePageViewModel, DetailRevenuePage>();
            Register<HistoryOrderPageViewModel, HistoryOrderPage>();

            Register<DetailProfilePageViewModel, DetailProfilePage>();
            Register<DetailProductFormPageViewModel, DetailProductFormPage>();
            Register<DetailCategoryPricesListPageViewModel, DetailCategoryPricesListPage>();
            Register<DetailCategoryTypeProductPageViewModel, DetailCategoryTypeProductPage>();
            Register<DetailProductNewPageViewModel, DetailProductNewPage>();
            Register<DetailAgencyPageViewModel, DetailAgencyPage>();
            Register<FailedProductPageViewModel, FailedProductPage>();

            Register<MoneyManagePageViewModel, MoneyManage>();
            Register<ProductDeliveryTrackingPageViewModel, ProductDeliveryTrackingPage>();
            // Register Services (registered as Singletons by default)
            _container.Register<INavigationService, NavigationService>();
            _container.Register<IAccountService>(DependencyService.Get<IAccountService>());
        }

        /// <summary>
        /// Private utility method to Register page and page model for page retrieval by it's
        /// specified page model type.
        /// </summary>
        /// <typeparam name="TPageModel"></typeparam>
        /// <typeparam name="TPage"></typeparam>
        static void Register<TViewModel, TView>() where TViewModel : BaseViewModel where TView : Page
        {
            _lookupTable.Add(typeof(TViewModel), typeof(TView));
            _container.Register<TViewModel>();
        }

        public static T Resolve<T>() where T : class
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (TinyIoCResolutionException e)
            {
                var message = e.Message;
                System.Diagnostics.Debug.WriteLine(e.Message);

                while (e.InnerException is TinyIoCResolutionException ex)
                {
                    message = ex.Message;
                    System.Diagnostics.Debug.WriteLine("\t" + ex.Message);
                    e = ex;
                }
#if DEBUG
                App.Current.MainPage.DisplayAlert("Resolution Error", message, "Ok");
#endif
            }
            return default(T);
        }


        public static Page CreatePageFor<TViewModelType>() where TViewModelType : BaseViewModel
        {
            Type viewModelType = typeof(TViewModelType);
            var viewType = _lookupTable[viewModelType];
            var view = (Page)Activator.CreateInstance(viewType);
            var viewModel = Resolve<TViewModelType>();
            view.BindingContext = viewModel;
            return view;
        }
    }
}
