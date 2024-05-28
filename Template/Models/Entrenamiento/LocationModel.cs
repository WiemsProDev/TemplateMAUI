using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Template.Models.Entrenamiento
{
    public class LocationModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string address { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        [ForeignKey(typeof(EntrenamientoClienteModel))]
        public int EntrenamientoId { get; set; }

        public static int getNumProperties()
        {
            return 5;
        }

        public static string getName()
        {
            return nameof(LocationModel);
        }


        public bool ShouldSerializeId()
        {
            return (false);
        }

        public bool ShouldSerializeEntrenamientoId()
        {
            return (false);
        }
    }
}

