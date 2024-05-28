using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Template.Models
{
    public class DeviceModel
    {
        [PrimaryKey]
        public int id { get; set; }
        public bool enabled { get; set; }
        public string name { get; set; }
        public string serialNumber { get; set; }
        public string uuid { get; set; }
        public string alias { get; set; }
        public string firmVersion { get; set; }
        public string firmBLEVersion { get; set; }
        public int managerId { get; set; }
        public int trainerId { get; set; }
        public int purchasedMinutes { get; set; }
        public int consumedMinutes { get; set; }
        public bool cedido { get; set; }
        [Ignore, JsonIgnore]
        public bool conectado { get; set; }
        [Ignore, JsonIgnore]
        public bool haApagado  { get; set; }

        public static int getNumProperties()
        {
            return 13;
        }

        public static string getName()
        {
            return nameof(DeviceModel);
        }

        public DeviceModel()
        {

        }

        public DeviceModel(string nombre, string id, string serial, bool con)
        {
            name = nombre;
            uuid = id;
            serialNumber = serial;
            conectado = con;
            haApagado = false;
        }

        //Evita que serialice la propiedad enable
        public bool ShouldSerializeenabled()
        {
            return (false);
        }

        public bool ShouldSerializename()
        {
            return (false);
        }

        public bool ShouldSerializemanagerId()
        {
            return (false);
        }

        public bool ShouldSerializetrainerId()
        {
            return (false);
        }

        public bool ShouldSerializefirmVersion()
        {
            return (false);
        }

        public bool ShouldSerializefirmBLEVersion()
        {
            return (false);
        }

        public bool ShouldSerializepurchasedMinutes()
        {
            return (false);
        }
        public bool ShouldSerializecesiones()
        {
            return (false);
        }

    }

    //En este caso no sería necesario, pero se deja de ejemplo
    public class DeviceContractResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public DeviceContractResolver()
        {
            this.PropertyMappings = new Dictionary<string, string>
        {
            {"id", "id"},
            {"alias", "alias"},
            {"uuid", "uuid"},
            {"consumedMinutes", "consumedMinutes"}
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
