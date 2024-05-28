using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using Template.Models.JSON;

namespace Template.Models
{
    public partial class TrainerModel : ObservableObject
    {
        [PrimaryKey]
        public int id { get; set; }
        public int managerId { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string autoSerial { get; set; }
        public bool enabled { get; set; }
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

        [ObservableProperty]
        int valueReturnMute;
        [ObservableProperty]
        int valueReturnEndMute;
        [ObservableProperty]
        int vueltaSetup;
        [ObservableProperty]
        int vueltaFinalSetup;
        [ObservableProperty]
        int vueltaSetupT1;
        [ObservableProperty]
        int vueltaFinalSetupT1;
        [ObservableProperty]
        int tipoChaleco;

        public bool esZurdo { get; set; }
        public int tipoLog { get; set; }
        public string tokenOneSignal { get; set; }
        public int porcInicial { get; set; }
        public int subidaHasta { get; set; }


        public static int getNumProperties()
        {
            return 39;
        }

        public static string getName()
        {
            return nameof(TrainerModel);
        }

        public TrainerModel()
        {

        }

        public TrainerModel(TrainerJSON trainerJSON) : this()
        {
            id = trainerJSON.id;
            managerId = trainerJSON.managerId;
            username = trainerJSON.user.email;
            name = trainerJSON.user.name;
            surname = trainerJSON.user.surname;
            autoSerial = trainerJSON.user.autoSerial;
            enabled = trainerJSON.enabled;
            premium = trainerJSON.premium;
            policy1 = trainerJSON.policy1;
            policy2 = trainerJSON.policy2;
            policy3 = trainerJSON.policy3;
            editRestingTime = trainerJSON.editRestingTime;
            muteEnable = trainerJSON.muteEnable;
            muteVisible = trainerJSON.muteVisible;
            vueltaSetupEnable = trainerJSON.vueltaSetupEnable;
            vueltaSetupVisible = trainerJSON.vueltaSetupEnable;
            currentChBanned = trainerJSON.currentChBanned;
            timeChRestored = trainerJSON.timeChRestored;
            timeCurrentIncrement = trainerJSON.timeCurrentIncrement;
            timeCurrentIncrement2 = trainerJSON.timeCurrentIncrement2;
            timeCurrentIncrement3 = trainerJSON.timeCurrentIncrement3;
            subidasTramo1 = trainerJSON.subidasTramo1;
            subidasTramo2 = trainerJSON.subidasTramo2;
            subidasTramo3 = trainerJSON.subidasTramo3;
            unidadesMedida = trainerJSON.unidadesMedida;
            unidadesPeso = trainerJSON.unidadesPeso;
            duracionMute = trainerJSON.duracionMute;
            valueReturnMute = trainerJSON.valueReturnMute;
            valueReturnEndMute = trainerJSON.valueReturnEndMute;
            vueltaSetup = trainerJSON.vueltaSetup;
            vueltaFinalSetup = trainerJSON.vueltaFinalSetup;
            vueltaSetupT1 = trainerJSON.vueltaSetupT1;
            vueltaFinalSetupT1 = trainerJSON.vueltaFinalSetupT1;
            esZurdo = trainerJSON.esZurdo;
            tipoLog = trainerJSON.tipoLog;
            tokenOneSignal = trainerJSON.tokenOneSignal;
            porcInicial = 0;
            subidaHasta = 60;
        }

        public int getTimeCurrentIncrementWithCurrent(int curr)
        {
            if (curr < 30)
                return this.timeCurrentIncrement;
            else if (curr < 60)
                return this.timeCurrentIncrement2;
            else
                return this.timeCurrentIncrement3;
        }

    }


}