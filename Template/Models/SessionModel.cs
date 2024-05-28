using System;
using SQLite;
using Newtonsoft.Json;
using Template.Converters;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;

namespace Template.Models
{
    public class SessionModel
    {
        [PrimaryKey]
        public int id { get; set; }
        public bool enabled { get; set; }

        public int id_trainer { get; set; }
        public int id_manager { get; set; }
        public string nombre { get; set; }
        public string nombreIngles { get; set; }
        public string urlMedia { get; set; }
        public string urlMediaCustom { get; set; }
        public string objetivo { get; set; }
        public string nivel { get; set; }
        public string descripcion { get; set; }
        public bool esCalibracion { get; set; }
        public int duracion { get; set; }

        public bool home { get; set; }
        public bool pro { get; set; }
        public bool beauty { get; set; }
        public bool libre { get; set; }
        public bool easy { get; set; }
        
        public string stampu { get; set; }

        public int idFamilia { get; set; }

        [JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime? dtStampu { get; set; }

        [JsonProperty("entrenamientoPrograma")]
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TrackModel> tracks { get; set; }
    
        public static int getNumProperties()
        {
            return 21;
        }

        public static string getName()
        {
            return nameof(SessionModel);
        }

        public SessionModel()
        {

        }

        public bool ShouldSerializeid()
        {
            return id > 0;
        }

    }


}