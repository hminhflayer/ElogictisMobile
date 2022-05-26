using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Validators;
using ElogictisMobile.Validators.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ElogictisMobile.ViewModels
{
    public class MoneyManagePageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;

        public ValidatableObject<double> Money { get; set; }
        public long MoneyProfile { get; set; }
        public bool IsEnable { get; set; }
        public bool IsVisible { get; set; }
        public MoneyManagePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.Money = new ValidatableObject<double>();
            this.Money.Validations.Add(new IsNotNullOrEmptyRule<double> { ValidationMessage = "Số tiền không được trống" });
            this.DepositCommand = new Command(this.DepositClicked);
            this.WithdrawCommand = new Command(this.WithdrawClicked);
            if(LocalContext.Current.AccountSettings.Id == LocalContext.ProfileSelected.Id && LocalContext.Current.AccountSettings.Auth == "3")
            {
                IsVisible = true;
            }    
        }

        public Command DepositCommand { get; set; }
        public Command WithdrawCommand { get; set; }

        private async void DepositClicked(object obj)
        {
            try
            {
                List<Profiles> newProfiles = new List<Profiles>();
                List<Profiles> oldProfiles = new List<Profiles>();
                oldProfiles.Add(LocalContext.ProfileSelected);
                oldProfiles.Add(LocalContext.Current.AccountSettings);
                if(Money.Value <= 0)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Số tiền phải lớn hơn 0!", "OK");
                    return;
                }    
                if (LocalContext.IsAdmin == false)
                {
                    if (LocalContext.Current.AccountSettings.Money < Money.Value && LocalContext.Current.AccountSettings.Auth != "4")
                    {
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn không đủ tiền trong tài khoản!", "OK");
                        return;
                    }
                }    
                
                IsLoading = true;
                bool updateUser = false;
                bool updateAgency = false;
                bool updateAdmin = false;

                Profiles profiles = LocalContext.ProfileSelected;
                profiles.Money += Money.Value;
                profiles.LastUpdateBy = LocalContext.Current.AccountSettings.Email;
                profiles.LastUpdateTime = DateTime.Now.ToString();

                string idTransaction = GeneralKey.Instance.General("TRANSACTION");
                HistoryTransaction historyTransaction = new HistoryTransaction
                {
                    Id = idTransaction,
                    CreateBy = LocalContext.Current.AccountSettings.Id,
                    CreateTime = DateTime.Now.ToShortDateString(),
                    TranferId = LocalContext.Current.AccountSettings.Id,
                    TranferName = LocalContext.Current.AccountSettings.Name,
                    TranferAuth = LocalContext.Current.AccountSettings.Auth_ext,
                    ReceiverId = LocalContext.ProfileSelected.Id,
                    ReceiverName = LocalContext.ProfileSelected.Name,
                    ReceiverAuth = LocalContext.ProfileSelected.Auth_ext,
                    Money = Money.Value,
                    Status = "NẠP TIỀN"
                };
                // Do Something
                if (LocalContext.IsManager)
                {
                    Profiles manage = LocalContext.Current.AccountSettings;
                    manage.Money -= Money.Value;
                    manage.LastUpdateBy = LocalContext.Current.AccountSettings.Email;
                    manage.LastUpdateTime = DateTime.Now.ToString();
                    updateUser = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.ProfileSelected.Id, JsonConvert.SerializeObject(profiles));
                    updateAgency = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.Current.AccountSettings.Id, JsonConvert.SerializeObject(manage));
                    if (updateUser && updateAgency)
                    {
                        IsLoading = false;

                        await RealtimeFirebase.Instance.UpSert("Transaction", historyTransaction.Id, JsonConvert.SerializeObject(historyTransaction));
                        LocalContext.Current.AccountSettings = manage;
                        newProfiles.Add(profiles);
                        newProfiles.Add(manage);

                        await App.Current.MainPage.DisplayAlert("Thông báo", "Nạp tiền thành công", "OK");
                        await _navigationService.GoBackRootAsync();
                    }    
                }
                else if(LocalContext.IsAdmin)
                {
                    updateAdmin = await RealtimeFirebase.Instance.UpdateMoneyCompany(Money.Value);
                    updateUser = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.ProfileSelected.Id, JsonConvert.SerializeObject(profiles));
                    if (updateAdmin && updateUser)
                    {
                        IsLoading = false;
                        await RealtimeFirebase.Instance.UpSert("Transaction", historyTransaction.Id, JsonConvert.SerializeObject(historyTransaction));
                        await App.Current.MainPage.DisplayAlert("Thông báo", "Nạp tiền thành công", "OK");
                        await _navigationService.GoBackRootAsync();
                    }
                }   
                else
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Có lỗi đã xảy ra!", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }

        private async void WithdrawClicked(object obj)
        {
            try
            {
                if (Money.Value <= 0)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Số tiền phải lớn hơn 0!", "OK");
                    return;
                }
                if (LocalContext.ProfileSelected.Money < Money.Value)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Không đủ tiền trong tài khoản!", "OK");
                    return;
                }

                string idTransaction = GeneralKey.Instance.General("TRANSACTION");
                HistoryTransaction historyTransaction = new HistoryTransaction
                {
                    Id = idTransaction,
                    CreateBy = LocalContext.Current.AccountSettings.Id,
                    CreateTime = DateTime.Now.ToShortDateString(),
                    TranferId = LocalContext.Current.AccountSettings.Id,
                    TranferName = LocalContext.Current.AccountSettings.Name,
                    TranferAuth = LocalContext.Current.AccountSettings.Auth_ext,
                    ReceiverId = LocalContext.ProfileSelected.Id,
                    ReceiverName = LocalContext.ProfileSelected.Name,
                    ReceiverAuth = LocalContext.ProfileSelected.Auth_ext,
                    Money = Money.Value,
                    Status = "RÚT TIỀN"
                };


                IsLoading = true;
                Profiles profiles = LocalContext.ProfileSelected;
                profiles.Money -= Money.Value;
                profiles.LastUpdateBy = LocalContext.Current.AccountSettings.Email;
                profiles.LastUpdateTime = DateTime.Now.ToString();
                // Do Something

                bool upt = await RealtimeFirebase.Instance.UpdateMoneyCompany(Money.Value, false);
                var upd = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.ProfileSelected.Id, JsonConvert.SerializeObject(profiles));
                if (upd && upt)
                {
                    IsLoading = false;
                    await RealtimeFirebase.Instance.UpSert("Transaction", historyTransaction.Id, JsonConvert.SerializeObject(historyTransaction));
                    LocalContext.ProfileSelected = profiles;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Rút tiền thành công", "OK");
                    await _navigationService.GoBackAsync();
                }
                else
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Rút tiền không thành công", "OK");
                }    
            }
            catch (Exception ex)
            {
                IsLoading = false;
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }
    }
}
