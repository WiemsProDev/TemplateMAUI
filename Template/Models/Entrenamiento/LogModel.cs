using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Template.Models.Entrenamiento
{
    public class LogModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string info { get; set; }

        [ForeignKey(typeof(EntrenamientoClienteModel))]
        public int EntrenamientoId { get; set; }

        public static int getNumProperties()
        {
            return 3;
        }

        public static string getName()
        {
            return nameof(LogModel);
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

