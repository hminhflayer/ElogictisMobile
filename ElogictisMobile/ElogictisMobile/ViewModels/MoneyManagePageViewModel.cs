using ElogictisMobile.Models;
using ElogictisMobile.Services;
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
        public ValidatableObject<double> Money { get; set; }
        public int MoneyProfile { get; set; }
        public bool IsEnable { get; set; }
        public bool IsVisible { get; set; }
        public MoneyManagePageViewModel()
        {
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
                if (LocalContext.Current.AccountSettings.Money < Money.Value && LocalContext.Current.AccountSettings.Auth != "4")
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Bạn không đủ tiền trong tài khoản!", "OK");
                    return;
                }
                IsLoading = true;
                Profiles profiles = LocalContext.ProfileSelected;
                profiles.Money += Money.Value;
                profiles.LastUpdateBy = LocalContext.Current.AccountSettings.Email;
                profiles.LastUpdateTime = DateTime.Now.ToString();

                bool upt = false;
                // Do Something
                if(LocalContext.IsManager)
                {
                    Profiles manage = LocalContext.Current.AccountSettings;
                    manage.Money -= Money.Value;
                    manage.LastUpdateBy = LocalContext.Current.AccountSettings.Email;
                    manage.LastUpdateTime = DateTime.Now.ToString();
                    upt = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.ProfileSelected.Id, JsonConvert.SerializeObject(profiles));
                    if(upt)
                    {
                        LocalContext.Current.AccountSettings = manage;
                    }    
                }
                else if(LocalContext.IsAdmin)
                {
                    upt = await RealtimeFirebase.Instance.UpdateMoneyCompany(Money.Value);
                }   
                else
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Có lỗi đã xảy ra!", "OK");
                    return;
                }    
                
                var upd = await RealtimeFirebase.Instance.UpSert("Profiles", LocalContext.ProfileSelected.Id, JsonConvert.SerializeObject(profiles));
                if(upd && upt)
                {
                    IsLoading = false;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Nạp tiền thành công", "OK");
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
                if (LocalContext.ProfileSelected.Money < Money.Value)
                {
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Không đủ tiền trong tài khoản!", "OK");
                    return;
                }
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
                    LocalContext.Current.AccountSettings = profiles;
                    LocalContext.ProfileSelected.Money -= Money.Value;
                    await App.Current.MainPage.DisplayAlert("Thông báo", "Rút tiền thành công", "OK");
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
