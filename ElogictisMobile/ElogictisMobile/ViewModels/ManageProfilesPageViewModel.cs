using ElogictisMobile.Models;
using ElogictisMobile.Services;
using ElogictisMobile.Services.Navigation;
using ElogictisMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.ViewModels
{
    /// <summary>
    /// ViewModel for contacts page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ManageProfilesPageViewModel : BaseViewModel
    {
        #region Fields

        private Command<object> itemTappedCommand;

        private Command<object> addProfileCommand;
        private Command<string> textChangedCommand;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ManageProfilesPageViewModel"/> class.
        /// </summary>
        public ManageProfilesPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            if(LocalContext.IsAdmin)
            {
                ProfilesList = RealtimeFirebase.Instance.GetAll<Profiles>("Profiles");
            }
            else
            {
                ProfilesList = RealtimeFirebase.Instance.GetAllProfileWithAgency();
            }
            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command<object> ItemTappedCommand
        {
            get
            {
                return this.itemTappedCommand ?? (this.itemTappedCommand = new Command<object>(this.NavigateToNextPage));
            }
        }

        public Command<object> AddProfileCommand
        {
            get
            {
                return this.addProfileCommand ?? (this.addProfileCommand = new Command<object>(this.AddProfileClicked));
            }
        }
        public Command<string> TextChangedCommand
        {
            get
            {
                return this.textChangedCommand ?? (this.textChangedCommand = new Command<string>(this.SearchTextChanged));
            }
        }

        /// <summary>
        /// Gets or sets a collction of value to be displayed in contacts list page.
        /// </summary>
        [DataMember(Name = "contactsPageList")]
        public ObservableCollection<Contact> ContactList { get; set; }

        private INavigationService _navigationService;

        public ObservableCollection<Profiles> ProfilesList { get; set; }
        public Profiles SelectedProfile { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Invoked when an item is selected from the contacts list.
        /// </summary>
        /// <param name="selectedItem">Selected item from the list view.</param>
        private async void NavigateToNextPage(object selectedItem)
        {
            // Do something
            LocalContext.ProfileSelected = selectedItem as Profiles;
            await _navigationService.NavigateToAsync<DetailProfilePageViewModel>();
        }

        private async void AddProfileClicked(object obj)
        {
            // Do something
            try
            {
                await _navigationService.NavigateToAsync<AddProfilesPageViewModel>();
            }    
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }
        private void SearchTextChanged(string search)
        {
            // Do something
            try
            {
                ObservableCollection<Profiles> tmp = LocalContext.ProfilesList;
                if (search == null || search == "")
                {
                    foreach (var item in tmp)
                    {
                        ProfilesList.Add(item);
                    }
                    return;
                }

                this.ProfilesList.Clear();
                foreach (var item in tmp)
                {
                    if (item.Name.ToLower().Contains(search.ToLower()))
                    {
                        ProfilesList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Thông báo", ex.Message, "OK");
            }
        }
        #endregion
    }
}