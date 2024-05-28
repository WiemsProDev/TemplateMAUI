using System;
using SQLite;
using Newtonsoft.Json;
using Template.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using Template.ViewModels.Base;

namespace Template.Models
{
    public partial class DeviceHRMModel : ViewModelBase
    {

        [PrimaryKey,AutoIncrement]
        public int id { get; set; }
        public string uuid { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool enabled { get; set; }
        public string alias { get; set; }
        public string name { get; set; }
        public string modelo { get; set; }
        public string manufacturer { get; set; }
        public int managerId { get; set; }
        public int trainerId { get; set; }
        public string sEditionDate { get; set; }

        [JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime? dtEditionDate { get; set; }

        [Ignore, JsonIgnore]
        public int battery { get; set; }

        [Ignore, JsonIgnore]
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool visibleBat { get; set; }

       // [JsonIgnore, ObservableProperty]
       // [JsonConverter(typeof(BoolToBitJsonConverter))]
       // public bool estadoBoton;

        public static int getNumProperties()
        {
            return 11;
        }

        public static string getName()
        {
            return typeof(DeviceHRMModel).Name;
        }

        public DeviceHRMModel()
        {
           // estadoBoton = false;
        }

        public DeviceHRMModel(string nombre, string id)
        {
            name = nombre;
            alias = nombre;
            uuid = id;

        }
    }
}
