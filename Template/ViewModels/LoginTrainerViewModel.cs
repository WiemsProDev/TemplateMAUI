
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using Template.Resources;
using Template.ViewModels.Base;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using CommunityToolkit.Mvvm.Input;
using Template.Interfaces;

namespace Template.ViewModels
{
    public partial class LoginTrainerViewModel : ViewModelBase
    {
        #region Propiedades

        public bool resultLogin = false;

        public string mensajeUsuario = string.Empty;

        [ObservableProperty]
        private string usuario;

        [ObservableProperty]
        private string passWord;

        [ObservableProperty]
        private string imagenPass = "candado.png";

        [ObservableProperty]
        private bool esPwd = true;

        [ObservableProperty]
        private bool recordarme = true;

        #endregion
        public LoginTrainerViewModel()
        {
            try
            {
                Usuario = Preferences.Get("Usuario", "");
                PassWord = Preferences.Get("Pass", "");
                Recordarme = Preferences.Get("Recordarme", true);

                if (App.userdialog == null)
                    App.userdialog = UserDialogs.Instance;
            }
            catch (Exception ex)
            {
                
            }
        }
        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                //DependencyService.Get<IDeviceOrientationService>().SetDeviceOrientation(DisplayOrientation.Portrait);
                DependencyService.Get<IDeviceOrientationService>().SetDeviceOrientation(DisplayOrientation.Portrait);
                var ser = DependencyService.Get<IDeviceOrientationService>();
                DependencyService.Get<IDeviceOrientationService>().SetDeviceOrientation(DisplayOrientation.Portrait);
                Spanish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper().Equals("ES");

            }
            catch (Exception ex)
            {
                App.userdialog.HideLoading();
                App.userdialog.Alert(AppResources.Error);
                
            }
            finally
            {
                App.userdialog.HideLoading();
            }
            return base.InitializeAsync(navigationData).ContinueWith((task) => { App.userdialog.HideLoading(); });

        }

        #region Comandos
        public ICommand VerPwd { get { return new Command(ExeVerPwd); } }
        public IAsyncRelayCommand BtnLogin => new AsyncRelayCommand(async () => await Login());
        #endregion

        #region Métodos
        private void ExeVerPwd()
        {
            EsPwd = !EsPwd;
            if (EsPwd)
                ImagenPass = "candado.png";
            else
                ImagenPass = "desbloquear.png";
        }
        public async Task Login()
        {
            try
            {

                await initLogin();
                if (resultLogin)
                {
                    Preferences.Set("Recordarme", Recordarme);

                    Preferences.Set("RUsuario", Usuario);
                    Preferences.Set("RPass", PassWord);

                    if (Recordarme)
                    {
                        Preferences.Set("Usuario", Usuario);
                        Preferences.Set("Pass", PassWord);

                    }
                    else
                    {
                        Preferences.Set("Usuario", "");
                        Preferences.Set("Pass", "");

                    }


                    App.userdialog.HideLoading();
                    //NavigationService.NavigateToAsync<HomeViewModel>();
                    await App.DAUtil.NavigationService.InitializeAsync();

                }
                else
                {
                    App.DAUtil.Trainer = null;
                    App.userdialog.HideLoading();
                    await App.userdialog.AlertAsync(mensajeUsuario, "ERROR", AppResources.Aceptar);
                }
            }
            catch (Exception ex)
            {
                
                App.userdialog.HideLoading();
                await App.userdialog.AlertAsync(AppResources.Error, "ERROR", AppResources.Aceptar);
            }
            finally
            {
                App.userdialog.HideLoading();
            }
        }
        private async Task initLogin()
        {
            try
            {
                DateTime lastLoginDate = Preferences.Get("LastLogin", DateTime.Now);
                DateTime nowDate = DateTime.Now;

                if (App.tengoConexion)
                {

                    if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(PassWord))
                    {
                        mensajeUsuario = AppResources.Usuario + AppResources.Y + AppResources.Password + " " + AppResources.Obligatorios;
                        resultLogin = false;
                    }
                    else
                    {
                        try
                        {
                            resultLogin = true; //lo ponemos al pruncipio, para que si cualquier llamada en paralelo lo pone a false, devuelva el error de login

                            var watch = Stopwatch.StartNew();

                            Debug.WriteLine("\nVCR-Start");
                            App.userdialog.ShowLoading(AppResources.Accediendo, MaskType.Black);
                            if (await App.ResponseWS.LoginTrainer(Usuario, PassWord) == false)
                            {
                                mensajeUsuario = AppResources.ErrorLogin;
                                resultLogin = false;
                                return;
                            }
                            watch.Stop();
                            Debug.WriteLine("\nVCR-Login " + watch.ElapsedMilliseconds);
                            watch = Stopwatch.StartNew();

                            App.userdialog.ShowLoading(AppResources.CargandoPerfil, MaskType.Black);
                            if (await App.ResponseWS.LoginWithToken() == false)
                            {
                                mensajeUsuario = AppResources.ErrorCargandoPerfil;
                                resultLogin = false;
                                return;
                            }
                            watch.Stop();
                            Debug.WriteLine("\nVCR-Token " + watch.ElapsedMilliseconds);

                            Preferences.Set("LastLogin", DateTime.Now);
                            Preferences.Set("PrimeraCarga", false);
                            mensajeUsuario =await App.RealizaCargaDatos();
                            if (!mensajeUsuario.Equals(""))
                            {
                                resultLogin = false;
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            App.userdialog.HideLoading();
                            resultLogin = false;
                        }
                        finally
                        {
                             //App.ResponseWS.sendMailSQL();
                            App.userdialog.HideLoading();
                        }
                    }
                }
                else
                {

                    if ((nowDate - lastLoginDate).TotalMinutes > 7 * 24 * 60)
                    {
                        mensajeUsuario = AppResources.ErrorInternetLong;
                        resultLogin = false;
                    }
                    else
                    {
                        App.DAUtil.Trainer = await App.DAUtil.GetTrainerByUsername(Preferences.Get("Usuario", ""));

                        if (App.DAUtil.Trainer == null)
                        {
                            mensajeUsuario = AppResources.ErrorInternet;
                            resultLogin = false;
                        }
                        else
                        {
                            resultLogin = true;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                mensajeUsuario = AppResources.Error;
                resultLogin = false;
                
            }
            finally
            {
                App.userdialog.HideLoading();
            }
        }
        #endregion


    }
}