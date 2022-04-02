using ElogictisMobile.Models;
using ElogictisMobile.Services;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel of transaction history template.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class TransactionHistoryPageViewModel : BaseViewModel
    {
        #region fields
        public ObservableCollection<TransactionHistory> ContactList { get; set; }
        #endregion

        #region Constructor

        public TransactionHistoryPageViewModel()
        {
            ContactList = RealtimeFirebase.Instance.GetAllNotifi();
        }

        #endregion

        #region Properties

        
        #endregion

        #region Methods

 
        #endregion
    }
}
