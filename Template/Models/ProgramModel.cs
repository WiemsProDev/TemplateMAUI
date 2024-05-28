using System;
using SQLite;
using Template.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;

namespace Template.Models
{
    public class ProgramModel
    {
        [PrimaryKey]
        public int id { get; set; }
        public bool enabled { get; set; }
        public int id_trainer { get; set; }
        public int id_manager { get; set; }
        public string nombre { get; set; }
        public string nombreIngles { get; set; }
        public string descripcion { get; set; }
        public string descripcionIngles { get; set; }
        public string tipoCliente { get; set; }
        public string color { get; set; }
        public string nivelEMS { get; set; }
        public int duracion { get; set; }
        public int idFamilia { get; set; }
        public int ciclos { get; set; }
        public int intensidad { get; set; }
        public int tiempo_trabajo { get; set; }
        public int tiempo_descanso { get; set; }
        public int rampa_subida { get; set; }
        public int rampa_bajada { get; set; }
        public int frecuencia { get; set; }
        public int frecuencia_resting { get; set; }
        public int atenuacion_resting { get; set; }
        public bool funcional { get; set; }
        public bool configurable { get; set; }
        public bool temporal { get; set; }
        public bool borrable { get; set; }
        public bool seleccionado { get; set; }

        public string stampu { get; set; }

        [JsonConverter(typeof(UTCDateTimeConverter))]
        public DateTime? dtStampu { get; set; }


        [ForeignKey(typeof(TrackModel))]
        public int trackId { get; set; }

        public static int getNumProperties()
        {
            return 30;
        }

        public static string getName()
        {
            return nameof(ProgramModel);
        }

        public ProgramModel()
        {

        }

        public bool ShouldSerializeid()
        {
            return false;
        }

        public bool ShouldSerializeenabled()
        {
            return false;
        }

        public bool ShouldSerializeid_manager()
        {
            return false;
        }

        public bool ShouldSerializeid_trainer()
        {
            return false;
        }

        public bool ShouldSerializeseleccionado()
        {
            return false;
        }

        public bool ShouldSerializetrackId()
        {
            return false;
        }

        public bool ShouldSerializeidFamilia()
        {
            return false;
        }

    }


}