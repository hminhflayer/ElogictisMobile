using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ElogictisMobile.PageModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ElogictisMobile.PageModels
{
    public class DashboardPageModel : PageModelBase
    {
        private ProfilePageModel _profilePM;
        private StatisticalPageModel _statisticalPM;
        private ManagePageModel _managePM;
        private HistoryPageModel _historyPM;

        public ProfilePageModel ProfilePageModel
        {
            get => _profilePM;
            set => SetProperty(ref _profilePM, value);
        }

        public StatisticalPageModel StatisticalPageModel
        {
            get => _statisticalPM;
            set => SetProperty(ref _statisticalPM, value);
        }

        public HistoryPageModel HistoryPageModel
        {
            get => _historyPM;
            set => SetProperty(ref _historyPM, value);
        }

        public ManagePageModel ManagePageModel
        {
            get => _managePM;
            set => SetProperty(ref _managePM, value);
        }

        public DashboardPageModel(ProfilePageModel profilePM,
            StatisticalPageModel statisticalPM,
            HistoryPageModel historyPM,
            ManagePageModel managePM)
        {
            ProfilePageModel = profilePM;
            StatisticalPageModel = statisticalPM;
            HistoryPageModel = historyPM;
            ManagePageModel = managePM;
        }
        
        public override Task InitializeAsync(object navigationData)
        {
            return Task.WhenAny(base.InitializeAsync(navigationData),
                ProfilePageModel.InitializeAsync(null),
                HistoryPageModel.InitializeAsync(null),
                ManagePageModel.InitializeAsync(null),
                StatisticalPageModel.InitializeAsync(null));
        }
    }
}
