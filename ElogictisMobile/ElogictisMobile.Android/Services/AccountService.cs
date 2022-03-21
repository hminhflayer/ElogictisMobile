﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ElogictisMobile.Droid.Services;
using ElogictisMobile.Models;
using ElogictisMobile.Services.Account;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AccountService))]
namespace ElogictisMobile.Droid.Services
{
    public class AccountService : IAccountService
    {

        public AccountService()
        {
        }

        public Task<bool> LoginAsync(string email, string password)
        {
            var tcs = new TaskCompletionSource<bool>();
            FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password)
                .ContinueWith((task) => OnAuthCompleted(task, tcs));

            return tcs.Task;
        }

        private void OnAuthCompleted(Task task, TaskCompletionSource<bool> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // something went wrong
                tcs.SetResult(false);
                return;
            }
            // user is logged in
            tcs.SetResult(true);
        }


        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdTokenAsync(false);
                LocalContext.Current.AccountSettings.ExpireToken = DateTime.Now.Add(new TimeSpan(0,0,0, 3600));
                LocalContext.Current.AccountSettings.Token = token.Token;

                return token.Token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsSignIn()
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> SignUpAsync(string email, string password)
        {
            //now we use the staic method in FirebaseAuth to get an instance of FirebaseAuth
            var authResult = await FirebaseAuth.Instance?.CreateUserWithEmailAndPasswordAsync(email, password);
            using (var user = authResult.User)
            using (var actionCode = ActionCodeSettings.NewBuilder().SetAndroidPackageName("com.caominh.elogictismobile", true, "0").Build())
            {
                await user.SendEmailVerificationAsync(actionCode);
            }
            var token = authResult.User.Uid;

            return token;
        }

        public string GetUidLogin()
        {
            if(IsSignIn())
            {
                return FirebaseAuth.Instance.CurrentUser.Uid;
            }
            return "";
        }

        public bool CheckEmailVerified()
        {
            return FirebaseAuth.Instance.CurrentUser.IsEmailVerified;
        }

        public async Task SendEmailVerified()
        {
            using (var actionCode = ActionCodeSettings.NewBuilder().SetAndroidPackageName("com.caominh.elogictismobile", true, "0").Build())
            {
                await FirebaseAuth.Instance.CurrentUser.SendEmailVerificationAsync(actionCode);
            }
        }

        public async Task<bool> ChangePasswordAsync(string password)
        {
            await FirebaseAuth.Instance.CurrentUser.UpdatePasswordAsync(password);
            return true;
        }
    }
}