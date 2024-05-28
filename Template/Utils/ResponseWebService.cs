using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using Template.Models;
using Template.Models.JSON;
using Template.Resources;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System;
using Microsoft.Maui.Devices;

namespace Template.Utils
{
    public class ResponseWebService
    {
        #region Propiedades
        private HttpResponseMessage response;
        private static string miLocale = App.spanish ? "es" : "en";

        public string miURL = "https://testws.wiemspro.com/wiemspro/";
        private string miURLwCommerce = "https://testws.wiemspro.com/Wcommerce/";
        public string miURLDashBoard = $"https://wcommerce.wiemspro.com/Wcommerce_2020/dashboardApp?token=@MiToken&request_locale={miLocale}";
        private HttpClient client;
        public string Error;
        public ClientModel Usuario;

        #endregion

        public ResponseWebService()
        {
        }
        #region Métodos Globales
        internal async Task<bool> GetMenu()
        {
            try
            {
                if (App.DAUtil.DoIHaveInternet())
                {
                    string url = "api/dispositivos/menuEasyMobileRole";
                    MenuModel r = new()
                    {
                        rol = 1
                    };
                    _ = await LlamadaPOSTAsync(url, JsonConvert.SerializeObject(r));

                    if (response.IsSuccessStatusCode)
                    {
                        string resultJSON = await response.Content.ReadAsStringAsync();
                        JsonResponse respuesta = JsonConvert.DeserializeObject<JsonResponse>(resultJSON);

                        Error = "";
                        if (respuesta.resultado == 1)
                        {
                            await App.DAUtil.GuardaMenu(respuesta.menuEasy.OrderBy(p => p.orden).ToList());
                            return true;
                        }
                        else
                        {
                            Error = respuesta.mensaje;
                        }
                    }
                    else
                    {
                        Error = AppResources.ErrorInternet;
                        return false;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                Error = ex.Message.ToString();
                
                return false;
            }
        }
        internal async Task LlamadaAsync(string url)
        {
            try
            {
                var httpClientHandler = new HttpClientHandler();
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                client = new HttpClient(new LoggingHandler(httpClientHandler));
                client.DefaultRequestHeaders.Add("X-CustomToken", ConnDataModel.token);
                client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                client.DefaultRequestHeaders.Add("appVersion", "1.8.39");
                client.DefaultRequestHeaders.Add("appId", App.appId);
                //client = new HttpClient(new HttpClientHandler());

                string requestUri = miURL + url;
                response = new HttpResponseMessage();
                response = await client.GetAsync(requestUri);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await GetToken(Preferences.Get("RUsuario", ""), Preferences.Get("RPass", ""));
                    httpClientHandler = new HttpClientHandler();
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    client = new HttpClient(new LoggingHandler(httpClientHandler));
                    client.DefaultRequestHeaders.Add("X-CustomToken", ConnDataModel.token);
                    client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                    client.DefaultRequestHeaders.Add("appVersion", "1.8.39");
                    client.DefaultRequestHeaders.Add("appId", App.appId);
                    //client = new HttpClient(new HttpClientHandler());

                    requestUri = miURL + url;
                    response = new HttpResponseMessage();
                    response = await client.GetAsync(requestUri);
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Console.WriteLine(ex.Message);
                
            }

        }
        internal async Task LlamadaProAsync(string url)
        {
            try
            {
                HttpClientHandler handler = new();
                handler.ServerCertificateCustomValidationCallback +=
                (sender, certificate, chain, errors) =>
                {
                    return true;
                };
                client = new HttpClient(new LoggingHandler(handler));
                client.DefaultRequestHeaders.Add("X-CustomToken", ConnDataModel.token);
                client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                client.DefaultRequestHeaders.Add("appVersion", "1.8.39");
                client.DefaultRequestHeaders.Add("appId", App.appId);
                //client = new HttpClient(new HttpClientHandler());
                string requestUri = miURL + url;
                response = new HttpResponseMessage();
                response = await client.GetAsync(requestUri);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await GetToken(Preferences.Get("RUsuario", ""), Preferences.Get("RPass", ""));
                    handler = new();
                    handler.ServerCertificateCustomValidationCallback +=
                    (sender, certificate, chain, errors) =>
                    {
                        return true;
                    };
                    client = new HttpClient(new LoggingHandler(handler));
                    client.DefaultRequestHeaders.Add("X-CustomToken", ConnDataModel.token);
                    client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                    client.DefaultRequestHeaders.Add("appVersion", "1.8.39");
                    client.DefaultRequestHeaders.Add("appId", App.appId);
                    //client = new HttpClient(new HttpClientHandler());
                    response = new HttpResponseMessage();
                    response = await client.GetAsync(requestUri);
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Console.WriteLine(ex.Message);
                
            }

        }
        //SOPORTE
        internal async Task LlamadawCommerceAsync(string url)
        {
            try
            {
                if (App.horaInicio.Ticks < DateTime.Now.AddMinutes(-5).Ticks)
                    await GetToken(Preferences.Get("RUsuario", ""), Preferences.Get("RPass", ""));
                HttpClientHandler handler = new();
                handler.ServerCertificateCustomValidationCallback +=
                (sender, certificate, chain, errors) =>
                {
                    return true;
                };
                client = new HttpClient(new LoggingHandler(handler));
                client.DefaultRequestHeaders.Add("X-CustomToken", ConnDataModel.token);
                client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                client.DefaultRequestHeaders.Add("appVersion", "1.8.39");
                client.DefaultRequestHeaders.Add("appId", App.appId);
                //client = new HttpClient(new HttpClientHandler());
                string requestUri = miURLwCommerce + url;
                response = new HttpResponseMessage();
                response = await client.GetAsync(requestUri);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Console.WriteLine(ex.Message);
            }

        }
        internal async Task<string> LlamadaPOSTAsync(string url, string request,string initialId="0",bool token=true)
        {
            try
            {

                //accept certificate seems to work only with HttpClientHandler
                var httpClientHandler = new HttpClientHandler();
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                client = new HttpClient(new LoggingHandler(httpClientHandler));
                if (token)
                    client.DefaultRequestHeaders.Add("X-CustomToken", ConnDataModel.token);
                client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                client.DefaultRequestHeaders.Add("appVersion", "1.8.39");
                client.DefaultRequestHeaders.Add("appId", App.appId);
                if (token)
                    client.DefaultRequestHeaders.Add("initialId", initialId);
                string requestUri = miURL + url;
                response = new HttpResponseMessage();

                response = await client.PostAsync(requestUri, new StringContent(request, Encoding.UTF8, "application/json"));
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await GetToken(Preferences.Get("RUsuario", ""), Preferences.Get("RPass", ""));
                    httpClientHandler = new HttpClientHandler();
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                    client = new HttpClient(new LoggingHandler(httpClientHandler));
                    if (token)
                        client.DefaultRequestHeaders.Add("X-CustomToken", ConnDataModel.token);
                    client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                    client.DefaultRequestHeaders.Add("appVersion", "1.8.39");
                    client.DefaultRequestHeaders.Add("appId", App.appId);
                    if (token)
                        client.DefaultRequestHeaders.Add("initialId", initialId);
                    response = new HttpResponseMessage();

                    response = await client.PostAsync(requestUri, new StringContent(request, Encoding.UTF8, "application/json"));
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Console.WriteLine(ex.Message);
                
                return "";
            }

        }
        internal async Task<string> LlamadaPOSTWithoutTokenAsync(string url, string request)
        {
            try
            {

                //accept certificate seems to work only with HttpClientHandler
                var httpClientHandler = new HttpClientHandler();
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                client = new HttpClient(new LoggingHandler(httpClientHandler));
                client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                client.DefaultRequestHeaders.Add("appId", App.appId);
                string requestUri = miURL + url;
                response = new HttpResponseMessage();

                response = await client.PostAsync(requestUri, new StringContent(request, Encoding.UTF8, "application/json"));


                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Console.WriteLine(ex.Message);
                
                return "";
            }

        }
        private async Task<bool> GetToken(string user, string pass)
        {
            try
            {
                ConnDataModel.uri = miURL + "account/login";
                var httpClientHandler = new HttpClientHandler();
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                client = new HttpClient(new LoggingHandler(httpClientHandler));
                client.DefaultRequestHeaders.Add("locale", App.spanish ? "es" : "en");
                string appVersion = "1.8.39";

                LoginModel login = new(user, appVersion, GetMD5string(pass));

                string log = JsonConvert.SerializeObject(login);
                var cont = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");


                HttpResponseMessage response = new();

                response = await client.PostAsync(ConnDataModel.uri, new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {


                    if (response.Headers.TryGetValues("X-CustomToken", out IEnumerable<string> values))
                    {
                        App.horaInicio = DateTime.Now;
                        ConnDataModel.token = values.ElementAt(0);
                        miURLDashBoard = miURLDashBoard.Replace("@MiToken", ConnDataModel.token);
                        return true;
                    }

                    Error = AppResources.ErrorLogin;
                    return false;
                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return false;
                }
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        internal string GetMD5string(string input)
        {
            try
            {
                using MD5 md5Hash = MD5.Create();
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5Hash.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                
                return "";
            }
        }
        #endregion
        #region Login y Registro
        internal async Task<bool> LoginTrainer(string user, string pass)
        {
            try
            {
                if (App.DAUtil.DoIHaveInternet())
                {
                    string appVersion = "1.8.39";
                    LoginModel login = new(user, appVersion, GetMD5string(pass));
                    await LlamadaPOSTAsync("account/loginEasy", JsonConvert.SerializeObject(login));
                    if (response.IsSuccessStatusCode)
                    {

                        if (response.Headers.TryGetValues("X-CustomToken", out IEnumerable<string> values))
                        {

                            ConnDataModel.token = values.ElementAt(0);
                            Preferences.Set("token", ConnDataModel.token);
                            miURLDashBoard = miURLDashBoard.Replace("@MiToken", ConnDataModel.token);
                            return true;
                        }
                        else
                        {
                            Error = AppResources.ErrorLogin;

                        }

                    }
                    return false;
                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return false;
                }
            }
            catch (Exception ex)
            {
                
                Error = ex.Message.ToString();
                return false;
            }
        }
        internal async Task<bool> LoginWithToken()
        {
            try
            {
                if (App.DAUtil.DoIHaveInternet())
                {
                    string url = "api/dispositivos/entrenador";

                    await LlamadaProAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string resultJSON = await response.Content.ReadAsStringAsync();
                        JsonResponse mensajeJSON = JsonConvert.DeserializeObject<JsonResponse>(resultJSON, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        if (!string.IsNullOrEmpty(mensajeJSON.mensaje) && mensajeJSON.mensaje.Equals("OK"))
                        {
                            TrainerModel trainer = new(mensajeJSON.entrenador);

                            if (await App.DAUtil.ActualizaEntrenador(trainer))
                            {
                                App.DAUtil.Trainer = trainer;
                                Preferences.Set("esTrainer", true);
                                Preferences.Set("senderId", App.DAUtil.Trainer.id);

                                Error = "";
                                return true;
                            }

                            return false;
                        }
                        else
                        {
                            Error = AppResources.ErrorLogin;
                            return false;
                        }

                    }
                    else
                    {
                        Error = AppResources.ErrorLogin;
                        return false;
                    }
                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return false;
                }
            }
            catch (Exception ex)
            {
                
                Error = ex.Message.ToString();
                return false;
            }
        }
        #endregion
        #region Ejercicios
        internal async Task<bool> GetEjercicios()
        {
            try
            {
                if (App.DAUtil.DoIHaveInternet())
                {
                    string url = "api/dispositivos/getEjercicios";

                    await LlamadaProAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultJSON = await response.Content.ReadAsStringAsync();
                        JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };
                        //settings.Converters.Add(new CustomDateTimeConverter());

                        JsonResponse mensajeJSON = JsonConvert.DeserializeObject<JsonResponse>(resultJSON, settings);

                        if (!string.IsNullOrEmpty(mensajeJSON.mensaje) && mensajeJSON.mensaje.Equals("OK"))
                        {
                            if (mensajeJSON.ejercicios.Count > 0)
                            {
                                if (await App.DAUtil.ActualizaEjercicios(mensajeJSON.ejercicios))
                                    return true;
                                else
                                    return false;
                            }
                            else
                                return true;


                        }
                        else
                        {
                            Error = AppResources.ErrorLogin;
                            return false;
                        }

                    }
                    else
                    {
                        Error = AppResources.ErrorLogin;
                        return false;
                    }
                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message.ToString();
                return false;
            }
        }
        internal async Task<bool> GetVideosEjercicios()
        {
            try
            {
                if (App.DAUtil.DoIHaveInternet())
                {
                    string url = "api/dispositivos/getEjercicios";

                    await LlamadaProAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultJSON = await response.Content.ReadAsStringAsync();
                        JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };
                        //settings.Converters.Add(new CustomDateTimeConverter());

                        JsonResponse mensajeJSON = JsonConvert.DeserializeObject<JsonResponse>(resultJSON, settings);

                        if (!string.IsNullOrEmpty(mensajeJSON.mensaje) && mensajeJSON.mensaje.Equals("OK"))
                        {
                            if (mensajeJSON.ejercicios.Count > 0)
                            {
                                foreach (EjercicioModel ej in mensajeJSON.ejercicios)
                                {
                                    try
                                    {
                                        
                                        
                                        await App.DAUtil.ActualizaEjercicio(ej);
                                        Console.WriteLine("Guardado ejercicio: " + ej.id.ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                                return true;
                            }
                            else
                                return true;


                        }
                        else
                        {
                            Error = AppResources.ErrorLogin;
                            return false;
                        }

                    }
                    else
                    {
                        Error = AppResources.ErrorLogin;
                        return false;
                    }
                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message.ToString();
                return false;
            }
            finally
            {
            }
        }
        #endregion
        #region Clientes
        internal async Task<bool> GetClientes()
        {
            try
            {
                if (App.DAUtil.DoIHaveInternet())
                {
                    string url = "api/dispositivos/getClienteSimple";

                    await LlamadaProAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string resultJSON = await response.Content.ReadAsStringAsync();
                        JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };
                        //settings.Converters.Add(new CustomDateTimeConverter());

                        JsonResponse mensajeJSON = JsonConvert.DeserializeObject<JsonResponse>(resultJSON, settings);

                        if (!string.IsNullOrEmpty(mensajeJSON.mensaje) && mensajeJSON.mensaje.Equals("OK"))
                        {
                            if (mensajeJSON.clientes.Count > 0)
                            {
                                if (await App.DAUtil.ActualizaClientes(mensajeJSON.clientes))
                                {
                                    List<ClientModel> clientList = new(await App.DAUtil.GetClientsFromTrainer(App.DAUtil.Trainer));

                                    return true;
                                }
                                else
                                    return false;
                            }
                            else
                                return true;


                        }
                        else
                        {
                            Error = AppResources.ErrorLogin;
                            return false;
                        }

                    }
                    else
                    {
                        Error = AppResources.ErrorLogin;
                        return false;
                    }
                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return false;
                }
            }
            catch (Exception ex)
            {
                
                Error = ex.Message.ToString();
                return false;
            }
        }
        internal async Task<ClientModel> GetCliente(ClientModel c)
        {
            try
            {
                string url = "api/dispositivos/getClienteHome";
                JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };


                string jsonString = JsonConvert.SerializeObject(c, settings);


                await LlamadaPOSTAsync(url, jsonString);

                if (response.IsSuccessStatusCode)
                {
                    string resultJSON = await response.Content.ReadAsStringAsync();

                    JsonResponse mensajeJSON = JsonConvert.DeserializeObject<JsonResponse>(resultJSON, settings);
                    Error = "";

                    if (!string.IsNullOrEmpty(mensajeJSON.mensaje) && mensajeJSON.mensaje.Equals("OK"))
                    {
                        
                        return mensajeJSON.user;
                    }
                    else
                    {
                        await App.DAUtil.ActualizaCliente(c);
                        Error = AppResources.Error;
                        return null;
                    }

                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion
        #region Entrenamientos
        internal async Task<(bool, JsonResponse)> GetEntrenamientos()
        {
            try
            {
                if (App.DAUtil.DoIHaveInternet())
                {
                    string url = "api/dispositivos/getEntrenamientosProgramasEasy";
                    await LlamadaAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string resultJSON = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };

                        JsonResponse mensajeJSON = JsonConvert.DeserializeObject<JsonResponse>(resultJSON, settings);
                        Error = "";

                        if (!string.IsNullOrEmpty(mensajeJSON.mensaje) && mensajeJSON.mensaje.Equals("OK"))
                        {
                            foreach (SessionModel s in mensajeJSON.entrenamientos)
                            {
                                s.urlMedia = "level1.png";
                            }
                            return (true, mensajeJSON);
                        }
                        else
                        {
                            Error = AppResources.ErrorLogin;
                            return (false, null);
                        }


                    }
                    else
                    {
                        Error = AppResources.ErrorLogin;
                        return (false, null);
                    }

                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return (false, null);
                }

            }
            catch (Exception ex)
            {
                
                Error = ex.Message.ToString();
                return (false, null);
            }
        }
        internal async Task<(bool,JsonResponse)> GetPrograms()
        {
            try
            {
                if (App.DAUtil.DoIHaveInternet())
                {
                    string url = "api/dispositivos/getSessionProgram";
                    await LlamadaAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string resultJSON = await response.Content.ReadAsStringAsync();

                        JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };

                        JsonResponse mensajeJSON = JsonConvert.DeserializeObject<JsonResponse>(resultJSON, settings);
                        Error = "";

                        if (!string.IsNullOrEmpty(mensajeJSON.mensaje) && mensajeJSON.mensaje.Equals("OK"))
                            return (true, mensajeJSON);
                        else
                        {
                            Error = AppResources.ErrorLogin;
                            return (false, null);
                        }


                    }
                    else
                    {
                        Error = AppResources.ErrorLogin;
                        return (false, null);
                    }

                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return (false, null);
                }

            }
            catch (Exception ex)
            {
                
                Error = ex.Message.ToString();
                return (false, null);
            }
        }
        internal async Task<(bool, JsonResponse)> GetProgramsByIds(List<ProgramModel> programs)
        {
            try
            {
                string url = "api/dispositivos/getSessionProgramByIds";

                string jsonString = "[";

                foreach (ProgramModel p in programs)
                {
                    jsonString += p.id + ",";
                }

                jsonString = jsonString.Substring(0, jsonString.Length-1) + "]";
                
                await LlamadaPOSTAsync(url, jsonString);

                if (response.IsSuccessStatusCode)
                {
                    string resultJSON = await response.Content.ReadAsStringAsync();

                    JsonSerializerSettings settings = new() { NullValueHandling = NullValueHandling.Ignore };

                    JsonResponse mensajeJSON = JsonConvert.DeserializeObject<JsonResponse>(resultJSON, settings);
                    Error = "";

                    if (!string.IsNullOrEmpty(mensajeJSON.mensaje) && mensajeJSON.mensaje.Equals("OK"))
                        return (true, mensajeJSON);
                    else
                    {
                        Error = AppResources.ErrorLogin;
                        return (false, null);
                    }

                }
                else
                {
                    Error = AppResources.ErrorInternet;
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                
                return (false, null);
            }
        }
        #endregion
    }
}
