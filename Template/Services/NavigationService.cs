using Template.Interfaces;
using Template.ViewModels;
using Template.ViewModels.Base;
using Template.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Devices;

namespace Template.Services
{
    public class NavigationService : INavigationService
    {
        public readonly Dictionary<Type, Type> _mappings;

        private NavigationPage navigationPage;

        private MainPage mainPage;

        internal Application CurrentApplication
        {
            get
            {
                return Application.Current;
            }
        }

        public NavigationService()
        {
            _mappings = new Dictionary<Type, Type>();
            CreatePageViewModelMappings();
        }

        public Task InitializeAsync()
        {

            return NavigateToAsync<MainPageViewModel>();
        }

        public async Task NavigateBackAsync()
        {
            try
            {
                if (CurrentApplication.MainPage != null)
                {
                    var mainPage = CurrentApplication.MainPage as MainPage;
                    var navigationPage = mainPage.Detail as NavigationPage;
                    if (navigationPage != null)
                    {

                        await navigationPage.PopAsync(true);
                    }
                    else
                    {
                        //Podria poner throw 
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), null, true);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), parameter, true);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return NavigateToAsync(viewModelType, null, true);
        }

        public Task NavigateToAsyncMenu<TViewModel>() where TViewModel : ViewModelBase
        {
            //App.userdialog.Loading(AppResources.Cargando, null, null, true, MaskType.Black);

            return NavigateToAsyncMenu(typeof(TViewModel), null, true);
        }

        public Task NavigateToAsyncMenu<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return NavigateToAsyncMenu(typeof(TViewModel), parameter, true);
        }

        public Task NavigateToAsyncMenu(Type viewModelType)
        {
            return NavigateToAsyncMenu(viewModelType, null, true);
        }

        Task INavigationService.NavigateToAsyncMenu(Type viewModelType, object parameter)
        {
            return NavigateToAsyncMenu(viewModelType, parameter, true);
        }

        protected virtual async Task NavigateToAsync(Type viewModelType, object parameter, bool visibleMenu)
        {
            //App.userdialog.ShowLoading(AppResources.Cargando, Acr.UserDialogs.MaskType.Black);
            Page page = CreateAndBindPage(viewModelType, parameter);

            if (page is MainPage)
            {
                CurrentApplication.MainPage = page;
            }
            else if (CurrentApplication.MainPage is MainPage)
            {

                mainPage = CurrentApplication.MainPage as MainPage;
                mainPage.IsGestureEnabled = visibleMenu;
                navigationPage = mainPage.Detail as NavigationPage;

                if (navigationPage != null)
                {
                    if (!App.DAUtil.ConFoto && !App.DAUtil.ConWeb)
                    {

                        navigationPage.BarTextColor = Color.FromArgb("#ffffff");
                        navigationPage.BarBackgroundColor = Color.FromArgb("#444444");
                        await navigationPage.PushAsync(page);
                    }
                    else
                        App.DAUtil.ConFoto = false;

                }
                else
                {
                    navigationPage = new NavigationPage(page);
                    navigationPage.BarTextColor = Color.FromArgb("#ffffff");
                    navigationPage.BarBackgroundColor = Color.FromArgb("#444444");
                    mainPage.Detail = navigationPage;
                }

                /* if (mainPage.Detail is NavigationPage navigationPage)
                 {

                     await navigationPage.PushAsync(page).ConfigureAwait(false);
                 }
                 else
                 {
                     navigationPage = new NavigationPage(page);
                     mainPage.Detail = navigationPage;
                 }*/
                try
                {
                    if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS)
                        mainPage.IsPresented = false;
                    else if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android)
                        mainPage.IsPresented = false;
                    else if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.WinUI)
                        mainPage.IsPresented = false;
                    else
                        mainPage.IsPresented = true;

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            else
            {

                if (CurrentApplication.MainPage is NavigationPage navigationPage)
                {
                    navigationPage.BarTextColor = Color.FromArgb("#ffffff");
                    navigationPage.BarBackgroundColor = Color.FromArgb("#444444");
                    await navigationPage.PushAsync(page).ConfigureAwait(false);
                }
                else
                {

                    CurrentApplication.MainPage = new NavigationPage(page);
                }
            }
        }

        public Task NavigateToAsyncWithoutMenuMenu<TViewModel>() where TViewModel : ViewModelBase
        {
            //App.userdialog.Loading(AppResources.Cargando, null, null, true, MaskType.Black);

            return NavigateToAsyncMenu(typeof(TViewModel), null, false);
        }

        public Task NavigateToAsyncWithoutMenuMenu<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return NavigateToAsyncMenu(typeof(TViewModel), parameter, false);
        }

        public Task NavigateToAsyncWithoutMenuMenu(Type viewModelType)
        {
            return NavigateToAsyncMenu(viewModelType, null, false);
        }

        Task INavigationService.NavigateToAsyncWithoutMenuMenu(Type viewModelType, object parameter)
        {
            return NavigateToAsyncMenu(viewModelType, parameter, false);
        }

        protected virtual async Task NavigateToAsyncMenu(Type viewModelType, object parameter, bool visibleMenu)
        {
            try
            {
                //App.userdialog.ShowLoading(AppResources.Cargando, Acr.UserDialogs.MaskType.Black);
                Page page = CreateAndBindPage(viewModelType, parameter);
                mainPage = CurrentApplication.MainPage as MainPage;
                if (mainPage == null)
                    mainPage = new MainPage();
                mainPage.IsGestureEnabled = visibleMenu;
                navigationPage = mainPage.Detail as NavigationPage;
                navigationPage = new NavigationPage(page);
                navigationPage.BarTextColor = Color.FromArgb("#ffffff");
                navigationPage.BackgroundColor = Color.FromArgb("#444444");
                mainPage.Detail = navigationPage;


                try
                {

                    if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS)
                        mainPage.IsPresented = false;
                    else if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android)
                        mainPage.IsPresented = false;
                    else if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.WinUI)
                        mainPage.IsPresented = false;
                    else
                        mainPage.IsPresented = true;

                }
                catch (Exception ex)
                {

                    Console.Write(ex.Message);
                }
                //App.userdialog.HideLoading();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }
            return _mappings[viewModelType];
        }

        private Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            try
            {

                Type pageType = GetPageTypeForViewModel(viewModelType);

                if (pageType == null)
                {
                    throw new Exception($"Mapping type for {viewModelType} is not a page");
                }

                Page page = Activator.CreateInstance(pageType) as Page;
                ViewModelBase viewModel = ViewModelLocator.Instance.Resolve(viewModelType) as ViewModelBase;
                Debug.WriteLine(string.Format("********* http://View --> {0} http://ViewModel --> {1}   Hora: {2} *********", page.GetType().Name, viewModel.GetType().Name, DateTime.Now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture)));
                page.BindingContext = viewModel;
                page.Appearing += async (object sender, EventArgs e) =>
                {
                    await viewModel.InitializeAsync(parameter).ConfigureAwait(true);
                };
                return page;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public Task InsertPageBefore(Type viewModelType)
        {
            mainPage = CurrentApplication.MainPage as MainPage;
            return NavigateToAsync(viewModelType, null, true);
        }

        public void InsertPageBefore<T>()
        {
            Page page = CreateAndBindPage(typeof(T), null);
            CurrentApplication.MainPage = new NavigationPage(page);
        }

        private void CreatePageViewModelMappings()
        {
            _mappings.Add(typeof(HomeViewModel), typeof(HomeView));
            _mappings.Add(typeof(MenuLateralViewModel), typeof(MenuLateral));
            _mappings.Add(typeof(MainPageViewModel), typeof(MainPage));
            _mappings.Add(typeof(LoginTrainerViewModel), typeof(LoginTrainerView));
        }

        public Task NavigateToAsyncWithoutMenu<TViewModel>() where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), null, false);
        }

        public Task NavigateToAsyncWithoutMenu<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), parameter, false);
        }

        Task INavigationService.NavigateToAsyncWithoutMenu(Type viewModelType)
        {
            return NavigateToAsync(viewModelType, null, false);
        }
    }
}