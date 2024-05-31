using System.Collections.Generic;
using Template.Models.Entrenamiento;

namespace Template.Models.JSON
{

    // MensajeJSON myDeserializedClass = JsonConvert.DeserializeObject<MensajeJSON>(myJsonResponse); 
    public class UserJSON
    {
        public bool enabled { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string autoSerial { get; set; }
    }

    public class TrainerJSON
    {
        public int id { get; set; }
        public bool enabled { get; set; }
        public int managerId { get; set; }
        public bool premium { get; set; }
        public bool policy1 { get; set; }
        public bool policy2 { get; set; }
        public bool policy3 { get; set; }
        public bool editRestingTime { get; set; }
        public bool muteEnable { get; set; }
        public bool muteVisible { get; set; }
        public bool vueltaSetupEnable { get; set; }
        public bool vueltaSetupVisible { get; set; }
        public int currentChBanned { get; set; }
        public int timeChRestored { get; set; }
        public int timeCurrentIncrement { get; set; }
        public int timeCurrentIncrement2 { get; set; }
        public int timeCurrentIncrement3 { get; set; }
        public int subidasTramo1 { get; set; }
        public int subidasTramo2 { get; set; }
        public int subidasTramo3 { get; set; }
        public string unidadesMedida { get; set; }
        public string unidadesPeso { get; set; }
        public int duracionMute { get; set; }
        public int valueReturnMute { get; set; }
        public int valueReturnEndMute { get; set; }
        public int vueltaSetup { get; set; }
        public int vueltaFinalSetup { get; set; }
        public int vueltaSetupT1 { get; set; }
        public int vueltaFinalSetupT1 { get; set; }
        public bool esZurdo { get; set; }
        public int tipoLog { get; set; }
        public string tokenOneSignal { get; set; }
        public UserJSON user { get; set; }
    }

    public class JsonResponse
    {
        public int resultado { get; set; }
        public string mensaje { get; set; }
public ClientModel cliente { get; set; }
        public TrainerJSON entrenador { get; set; }
        public List<DeviceModel> dispositivos { get; set; }
        public List<DeviceHRMModel> devicesHRM { get; set; }
        public List<ClientModel> clientes { get; set; }
        public List<EjercicioModel> ejercicios { get; set; }
        public List<SessionModel> entrenamientos { get; set; }
        public List<ProgramModel> sessionPrograms { get; set; }
        public List<EntrenamientoClienteModel> trainings { get; set; }
        public FotoModel foto { get; set; }
        public ClientModel user { get; set; }
        public List<MenuModel> menuEasy { get; set; }
        public bool tienePermisoApp { get; set; }
    }


}
