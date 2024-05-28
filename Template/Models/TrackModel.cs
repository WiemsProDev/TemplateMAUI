using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Template.Models
{
    public class TrackModel
    {
        [PrimaryKey]
        public int id { get; set; }
        public int orden { get; set; }
        public int preparacion { get; set; }
        public int descanso { get; set; }
        public int intensidad { get; set; }
        public int duracion { get; set; }
        public int zonaHRobj { get; set; }

       [JsonProperty("sessionProgram")]
       [OneToOne(CascadeOperations = CascadeOperation.All)]
       public ProgramModel programa { get; set; }

       [JsonProperty("ejercicios")]
       [OneToMany(CascadeOperations = CascadeOperation.All)]
       public List<EjercicioServerModel> ejercicios { get; set; }


        [ForeignKey(typeof(SessionModel))]
        public int entrenamientoId { get; set; }

        public static int getNumProperties()
        {
           return 8;
        }

        public static string getName()
        {
            return nameof(TrackModel);
        }

        public TrackModel()
        {

        }


        public bool ShouldSerializeid()
        {
            return false;
        }

        public bool ShouldSerializeentrenamientoId()
        {
            return (false);
        }


    }
        
}