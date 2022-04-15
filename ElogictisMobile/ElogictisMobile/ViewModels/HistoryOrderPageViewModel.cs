using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ElogictisMobile.ViewModels
{
    public class HistoryOrderPageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<DetailRevenue> observableCollection;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProductsPageViewModel"/> class.
        /// </summary>
        public HistoryOrderPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.ObservableCollection = RealtimeFirebase.Instance.GetAll<DetailRevenue>("HistoryOrder/" + LocalContext.Current.AccountSettings.Id);
        }

        #endregion

        #region Properties

        private INavigationService _navigationService;

        public ObservableCollection<DetailRevenue> ObservableCollection
        {
            get
            {
                return this.observableCollection;
            }

            set
            {
                if(value == null)
                {
                    return;
                }
                this.SetProperty(ref this.observableCollection, value);
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
