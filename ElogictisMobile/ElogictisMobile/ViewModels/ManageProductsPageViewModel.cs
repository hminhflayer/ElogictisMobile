using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for contacts page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ManageProductsPageViewModel : BaseViewModel
    {
        #region Fields

        private Command<object> itemTappedCommand;
        private Command<object> addProductCommand;
        private Command<string> textChangedCommand;
        private ObservableCollection<Products> productList;
        private List<Products> products;
        private bool first = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProductsPageViewModel"/> class.
        /// </summary>
        public ManageProductsPageViewModel(INavigationService navigationService)
        {
            try
            {
                products = new List<Products>();
                ProductList = new ObservableCollection<Products>();
                _navigationService = navigationService;
                if (LocalContext.IsShipper)
                {
                    ProductList = RealtimeFirebase.Instance.GetAllProductGeted();
                }
                else if (LocalContext.IsAdmin)
                {
                    ProductList = RealtimeFirebase.Instance.GetAll<Products>("Products");   
                }
                else if(LocalContext.IsManager)
                {
                    ProductList = RealtimeFirebase.Instance.GetAllProductWithAgency();
                }
                else
                {
                    ProductList = RealtimeFirebase.Instance.GetAllProductCreated();
                }

                LocalContext.ProductsList = ProductList;
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command<object> ItemTappedCommand
        {
            get
            {
                return this.itemTappedCommand ?? (this.itemTappedCommand = new Command<object>(this.NavigateToNextPage));
            }
        }

        public Command<object> AddProductCommand
        {
            get
            {
                return this.addProductCommand ?? (this.addProductCommand = new Command<object>(this.AddProductClicked));
            }
        }
        public Command<string> TextChangedCommand
        {
            get
            {
                return this.textChangedCommand ?? (this.textChangedCommand = new Command<string>(this.SearchTextChanged));
            }
        }

        private INavigationService _navigationService;

       
        public ObservableCollection<Products> ProductList
        {
            get
            {
                return this.productList;
            }

            set
            {
                this.SetProperty(ref this.productList, value);
            }
        }
        public bool IsShipper { get; set; } = !LocalContext.IsShipper;

        #endregion

        #region Methods

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var x in e.NewItems)
            {
                // do something
                App.Current.MainPage.DisplayAlert("OK", x.ToString(), "OK");
            }
            foreach (var x in e.OldItems)
            {
                // do something
                Console.WriteLine("Old: " + x);
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                //do something
            }
        }
        /// <summary>
        /// Invoked when an item is selected from the contacts list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            // Do something
            if(selectedItem != null)
            {
                LocalContext.ProductSelected = selectedItem as Products;
                await _navigationService.NavigateToAsync<DetailProductFormPageViewModel>();
            }   
        }

        private async void AddProductClicked(object obj)
        {
            // Do something
            try
            {
                LocalContext.TmpProduct = new Products();
                await _navigationService.NavigateToAsync<MapCreateProductPageViewModel>();
                // await _navigationService.NavigateToAsync<AddProductFormPageViewModel>();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private void SearchTextChanged(string search)
        {
            // Do something
            try
            {
                ObservableCollection<Products> tmp = LocalContext.ProductsList;
                
                if(first)
                {
                    foreach(var item in tmp)
                    {
                        products.Add(item);
                    }
                    first = false;
                }

                this.ProductList.Clear();
                if (search == null || search == "")
                {
                    foreach (var item in products)
                    {
                        this.ProductList.Add(item);
                    }
                    return;
                }

                foreach (var item in products)
                {
                    if (item.ID.ToLower().Contains(search.ToLower()) || item.Name.ToLower().Contains(search.ToLower()))
                    {
                        this.ProductList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }
        #endregion
    }
}