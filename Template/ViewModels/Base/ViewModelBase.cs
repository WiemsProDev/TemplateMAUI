using Template.Interfaces;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Template.Resources;
using System;
using CommunityToolkit.Mvvm.Input;

namespace Template.ViewModels.Base
{
    public partial class ViewModelBase : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        internal readonly INavigationService NavigationService;

        [ObservableProperty]
        private double opacity;
        [ObservableProperty]
        private bool spanish;

        public ViewModelBase()
        {
            NavigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
        }

        public virtual async Task InitializeAsync(object navigationData)
        {
           
        }
        public virtual Task FinalizePage(object navigationData)
        {
            return Task.FromResult(false);
        }
        public virtual void FinalizePage()
        {

        }
    }
}
