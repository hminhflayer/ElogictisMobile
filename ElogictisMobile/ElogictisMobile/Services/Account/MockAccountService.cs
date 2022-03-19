using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElogictisMobile.Services.Account
{
    public class MockAccountService : IAccountService
    {

        public Task<bool> ChangePasswordAsync(string password)
        {
            throw new NotImplementedException();
        }

        public bool CheckEmailVerified()
        {
            throw new NotImplementedException();
        }

        public Task<double> GetCurrentPayRateAsync()
        {
            return Task.FromResult(10.0);
        }

        public string GetUidLogin()
        {
            throw new NotImplementedException();
        }

        public bool IsSignIn()
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Task.FromResult(false);
            }
            return Task.Delay(1000).ContinueWith((task) => true);
        }

        public Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailVerified()
        {
            throw new NotImplementedException();
        }

        public bool SignOut()
        {
            throw new NotImplementedException();
        }

        public Task<string> SignUpAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
