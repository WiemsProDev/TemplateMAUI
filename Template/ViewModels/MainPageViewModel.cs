using System.Globalization;
using System.Threading.Tasks;
using Template.ViewModels.Base;

namespace Template.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private MenuLateralViewModel _menuLateralViewModel;

        public MainPageViewModel(MenuLateralViewModel menuLateralViewModel)
        {
            _menuLateralViewModel = menuLateralViewModel;

        }

        public MenuLateralViewModel MenuViewModel
        {
            get
            {
                return _menuLateralViewModel;
            }

            set
            {
                _menuLateralViewModel = value;
                OnPropertyChanged(nameof(MenuLateralViewModel));
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            
            int rol = 0;
            if (App.DAUtil.Trainer != null)
                rol = 1;
            Spanish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper().Equals("ES");
            Task result;
            switch (rol)
            {
                case 1: //Trainer
                    result = Task.WhenAll(_menuLateralViewModel.InitializeAsync(navigationData), NavigationService.NavigateToAsync<HomeViewModel>());
                    break;
                default:
                    //result = Task.WhenAll(_menuLateralViewModel.InitializeAsync(navigationData), NavigationService.NavigateToAsync<PreLoginViewModel>());
                    result = Task.WhenAll(_menuLateralViewModel.InitializeAsync(navigationData), NavigationService.NavigateToAsyncWithoutMenu<LoginTrainerViewModel>());
                    break;
            }
            return result;
        }
    }
}
