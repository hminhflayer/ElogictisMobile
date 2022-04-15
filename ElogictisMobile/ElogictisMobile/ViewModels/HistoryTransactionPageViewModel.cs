using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ElogictisMobile.ViewModels
{
    public class HistoryTransactionPageViewModel : BaseViewModel
    {
        #region Constructor
        private INavigationService _navigationService;
        private string selectedDate;
        private ObservableCollection<HistoryTransaction> historyTransactions;

        public HistoryTransactionPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            HistoryTransactions = RealtimeFirebase.Instance.GetAll<HistoryTransaction>("Transaction");
            this.DateCommand = new Command(this.DateSelected);
        }

        #endregion

        #region Properties

        public string SelectedDate
        {
            get
            {
                return this.selectedDate;
            }

            set
            {
                if (this.selectedDate == value)
                {
                    return;
                }

                this.SetProperty(ref this.selectedDate, value);
            }
        }

        public ObservableCollection<HistoryTransaction> HistoryTransactions
        {
            get
            {
                return this.historyTransactions;
            }

            set
            {
                if (this.historyTransactions == value)
                {
                    return;
                }

                this.SetProperty(ref this.historyTransactions, value);
            }
        }

        #endregion 

        #region Comments

        public Command DateCommand { get; set; }

        #endregion

        #region Methods


        private async void DateSelected(object obj)
        {
            // Do something
            try
            {
                
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        #endregion
    }
}
