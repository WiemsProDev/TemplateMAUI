using System.Diagnostics;
using System.Globalization;
using Acr.UserDialogs;
using Template.Models;
using Template.Resources;
using Template.Utils;
using Template.Models.JSON;
using Permissions = Microsoft.Maui.ApplicationModel.Permissions;
using Preferences = Microsoft.Maui.Storage.Preferences;
using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Template.Views;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;
using Template.Platforms.Source;

[assembly: ExportFont("WorkSans-Medium.ttf", Alias = "WorkSans-Medium")]
[assembly: ExportFont("WorkSans-Medium.ttf", Alias = "WorkSans-Medium")]
[assembly: ExportFont("WorkSans-Medium.ttf", Alias = "WorkSans-Medium")]
[assembly: ExportFont("WorkSans-Medium.ttf", Alias = "WorkSans-Medium")]

namespace Template;

public partial class App : Microsoft.Maui.Controls.Application
{
    public static string appId = "1";
    public static string idSesion;
    public static DateTime horaInicio;
    public static bool spanish = false;
    private static DataUtils dbUtils;
    private static ResponseWebService responseWS;
    public static IUserDialogs userdialog = UserDialogs.Instance;
    public static bool tengoConexion = false;
    private bool resultLogin = false;
    public static string appVersion = "";

    public static string UsuarioLogin = "";
    public static string PasswordLogin = "";

    public static DataUtils DAUtil
    {
        get
        {
            try
            {
                dbUtils ??= new DataUtils();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dbUtils;
        }
    }
    public static ResponseWebService ResponseWS
    {
        get
        {
            try
            {
                responseWS ??= new ResponseWebService();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseWS;
        }
    }


    public App()
    {
        try
        {

            DependencyService.Register<PathService>();
            DependencyService.Register<Localize>();
            DependencyService.Register<DeviceOrientation>();

            InitializeComponent();
            horaInicio = DateTime.Now;
            idSesion = Guid.NewGuid().ToString();
            DAUtil.ConFoto = false;
            tengoConexion = DAUtil.DoIHaveInternet();
            MainPage = new NavigationPage(new CargandoView());
        }
        catch (Exception)
        {
            userdialog.HideLoading();
        }
    }

    protected override async void OnStart()
    {
        try
        {
            try
            {
                // Current app version (2.0.0)
                string currentVersion = VersionTracking.CurrentVersion;

                // Current build (2)
                string currentBuild = VersionTracking.CurrentBuild;

                appVersion = currentVersion + $" ({currentBuild})";

               
                spanish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper().Equals("ES");

                PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
                status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationAlways>();
                }
            }
            catch (Exception)
            {
            }

            //AppCenterConfiguration();

            await InitNavigation().ContinueWith(res => MainThread.BeginInvokeOnMainThread(() =>
            {
                DAUtil.NavigationService.InitializeAsync();
            }));
        }
        catch (Exception)
        {
            await DAUtil.NavigationService.InitializeAsync();
        }
        base.OnStart();
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }


    #region Métodos
    public async Task<bool> InitNavigation()
    {
        try
        {

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            resultLogin = await IsLogin();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            userdialog.HideLoading();
            return false;
        }

        return true;

    }
    private async Task<bool> IsLogin()
    {
        try
        {
            DateTime lastLoginDate = Preferences.Get("LastLogin", DateTime.Now);
            DateTime nowDate = DateTime.Now;
            if (DAUtil.DoIHaveInternet())
            {
                if (!Preferences.Get("Usuario", "").Equals(""))
                {
                    if (await ResponseWS.Login(Preferences.Get("Usuario", ""), Preferences.Get("Pass", "")))
                    {
                        userdialog.ShowLoading(AppResources.CargandoPerfil, MaskType.Black);
                        if (await ResponseWS.LoginWithToken() == false)
                        {
                            resultLogin = false;
                            return false;
                        }
                        else
                        {
                            resultLogin = true;
                            Preferences.Set("LastLogin", DateTime.Now);
                            await RealizaCargaDatos();
                        }
                        return resultLogin;
                    }
                    else
                        return false;
                }
                else
                {
                    if ((nowDate - lastLoginDate).TotalMinutes > 7 * 24 * 60)
                    {

                        resultLogin = false;
                        return false;
                    }
                    else
                    {
                        DAUtil.Trainer = await DAUtil.GetTrainerByUsername(Preferences.Get("Usuario", ""));

                        if (DAUtil.Trainer == null)
                        {
                            resultLogin = false;
                            return false;
                        }
                        else
                        {
                            resultLogin = true;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        catch (Exception)
        {
            userdialog.Alert(AppResources.Error);
            return false;
        }
    }


    private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        try
        {
            if ((Current.MainPage.Title != null) && Current.MainPage.Title.Equals("LOGIN"))
            {
                if (DAUtil.DoIHaveInternet())
                {
                    tengoConexion = true;

                    if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        using (userdialog.Loading(AppResources.ReestableciendoConexion, null, null, true, MaskType.Black))
                        {
                            if (await IsLogin())
                            {
                                await DAUtil.NavigationService.InitializeAsync();
                            }
                        }
                    }
                    else
                    {
                        if (await IsLogin())
                        {
                            await DAUtil.NavigationService.InitializeAsync();
                        }
                    }
                }
                else
                {
                    tengoConexion = false;
                }
            }
            if (DAUtil.DoIHaveInternet())
            {
                tengoConexion = true;
            }
            else
            {
                tengoConexion = false;
            }

        }
        catch (Exception)
        {
        }
    }
    #endregion
    #region Carga Datos
    private static bool resultadoCarga = true;
    private static string mensajeCarga = "";


    private static async Task GetEntrenamientosYProgramasEasy()
    {
        try
        {
            Stopwatch watch = Stopwatch.StartNew();
            if (Preferences.Get("PrimeraCarga", false))
                userdialog.ShowLoading(AppResources.DescargandoEntrenamientos, MaskType.Black);
            else
                userdialog.ShowLoading(AppResources.VariosMinutos + Environment.NewLine + Environment.NewLine + AppResources.DescargandoEntrenamientos, MaskType.Black);
            (bool, JsonResponse) r1, r2;

            r1.Item1 = true;
            r2.Item1 = true;
            r1.Item2 = new JsonResponse();
            r2.Item2 = new JsonResponse();

            r1 = await ResponseWS.GetEntrenamientos();

            if (r1.Item1 == false)
            {
                mensajeCarga = AppResources.ErrorDescargandoEntrenamientos;
                resultadoCarga = false;
                return;
            }

            await DAUtil.ActualizaEntrenamientos(r1.Item2.entrenamientos);

            List<ProgramModel> programs = new();

            foreach (SessionModel ses in r1.Item2.entrenamientos)
            {
                foreach (TrackModel tr in ses.tracks)
                {
                    if (tr.programa != null)
                        programs.Add(tr.programa);
                }
            }


            if (programs.Count > 0)
            {
                r2 = await ResponseWS.GetProgramsByIds(programs);

                if (r2.Item1 == false)
                {
                    mensajeCarga = AppResources.ErrorDescargandoEntrenamientos;
                    resultadoCarga = false;
                    return;
                }
                await DAUtil.ActualizaProgramasAsync(r2.Item2.sessionPrograms);
            }


            watch.Stop();
            Debug.WriteLine("\nVCR-GetEntrenamientosYProgramasEasy " + watch.ElapsedMilliseconds);
        }
        catch (Exception)
        {

        }
    }
    private static async Task GetEjercicios()
    {
        try
        {
            if (Preferences.Get("PrimeraCarga", false))
                userdialog.ShowLoading(AppResources.ActualizandoEjercicios, MaskType.Black);
            else
                userdialog.ShowLoading(AppResources.VariosMinutos + Environment.NewLine + Environment.NewLine + AppResources.ActualizandoEjercicios, MaskType.Black);
            if (await ResponseWS.GetEjercicios() == false)
            {

                mensajeCarga = AppResources.ErrorDescargandoEjercicios;
                resultadoCarga = false;
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }
    private static async Task GetVideosEjercicios()
    {
        try
        {
            ResponseWS.GetVideosEjercicios();

        }
        catch (Exception)
        {

        }
    }
    private static async Task GetPostClientes()
    {
        try
        {
            Stopwatch watch = Stopwatch.StartNew();
            if (Preferences.Get("PrimeraCarga", false))
                userdialog.ShowLoading(AppResources.ActualizandoClientes, MaskType.Black);
            else
                userdialog.ShowLoading(AppResources.VariosMinutos + Environment.NewLine + Environment.NewLine + AppResources.ActualizandoClientes, MaskType.Black);

            List<ClientModel> clientList = new((await DAUtil.GetClientsFromTrainer(DAUtil.Trainer)).Where(x => x.toSend));
            if (!Preferences.Get("PrimeraCarga", false))
            {
                if (await ResponseWS.GetClientes() == false)
                {
                    mensajeCarga = AppResources.ErrorDescargandoClientes;
                    resultadoCarga = false;
                }
                //GetPlanesClientes();

                watch.Stop();
                Debug.WriteLine("\nVCR-GetClientes " + watch.ElapsedMilliseconds);

            }
        }
        catch (Exception)
        {

        }
    }

    public static async Task<string> RealizaCargaDatos()
    {
        mensajeCarga = "";
        resultadoCarga = true;
        if (Preferences.Get("PrimeraCarga", false))
        {
            Task task1 = GetPostClientes();
            await Task.WhenAll(task1);

            GetVideosEjercicios();
        }
        else
        {
            await GetEntrenamientosYProgramasEasy();
            await GetPostClientes();
            await GetEjercicios();
            if (resultadoCarga)
                Preferences.Set("PrimeraCarga", true);
            GetVideosEjercicios();
        }
        userdialog.HideLoading();
        return mensajeCarga;
    }
    #endregion

}

