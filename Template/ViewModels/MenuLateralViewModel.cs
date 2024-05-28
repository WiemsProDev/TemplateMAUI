using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Template.Models;
using Template.Resources;
using Template.ViewModels.Base;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using CommunityToolkit.Mvvm.Input;

namespace Template.ViewModels
{
    public class MenuLateralViewModel : ViewModelBase
    {
        List<MenuModel> menus;
        bool cs = false;
        private ObservableCollection<SideMenuItemModel> listaMenuByRol;

        // public MasterDetailPage masterDetail { get; set; }

        public ObservableCollection<SideMenuItemModel> ListaMenuByRol
        {
            get { return listaMenuByRol; }
            set
            {
                if (listaMenuByRol != value)
                {
                    listaMenuByRol = value;
                    OnPropertyChanged(nameof(ListaMenuByRol));
                }
            }
        }
        public ICommand PerfilCommand { get { return new Command(IrAPerfil); } }

        void IrAPerfil()
        {

        }
        private string version;
        public string Version
        {
            get { return version; }
            set
            {
                if (version != value)
                {
                    version = value;
                    OnPropertyChanged(nameof(Version));
                }
            }
        }
        
        private String nombre;
        public String Nombre
        {
            get { return nombre; }
            set
            {
                if (nombre != value)
                {
                    nombre = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }
        public IAsyncRelayCommand<SideMenuItemModel> ItemSelected => new AsyncRelayCommand<SideMenuItemModel>(async (parametro) => await Seleccionado(parametro));
        async Task Seleccionado(object param)
        {
            SideMenuItemModel item = (SideMenuItemModel)param;
            foreach (SideMenuItemModel s in ListaMenuByRol)
                s.Selec = false;
            item.Selec = true;
            try
            {
                if (!item.TieneHijos)
                {
                    if (item != null)
                    {
                        if (item.Title.Equals(AppResources.CerrarSesion))
                        {
                            App.DAUtil.Trainer = null;
                            Preferences.Set("Usuario", "");
                            Preferences.Set("Pass", "");
                            //App.DAUtil.Entrenador = null;
                            NavigationService.InitializeAsync();
                        }
                        else
                        {
                            NavigationService.NavigateToAsyncMenu(item.TargetType, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public MenuLateralViewModel()
        {

        }

        public async Task<ObservableCollection<SideMenuItemModel>> GetListByRol(int rol)
        {
            try
            {
                ObservableCollection<SideMenuItemModel> menuItems;
                if (App.DAUtil.listadoOpcionesPorRol == null || App.DAUtil.listadoOpcionesPorRol.Count == 0)
                {
                    if (App.DAUtil.listadoOpcionesPorRol == null)
                        App.DAUtil.listadoOpcionesPorRol = new List<SideMenuItemModel>();
                    else
                        App.DAUtil.listadoOpcionesPorRol.Clear();

                    menuItems = new ObservableCollection<SideMenuItemModel>();
                    menus = new List<MenuModel>(await App.DAUtil.GetMenu(rol));
                    string nombreMenu = "";
                    /*MenuModel m4 = new MenuModel()
                    {
                        easy = true,
                        id = 999,
                        idParent = 0,
                        imagen = "emg",
                        nombre = "EMG",
                        nombre_ingles = "EMG",
                        orden = 7,
                        rol = 1,
                        viewmodel = "WiemsProEasy.ViewModels.EMGViewModel",
                        visible = true
                    };
                    menus.Add(m4);*/
                    //foreach (MenuModel m in menus.Where(p => p.visible == true && p.easy == !App.esPRO))
                    foreach (MenuModel m in menus.OrderBy(p=>p.orden).Where(p => p.visible == true))
                    {
                        nombreMenu = App.spanish ? m.nombre : m.nombre_ingles;
                        if (m.idParent == 0)
                        {
                            List<MenuModel> hijos = menus.Where(p => p.idParent == m.id).ToList();
                            if (hijos == null)
                                hijos = new List<MenuModel>();
                            SideMenuItemModel ml = (new SideMenuItemModel
                            {
                                Title = nombreMenu,
                                IconSource = m.imagen,
                                TargetType = Type.GetType(m.viewmodel),
                                TieneHijos = hijos.Count > 0,
                                Hijos = new List<SideMenuItemModel>()
                            });
                            foreach (MenuModel m2 in hijos.Where(p => p.visible == true))
                            {
                                nombreMenu = App.spanish ? m2.nombre : m2.nombre_ingles;
                                ml.Hijos.Add(new SideMenuItemModel
                                {
                                    Title = nombreMenu,
                                    IconSource = m2.imagen,
                                    TargetType = Type.GetType(m2.viewmodel),
                                    TieneHijos = false
                                });
                            }
                            menuItems.Add(ml);
                        }
                    }
                    foreach (var item in menuItems)
                    {
                        App.DAUtil.listadoOpcionesPorRol.Add(item);
                    }
                }
                else
                    menuItems = new ObservableCollection<SideMenuItemModel>(App.DAUtil.listadoOpcionesPorRol);
                return menuItems;
            }
            catch (Exception ex)
            {
                
                return new ObservableCollection<SideMenuItemModel>(App.DAUtil.listadoOpcionesPorRol);
            }
        }

        public async override Task InitializeAsync(object navigationData)
        {
            Spanish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper().Equals("ES");
            // Current app version (2.0.0)
            var currentVersion = VersionTracking.CurrentVersion;
            // Current build (2)
            var currentBuild = VersionTracking.CurrentBuild;
            Version = currentVersion + " (" + currentBuild + ")";
            ClientModel Usuario = App.DAUtil.Client;
            try
            {
                if (App.DAUtil.Trainer != null)
                {
                    ListaMenuByRol = await GetListByRol(1);
                    ListaMenuByRol.First(p => p.Title.Equals(AppResources.Clientes)).Selec=true;
                    Nombre = App.DAUtil.Trainer.name;
                }
                else
                {
                    ListaMenuByRol = await GetListByRol(0);
                    Nombre = "";
                }
            }
            catch (Exception ex)
            {
                
            }
            await base.InitializeAsync(navigationData).ContinueWith((task) => { });
        }
    }
}
