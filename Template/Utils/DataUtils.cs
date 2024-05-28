

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using Template.Interfaces;
using Template.Models;
using Template.ViewModels.Base;

namespace Template.Utils
{
    public class DataUtils
    {
        #region Propiedades
        public string Idioma = "";
        private static readonly AsyncLock Mutex = new();
        private readonly SQLiteAsyncConnection _sqlCon;
        public INavigationService NavigationService { get; }
        public List<SideMenuItemModel> listadoOpcionesPorRol = new();
        public bool ConFoto;
        public bool ConWeb;

        public ClientModel Client;
        public SessionModel Session;
        public TrainerModel Trainer;
        public string Error;

        private string installID;
        public string InstallId
        {
            get { return installID; }
            set { installID = value; }
        }
        #endregion
              

        public DataUtils()
        {
            try
            {
                var service = DependencyService.Get<IPathService>();
                var databasePath = service.GetDatabasePath();
                Debug.WriteLine("DB: " + databasePath);
                
                _sqlCon = new SQLiteAsyncConnection(databasePath);

                CreateDatabaseAsync();
                NavigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #region Métodos Globales
        internal bool DoIHaveInternet()
        {
            var current = Connectivity.NetworkAccess;

            return current == NetworkAccess.Internet;
        }
        public async void CreateDatabaseAsync()
        {
            try
            {
               if (await GetInfo(TrainerModel.getName()) != TrainerModel.getNumProperties())
                {
                    if (await GetInfo(TrainerModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<TrainerModel>();

                    await _sqlCon.CreateTableAsync<TrainerModel>(CreateFlags.None).ConfigureAwait(false);
                }
                if (await GetInfo(MenuModel.getName()) != MenuModel.getNumProperties())
                {
                    if (await GetInfo(MenuModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<MenuModel>();

                    await _sqlCon.CreateTableAsync<MenuModel>(CreateFlags.None).ConfigureAwait(false);
                }
                if (await GetInfo(ClientModel.getName()) != ClientModel.getNumProperties())
                {
                    if (await GetInfo(ClientModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<ClientModel>();

                    await _sqlCon.CreateTableAsync<ClientModel>(CreateFlags.None).ConfigureAwait(false);
                }

                if (await GetInfo(DeviceModel.getName()) != DeviceModel.getNumProperties())
                {
                    if (await GetInfo(DeviceModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<DeviceModel>();

                    await _sqlCon.CreateTableAsync<DeviceModel>(CreateFlags.None).ConfigureAwait(false);
                }


                if (await GetInfo(SessionModel.getName()) != SessionModel.getNumProperties())
                {
                    if (await GetInfo(SessionModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<SessionModel>();

                    await _sqlCon.CreateTableAsync<SessionModel>(CreateFlags.None).ConfigureAwait(false);
                }

                if (await GetInfo(TrackModel.getName()) != TrackModel.getNumProperties())
                {
                    if (await GetInfo(TrackModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<TrackModel>();

                    await _sqlCon.CreateTableAsync<TrackModel>(CreateFlags.None).ConfigureAwait(false);
                }

                if (await GetInfo(ProgramModel.getName()) != ProgramModel.getNumProperties())
                {
                    if (await GetInfo(ProgramModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<ProgramModel>();

                    await _sqlCon.CreateTableAsync<ProgramModel>(CreateFlags.None).ConfigureAwait(false);
                }

                if (await GetInfo(EjercicioModel.getName()) != EjercicioModel.getNumProperties())
                {
                    if (await GetInfo(EjercicioModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<EjercicioModel>();

                    await _sqlCon.CreateTableAsync<EjercicioModel>(CreateFlags.None).ConfigureAwait(false);
                }

                if (await GetInfo(EjercicioServerModel.getName()) != EjercicioServerModel.getNumProperties())
                {
                    if (await GetInfo(EjercicioServerModel.getName()) != 0)
                        await _sqlCon.DropTableAsync<EjercicioServerModel>();

                    await _sqlCon.CreateTableAsync<EjercicioServerModel>(CreateFlags.None).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
        }
        public async Task<double> CheckInternetSpeed()
        {
            DateTime dt1 = DateTime.Now;
            double internetSpeed;
            try
            {
                var client = new HttpClient();
                byte[] data = await client.GetByteArrayAsync("http://xamarinmonkeys.blogspot.com/");
                DateTime dt2 = DateTime.Now;
                internetSpeed = Math.Round((data.Length / 1024) / (dt2 - dt1).TotalSeconds, 2);
            }
            catch (Exception)
            {
                internetSpeed = 0;
            }
            return internetSpeed;
        }

        public async Task<int> GetInfo(string nombreTabla)
        {
            try
            {
                var info = await _sqlCon.GetTableInfoAsync(nombreTabla);
                return info.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
        #region Menu

        internal async Task<List<MenuModel>> GetMenu(int idRol)
        {
            try
            {
                List<MenuModel> listPersona = await _sqlCon.QueryAsync<MenuModel>("Select * From [MenuModel] Where rol=" + idRol + " order by orden");
                if (listPersona.Count > 0)
                {
                    /*if (App.DAUtil.Client != null)
                    {
                        foreach (MenuModel m in listPersona)
                        {
                            if (App.DAUtil.Client.id == -1 && (m.nombre.Equals("Histórico de tratamientos") || m.nombre.Equals("Perfil")))
                                m.visible = false;
                        }
                    }*/
                    return listPersona;
                }

                return new List<MenuModel>();

            }
            catch (Exception ex)
            {
                
                return new List<MenuModel>();
            }

        }
        internal async Task GuardaMenu(List<MenuModel> items)
        {
            try
            {
                    foreach (MenuModel item in items)
                    {
                        var existingUsuarioItem = await _sqlCon.Table<MenuModel>()
                                .Where(x => x.id == item.id)
                                .FirstOrDefaultAsync();
                        if (existingUsuarioItem == null)
                        {
                            await _sqlCon.InsertWithChildrenAsync(item, recursive: false).ConfigureAwait(false);
                        }
                        else
                        {
                            item.id = existingUsuarioItem.id;
                            await _sqlCon.UpdateWithChildrenAsync(item).ConfigureAwait(false);
                        }
                    }
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion

        #region Entrenador
        internal async Task<bool> ActualizaEntrenador(TrainerModel item)
        {
            try
            {
                    TrainerModel existingEntrenadorItem = await _sqlCon.Table<TrainerModel>()
                            .Where(x => x.id == item.id)
                            .FirstOrDefaultAsync();

                    if (existingEntrenadorItem == null)
                    {
                        await _sqlCon.InsertWithChildrenAsync(item, recursive: true).ConfigureAwait(false);
                        return true;
                    }
                    else
                    {
                        await _sqlCon.UpdateWithChildrenAsync(item).ConfigureAwait(false);
                        return true;
                    }
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        internal async Task<TrainerModel> GetTrainerByUsername(string username)
        {

            if (username == null)
                return null;

            try
            {
                    return await _sqlCon.Table<TrainerModel>()
                            .Where(x => x.username == username)
                            .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
        #endregion
        #region Ejercicios
        internal async Task<List<EjercicioModel>> GetEjercicios()
        {
            try
            {
                    List<EjercicioModel> clientList = await _sqlCon.GetAllWithChildrenAsync<EjercicioModel>();

                    if (clientList.Count > 0)
                    {
                        EjercicioModel ej = clientList.FirstOrDefault(x => x.nombre.ToLower().Equals("ejercicio libre"));
                        if (ej != null)
                        {
                            clientList.Remove(ej);
                            clientList= clientList.OrderBy(p => p.nombre).ToList();
                            clientList.Insert(0, ej);
                            return clientList;
                        }else
                            return clientList.OrderBy(p => p.nombre).ToList();
                    }
                    else
                        return new List<EjercicioModel>();
            }
            catch (Exception ex)
            {
                
                return new List<EjercicioModel>();
            }
        }

        internal async Task<EjercicioModel> GetEjercicioById(int id)
        {

            if (id == 0)
                return null;

            try
            {
                    return await _sqlCon.Table<EjercicioModel>()
                            .Where(x => x.id == id)
                            .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
        internal async Task<bool> ActualizaEjercicios(List<EjercicioModel> items)
        {


            //si no devuelve ninguno del servidor, no tengo que almacenarlos por lo que retorno
            if (items.Count == 0)
            {
                return true;
            }

            try
            {
                    await _sqlCon.InsertOrReplaceAllWithChildrenAsync(items, recursive: true).ConfigureAwait(false);
                    return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        internal async Task<bool> ActualizaEjercicio(EjercicioModel item)
        {
            try
            {
                    var existingUsuarioItem = await _sqlCon.Table<EjercicioModel>()
                            .Where(x => x.id == item.id)
                            .FirstOrDefaultAsync() ?? await _sqlCon.Table<EjercicioModel>()
                            .Where(x => x.id == item.id)
                            .FirstOrDefaultAsync();
                    if (existingUsuarioItem == null)
                    {
                        await _sqlCon.InsertWithChildrenAsync(item, recursive: true).ConfigureAwait(false);
                        return true;
                    }
                    else
                    {
                        item.id = existingUsuarioItem.id;
                        await _sqlCon.UpdateWithChildrenAsync(item).ConfigureAwait(false);
                        return true;
                    }
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        #endregion
        #region Clientes
        internal async Task<bool> ActualizaClientes(List<ClientModel> items)
        {
            //si no devuelve ninguno del servidor, no tengo que almacenarlos por lo que retorno
            if (items.Count == 0)
            {
                return true;
            }

            await _sqlCon.InsertOrReplaceAllWithChildrenAsync(items, recursive: true).ConfigureAwait(false);
              
            return true;
        }

        internal async Task<bool> ActualizaCliente(ClientModel item)
        {
            try
            {
                    ClientModel existingUsuarioItem = await _sqlCon.Table<ClientModel>()
                            .Where(x => x.id == item.id)
                            .FirstOrDefaultAsync() ?? await _sqlCon.Table<ClientModel>()
                            .Where(x => x.userId == item.userId)
                            .FirstOrDefaultAsync();
                    if (existingUsuarioItem == null)
                    {
                        await _sqlCon.InsertWithChildrenAsync(item, recursive: true).ConfigureAwait(false);
                        return true;
                    }
                    else
                    {
                        item.id = existingUsuarioItem.id;
                        await _sqlCon.UpdateWithChildrenAsync(item).ConfigureAwait(false);
                        return true;
                    }
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        internal async Task<List<ClientModel>> GetClientsFromTrainer(TrainerModel trainer)
        {
            try
            {
                    
                    List<ClientModel> clientList = await _sqlCon.GetAllWithChildrenAsync<ClientModel>(x => x.idTrainer == trainer.id,true);

                    if (clientList.Count > 0)
                    {
                        foreach (ClientModel c in clientList)
                        {
                        //List<EntrenamientoClienteModel> a = new List<EntrenamientoClienteModel>(await GetTrainingsFromClient(c));
                            c.calculaEdad();
                            /*foreach (PlanEntrenamientoClienteModel pc in c.planes.Where(p => p.enabled == true))
                            {
                                c.tienePlan = true;
                                if (pc.estructura.Count > 0)
                                    pc.progreso = (pc.estructura.Count - pc.sesionesRestantes).ToString() + "/" + pc.estructura.Count.ToString();

                                try
                                {
                                    if (pc.sesionesRestantes > 0)
                                        pc.entrenamiento = await App.DAUtil.GetTrainingByID(pc.estructura[pc.estructura.Count - pc.sesionesRestantes].idEntrenamiento);
                                    else
                                    {
                                        pc.entrenamiento = new SessionModel()
                                        {
                                            nombre = "Entrenamiento Finalizado",
                                            nombreIngles = "Training Ended"
                                        };
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }*/
                            if (c.name == null)
                                c.name = "";
                            if (c.surname == null)
                                c.surname = "";
                            if (c.alias == null)
                                c.alias = "";
                            if (c.name == null || c.name.Equals(""))
                            {
                                string[] nombreApellido = c.alias.Split('-');

                                if (nombreApellido.Count() == 2)
                                {
                                    c.name = nombreApellido[0];
                                    c.surname = nombreApellido[1];
                                }
                            }
                        }
                        return clientList;
                    }
                    else
                        return new List<ClientModel>();
            }
            catch (Exception ex)
            {
                
                return new List<ClientModel>();
            }
        }
        internal async Task<ClientModel> GetClienteById(int id)
        {

            if (id == 0)
                return null;

            try
            {
                List<ClientModel> clientList = await _sqlCon.GetAllWithChildrenAsync<ClientModel>(x => x.id == id, true);

                if (clientList.Count > 0)
                {
                    foreach (ClientModel c in clientList)
                    {
                        c.calculaEdad();

                        if (c.name == null || c.name.Equals(""))
                        {
                            string[] nombreApellido = c.alias.Split('-');

                            if (nombreApellido.Count() == 2)
                            {
                                c.name = nombreApellido[0];
                                c.surname = nombreApellido[1];
                            }
                        }
                    }

                    return clientList.FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
        internal async Task<ClientModel> GetClienteByUserId(string userId)
        {
            try
            {
                List<ClientModel> clientList = await _sqlCon.GetAllWithChildrenAsync<ClientModel>(x => x.userId == userId, true);

                if (clientList.Count > 0)
                {
                    foreach (ClientModel c in clientList)
                    {
                        c.calculaEdad();

                        if (c.name == null || c.name.Equals(""))
                        {
                            string[] nombreApellido = c.alias.Split('-');

                            if (nombreApellido.Count() == 2)
                            {
                                c.name = nombreApellido[0];
                                c.surname = nombreApellido[1];
                            }
                        }
                    }

                    return clientList.FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
        internal async Task<bool> BorraCliente(ClientModel item)
        {
            try
            {
                await _sqlCon.DeleteAsync(item);
                //await _sqlCon.DeleteAllAsync<ClientModel>();

                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        internal async Task<bool> BorraEjercicio(EjercicioModel item)
        {
            try
            {
                await _sqlCon.DeleteAsync(item);
                //await _sqlCon.DeleteAllAsync<ClientModel>();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        #region Entrenamientos
        internal async Task<bool> ActualizaEntrenamientos(List<SessionModel> items)
        {
            try
            {
                    await _sqlCon.DeleteAllAsync<ProgramModel>();
                    await _sqlCon.DeleteAllAsync<TrackModel>();
                    await _sqlCon.DeleteAllAsync<SessionModel>();
                    await _sqlCon.DeleteAllAsync<EjercicioServerModel>();
                    foreach (SessionModel s in items)
                    {
                        foreach (TrackModel t in s.tracks)
                        {
                            foreach (EjercicioServerModel e in t.ejercicios)
                            {
                                e.trackId = t.id;
                            }
                        }
                    }
                    await _sqlCon.InsertAllWithChildrenAsync(items, recursive: true).ConfigureAwait(false);  //lento, 2 segundos

                    return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        internal async Task<bool> CreaEntrenamiento(SessionModel training)
        {
            try {
            await _sqlCon.InsertWithChildrenAsync(training, recursive: true).ConfigureAwait(false);

            return true;
        }catch (Exception ex)
            {
                
                return false;
            }
        }
        internal async Task<bool> ActualizaEntrenamiento(SessionModel training)
        {
            try
            {
                    await _sqlCon.UpdateWithChildrenAsync(training).ConfigureAwait(false);

                    return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        internal async Task<bool> DeleteEntrenamiento(SessionModel training)
        {
            try
            {
                    await _sqlCon.DeleteAsync(training).ConfigureAwait(false);

                    return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        internal async Task<bool> ActualizaProgramasAsync(List<ProgramModel> items)
        {
            //Funcion muy optimizada en tiempos 165 ms

            //actualizo los trackID de los programas para mantener la relacion en base de datos
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                List<ProgramModel> programList = new();

                    foreach (var p in items)
                    {
                        var existingProgram = await _sqlCon.Table<ProgramModel>()
                            .Where(x => x.id == p.id)
                            .FirstOrDefaultAsync();


                        if (existingProgram != null)
                        {
                            p.trackId = existingProgram.trackId;

                            programList.Add(p);


                        }
                    }
               

                    if (programList.Count > 0)
                        await _sqlCon.UpdateAllAsync(programList).ConfigureAwait(false); //rápido

                Debug.WriteLine("\nVCR-ActualizaProgramas " + watch.ElapsedMilliseconds);
                return true;
            }
            catch
            {
                return false;
            }


        }

        internal async Task<List<SessionModel>> GetTrainingsFromManager(TrainerModel trainer)
        {
            try
            {
                    List<SessionModel> trainingsList = await _sqlCon.GetAllWithChildrenAsync<SessionModel>(x => x.id_manager == trainer.managerId || x.id_manager==0,recursive:true);  //tambien los genéricos
                    foreach (SessionModel s in trainingsList)
                    {
                        if (string.IsNullOrEmpty(s.urlMedia))
                        {
                            switch (s.idFamilia)
                            {
                                case 1:
                                    s.urlMedia = "level1.png";
                                    break;
                                case 2:
                                    s.urlMedia = "level2.png";
                                    break;
                                case 3:
                                    s.urlMedia = "level3.png";
                                    break;
                                case 4:
                                    s.urlMedia = "level4.png";
                                    break;
                                case 5:
                                    s.urlMedia = "level5.png";
                                    break;
                            }
                        }
                        
                    }
                    if (trainingsList.Count > 0)
                        return trainingsList;
                    else
                        return new List<SessionModel>();
            }
            catch (Exception ex)
            {
                
                return new List<SessionModel>();
            }
        }

        internal async Task<SessionModel> GetTrainingByID(int id)
        {
            try
            {

                    List<SessionModel> sessionList = await _sqlCon.GetAllWithChildrenAsync<SessionModel>(x => x.id == id);

                    if (sessionList.Count > 0)
                        return sessionList[0];
                    else
                    {
                        //await App.ResponseWS.GetEntrenamientosCliente
                        return null;
                    }

            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
        internal async Task<ProgramModel> GetProgram(int idTrack)
        {
            try
            {
                    List<ProgramModel> programs = await _sqlCon.GetAllWithChildrenAsync<ProgramModel>(x => x.trackId == idTrack);

                    if (programs.Count > 0)
                        return programs.First();
                    else
                        return null;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
        internal async Task<List<EjercicioServerModel>> GetEjerciciosByTrack(int idTrack)
        {
            try
            {
                    List<EjercicioServerModel> programs = await _sqlCon.GetAllWithChildrenAsync<EjercicioServerModel>(x => x.trackId == idTrack);

                    if (programs.Count > 0)
                        return programs;
                    else
                        return null;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }


        #endregion
    }
}
