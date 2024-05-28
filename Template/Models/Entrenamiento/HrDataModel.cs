using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Template.Models.Entrenamiento
{
    public class HrDataModel
    {  
            [PrimaryKey, AutoIncrement]
            public int id { get; set; }
            public int hr { get; set; }
            public long tiempo { get; set; }

            [ForeignKey(typeof(EntrenamientoClienteModel))]
            public int EntrenamientoId { get; set; }
            [JsonIgnore,Ignore]
            public string training { get; set; }

        public static int getNumProperties()
        {
            return 4;
        }

        public static string getName()
        {
            return nameof(HrDataModel);
        }

        public HrDataModel()
        {

        }

        public HrDataModel(int time, int puls)
        {
            hr = puls;
            tiempo = time; 
        }

        //Evita que serialice la propiedad id, pero si que la deserializa
        public bool ShouldSerializeid()
        {
            return (false);
        }

        public bool ShouldSerializeEntrenamientoId()
        {
            return (false);
        }
        public bool ShouldSerializetraining()
        {
            return (false);
        }
    }
}

