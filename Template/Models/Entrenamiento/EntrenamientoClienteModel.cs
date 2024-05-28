using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;
using Template.Converters;
using Newtonsoft.Json.Serialization;

namespace Template.Models.Entrenamiento
{
    public class EntrenamientoClienteModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int idServer { get; set; }
        public bool enabled { get; set; }
        public bool toSend { get; set; }
        [JsonIgnore]
        public int subidasFallidas { get; set; }
        [JsonConverter(typeof(DateEntrenamientoJsonConverter))]
        public DateTime date { get; set; }

        public string name { get; set; }
        public int programedDuration { get; set; }
        public int realDuration { get; set; }
        public int frequency { get; set; }
        public int workingTime { get; set; }
        public int restingTime { get; set; }
        public string deviceSerialNumber { get; set; }
        public int deviceConsumedMinutes { get; set; }
        public int trainerId { get; set; }
        public int rampUp { get; set; }
        public int rampDown { get; set; }

        public string userId { get; set; }
        public int userIdCode { get; set; }
        public int numLogs { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public LocationModel location { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<HrDataModel> hrdata { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<InformeModel> informes { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LogModel> logs { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LogBigDataModel> logsBigData { get; set; }

        public string appVersion { get; set; }
        public int vestType { get; set; }
        public string iPadModel { get; set; }
        public string iOsVersion { get; set; }
        public string firmwareVersion { get; set; }
        public string firmwareBLEVersion { get; set; }
        public int hrMaxTeorica { get; set; }
        public int hrReposo { get; set; }
        public double vo2max { get; set; }
        public double pesoUsuario { get; set; }
        public string uuidCode { get; set; }
        public int idTrainingMto { get; set; }

        public static int getNumProperties()
        {
            return 33;
        }

        public static string getName()
        {
            return nameof(EntrenamientoClienteModel);
        }

        //Evita que serialice la propiedad id

        public bool ShouldSerializeid()
        {
            return (false);
        }

        public bool ShouldSerializeidServer()
        {
            return (false);
        }

        public bool ShouldSerializeenabled()
        {
            return (false);
        }

        public bool ShouldSerializetoSend()
        {
            return (false);
        }

        public bool ShouldSerializeuserIdCode()
        {
            return (false);
        }

        

        public EntrenamientoClienteModel()
        {
           
        }

    }


    public class EntrenamientoClienteContractResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public EntrenamientoClienteContractResolver()
        {
            this.PropertyMappings = new Dictionary<string, string>
        {
            {"hrdata", "hrData"},
            {"informes", "informe"}
        };
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            string resolvedName = null;
            var resolved = this.PropertyMappings.TryGetValue(propertyName, out resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
}

