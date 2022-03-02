using ElogictisMobile.PageModels.Base;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.ViewModels.Buttons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ElogictisMobile.PageModels
{
    public class ManagePageModel : PageModelBase
    {
        private ICommand _addProduct_Tapped;
        private INavigationService _navigationService;
        public ButtonModel AddProductModel { get; set; }

        public ICommand AddProduct_Tapped
        {
            get => _addProduct_Tapped;
        }

        public ManagePageModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            AddProductModel = new ButtonModel("Thêm Đơn", GoToProduct);

        }
        public async void GoToProduct()
        {
            await _navigationService.NavigateToAsync<ProductPageModel>();
        }
    }
}
