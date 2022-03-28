using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.Models
{
    /// <summary>
    /// Model for transaction history template.
    /// </summary>
    public class TransactionHistory
    {
        #region Fields

        public string IdProduct { get; set; }

        public string TransactionDescription { get; set; }

        public string Email { get; set; }

        public string Time { get; set; }

        #endregion
    }
}
