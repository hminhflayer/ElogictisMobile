using System.Threading.Tasks;

namespace ElogictisMobile.ViewModels
{
    public class DashboardPageViewModel : BaseViewModel
    {
        private StatisticalPageViewModel _statisticalVM;
        private TransactionHistoryPageViewModel _historyVM;
        private ManageItemPageViewModel _manageVM;
        private SettingsPageViewModel _settingVM;

        public SettingsPageViewModel SettingsPageViewModel
        {
            get => _settingVM;
            set => SetProperty(ref _settingVM, value);
        }

        public StatisticalPageViewModel StatisticalPageViewModel
        {
            get => _statisticalVM;
            set => SetProperty(ref _statisticalVM, value);
        }

        public TransactionHistoryPageViewModel TransactionHistoryPageViewModel
        {
            get => _historyVM;
            set => SetProperty(ref _historyVM, value);
        }

        public ManageItemPageViewModel ManageItemPageViewModel
        {
            get => _manageVM;
            set => SetProperty(ref _manageVM, value);
        }

        public DashboardPageViewModel(StatisticalPageViewModel statisticalVM,
            TransactionHistoryPageViewModel historyVM,
            ManageItemPageViewModel manageVM,
            SettingsPageViewModel settingVM)
        {
            StatisticalPageViewModel = statisticalVM;
            TransactionHistoryPageViewModel = historyVM;
            ManageItemPageViewModel = manageVM;
            SettingsPageViewModel = settingVM;;
        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.WhenAny(base.InitializeAsync(navigationData),
                StatisticalPageViewModel.InitializeAsync(null),
                TransactionHistoryPageViewModel.InitializeAsync(null),
                ManageItemPageViewModel.InitializeAsync(null),
                SettingsPageViewModel.InitializeAsync(null));
        }
    }
}
