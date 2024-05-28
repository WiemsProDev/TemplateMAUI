
using System;
using Template.Interfaces;
using Template.Services;
using Unity;
using Unity.Lifetime;

namespace Template.ViewModels.Base
{
    internal class ViewModelLocator
    {
        readonly IUnityContainer _unityContainer;
        private static readonly ViewModelLocator _intance = new ViewModelLocator();

        public static ViewModelLocator Instance
        {
            get
            {
                return _intance;
            }
        }

        public ViewModelLocator()
        {
            _unityContainer = new UnityContainer();
            // ViewModels
            _unityContainer.RegisterType<HomeViewModel>();
            _unityContainer.RegisterType<MainPageViewModel>();
            _unityContainer.RegisterType<MenuLateralViewModel>();
            _unityContainer.RegisterType<LoginTrainerViewModel>();

            // Services
            _unityContainer.RegisterType<INavigationService, NavigationService>();

            

        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>();
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
