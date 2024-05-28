using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Template.Models.Entrenamiento
{
    public class LogBigDataModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int i1 { get; set; }
        public int i2 { get; set; }
        public int i3 { get; set; }
        public int i4 { get; set; }
        public int i5 { get; set; }
        public int i6 { get; set; }
        public int i7 { get; set; }
        public int i8 { get; set; }
        public int i9 { get; set; }
        public int i10 { get; set; }

        public int c1 { get; set; }
        public int c2 { get; set; }
        public int c3 { get; set; }
        public int c4 { get; set; }
        public int c5 { get; set; }
        public int c6 { get; set; }
        public int c7 { get; set; }
        public int c8 { get; set; }
        public int c9 { get; set; }
        public int c10 { get; set; }

        public int w1 { get; set; }
        public int w2 { get; set; }
        public int w3 { get; set; }
        public int w4 { get; set; }
        public int w5 { get; set; }
        public int w6 { get; set; }
        public int w7 { get; set; }
        public int w8 { get; set; }
        public int w9 { get; set; }
        public int w10 { get; set; }

        public string date { get; set; }      
        public int frec { get; set; }    
        public int working_time { get; set; }
        public int resting_time { get; set; }
        public int rise { get; set; }
        public int fall { get; set; }
        public int hr { get; set; }
        
        [ForeignKey(typeof(EntrenamientoClienteModel))]
        public int EntrenamientoId { get; set; }

        public static int getNumProperties()
        {
            return 39;
        }

        public static string getName()
        {
            return nameof(LogBigDataModel);
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

