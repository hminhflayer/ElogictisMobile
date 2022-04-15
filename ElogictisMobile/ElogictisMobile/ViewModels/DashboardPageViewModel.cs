﻿using ElogictisMobile.Models;
using ElogictisMobile.Services;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ElogictisMobile.ViewModels
{
    public class DashboardPageViewModel : BaseViewModel
    {
        private StatisticalPageViewModel _statisticalVM;
        private TransactionHistoryPageViewModel _historyVM;
        private ManageProductsPageViewModel _manageVM;
        private SettingsPageViewModel _settingVM;

        public SettingsPageViewModel SettingsPageViewModel
        {
            get => _settingVM;
            set => SetProperty(ref _settingVM, value);
        }

        public StatisticalPageViewModel StatisticalPageViewModel
        {
            get => _statisticalVM;
            set => SetProperty(ref _statisticalVM, value);
        }

        public TransactionHistoryPageViewModel TransactionHistoryPageViewModel
        {
            get => _historyVM;
            set => SetProperty(ref _historyVM, value);
        }

        public ManageProductsPageViewModel ManageProductsPageViewModel
        {
            get => _manageVM;
            set => SetProperty(ref _manageVM, value);
        }

        public DashboardPageViewModel(StatisticalPageViewModel statisticalVM,
            TransactionHistoryPageViewModel historyVM,
            ManageProductsPageViewModel manageVM,
            SettingsPageViewModel settingVM)
        {
            StatisticalPageViewModel = statisticalVM;
            TransactionHistoryPageViewModel = historyVM;
            ManageProductsPageViewModel = manageVM;
            SettingsPageViewModel = settingVM;

            //Load Profiles
            if (LocalContext.IsAdmin)
            {
                LocalContext.ProfilesList = RealtimeFirebase.Instance.GetAll<Profiles>("Profiles");
            }
            else
            {
                LocalContext.ProfilesList = RealtimeFirebase.Instance.GetAllProfileWithAgency();
            }

            if(LocalContext.IsShipper)
            {
                ObservableCollection<Products> products = RealtimeFirebase.Instance.GetAllNewProduct();
            }    

            //Load Products, Agency, PriceList
            if (LocalContext.IsAdmin)
            {
                LocalContext.AgencyList = RealtimeFirebase.Instance.GetAll<Agency>("Agencies");
                LocalContext.PriceLists = RealtimeFirebase.Instance.GetAllCategory<PriceList>("PricesList");
                LocalContext.TypeProductList = RealtimeFirebase.Instance.GetAllCategory<Category>("TypeProduct");
            }

        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.WhenAny(base.InitializeAsync(navigationData),
                StatisticalPageViewModel.InitializeAsync(null),
                TransactionHistoryPageViewModel.InitializeAsync(null),
                ManageProductsPageViewModel.InitializeAsync(null),
                SettingsPageViewModel.InitializeAsync(null));
        }
    }
}
