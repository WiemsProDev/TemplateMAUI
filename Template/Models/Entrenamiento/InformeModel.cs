using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Template.Models.Entrenamiento
{
    public class InformeModel
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string nombre { get; set; }
        public int ejercicio { get; set; }
        public int orden { get; set; }
        public int duracion { get; set; }
        public int tiempoInicio { get; set; }
        public int zonaHRobj { get; set; }
        public int hrMax { get; set; }
        public int hrMed { get; set; }
        public int calorias { get; set; }
        public int frec { get; set; }
        public int working { get; set; }
        public int resting { get; set; }

        public int zonaFC { get; set; }
        public int zonaReg { get; set; }
        public int segZ0 { get; set; }
        public int segZ1 { get; set; }
        public int segZ2 { get; set; }
        public int segZ3 { get; set; }
        public int segZ4 { get; set; }
        public int segZ5 { get; set; }

        public int cMax1 { get; set; }
        public int cMax2 { get; set; }
        public int cMax3 { get; set; }
        public int cMax4 { get; set; }
        public int cMax5 { get; set; }
        public int cMax6 { get; set; }
        public int cMax7 { get; set; }
        public int cMax8 { get; set; }
        public int cMax9 { get; set; }
        public int cMax10 { get; set; }

        public int cMed1 { get; set; }
        public int cMed2 { get; set; }
        public int cMed3 { get; set; }
        public int cMed4 { get; set; }
        public int cMed5 { get; set; }
        public int cMed6 { get; set; }
        public int cMed7 { get; set; }
        public int cMed8 { get; set; }
        public int cMed9 { get; set; }
        public int cMed10 { get; set; }


        public int cMaxP1 { get; set; }
        public int cMaxP2 { get; set; }
        public int cMaxP3 { get; set; }
        public int cMaxP4 { get; set; }
        public int cMaxP5 { get; set; }
        public int cMaxP6 { get; set; }
        public int cMaxP7 { get; set; }
        public int cMaxP8 { get; set; }
        public int cMaxP9 { get; set; }
        public int cMaxP10 { get; set; }
        public int cMaxP11 { get; set; }

        public int cMedP1 { get; set; }
        public int cMedP2 { get; set; }
        public int cMedP3 { get; set; }
        public int cMedP4 { get; set; }
        public int cMedP5 { get; set; }
        public int cMedP6 { get; set; }
        public int cMedP7 { get; set; }
        public int cMedP8 { get; set; }
        public int cMedP9 { get; set; }
        public int cMedP10 { get; set; }
        public int cMedP11 { get; set; }

        public string vmed { get; set; }
        public string vmax { get; set; }
        public string rom { get; set; }
        public int reps { get; set; }

        [ForeignKey(typeof(EntrenamientoClienteModel))]
        public int EntrenamientoId { get; set; }

        public static int getNumProperties()
        {
            return 68;
        }

        public static string getName()
        {
            return typeof(InformeModel).Name;
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

