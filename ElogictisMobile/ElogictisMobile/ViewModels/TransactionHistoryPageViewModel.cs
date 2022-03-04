using ElogictisMobile.Models;
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

        private static TransactionHistoryPageViewModel transactionHistoryViewModel;

        private Command itemSelectedCommand;

        #endregion

        #region Constructor

        public TransactionHistoryPageViewModel()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of transaction History page view model.
        /// </summary>
        public static TransactionHistoryPageViewModel BindingContext =>
            transactionHistoryViewModel = PopulateData<TransactionHistoryPageViewModel>("ecommerce.json");

        /// <summary>
        /// Gets or sets the employee details.
        /// </summary>
        /// <value>The employee details.</value>
        [DataMember(Name = "transactionDetails")]
        public ObservableCollection<TransactionHistory> TransactionDetails { get; set; }

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand
        {
            get
            {
                return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates the data for view model from json file.
        /// </summary>
        /// <typeparam name="T">Type of view model.</typeparam>
        /// <param name="fileName">Json file to fetch data.</param>
        /// <returns>Returns the view model object.</returns>
        private static T PopulateData<T>(string fileName)
        {
            var file = "ElogictisMobile.Data." + fileName;

            var assembly = typeof(App).GetTypeInfo().Assembly;

            T data;

            using (var stream = assembly.GetManifestResourceStream(file))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                data = (T)serializer.ReadObject(stream);
            }

            return data;
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        private void ItemSelected(object selectedItem)
        {
            // Do something
        }

        #endregion
    }
}
