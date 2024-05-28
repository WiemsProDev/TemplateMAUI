using SQLite;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;

namespace Template.Models
{
	public class EjercicioServerModel
	{
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int idEjercicio { get; set; }
        public int orden { get; set; }
        public int duracion { get; set; }
        public int descanso { get; set; }
        public int zonaHRObj { get; set; }

        [ForeignKey(typeof(TrackModel))]
        public int trackId { get; set; }

        [Ignore, JsonIgnore]
        public EjercicioModel ejercicio { get; set; }

        public static int getNumProperties()
        {
            return 7;
        }

        public static string getName()
        {
            return nameof(EjercicioServerModel);
        }

        public EjercicioServerModel()
        {
        }

        public bool ShouldSerializeid()
        {
            if (id == 0)
                return false;
            else
                return true;
        }
    }
}

