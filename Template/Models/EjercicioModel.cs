using Newtonsoft.Json;
using SQLite;
using Template.Converters;

namespace Template.Models
{
	public class EjercicioModel
	{
        [PrimaryKey]
        public int id { get; set; }
        public string descripcion { get; set; }
        public string nombre { get; set; }
        public string urlMedia { get; set; }
        public string urlPicture { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool enabled { get; set; }
        public string nombreIngles { get; set; }
        public string descripcionIngles { get; set; }
        public int? idManager { get; set; }
        public int idZonaCorporal { get; set; }
        public int idTipoMaterial { get; set; }
        public int idTipoCualidadFisica { get; set; }


        public static int getNumProperties()
        {
            return 12;
        }

        public static string getName()
        {
            return nameof(EjercicioModel);
        }

        public EjercicioModel()
		{
		}
	}
}

