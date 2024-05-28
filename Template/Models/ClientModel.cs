using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Template.Converters;

namespace Template.Models
{
    public class ClientModel
    {
        [PrimaryKey]
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string alias { get; set; }
        public string userId { get; set; }
        public string address { get; set; }
        public int age { get; set; }
        public int numTrainings { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public string hrm_uuid { get; set; }
        public string hrm_model { get; set; }
        public string town { get; set; }
        public string dni { get; set; }
        public string cellPhone { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string emailBase { get; set; }
        public string postal_code { get; set; }
        public string objective { get; set; }
        public DateTime birthdate { get; set; }

        [Ignore]
        public int edad { get; set; }

        public int? idTrainer { get; set; }
        public int estadoForma { get; set; }
        public string imgUrl { get; set; }
        public string platform { get; set; }
        public string oneSignalToken { get; set; }

        [JsonIgnore]
        public DateTime dtEditionDate { get; set; }

        public string editionDate { get; set; }
        public int hrReposo { get; set; }
        public int motivacion { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool shared { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool enabled { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool medicacion { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool bidireccional { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool enfermedad { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool activo { get; set; }
        public string medicacion_info { get; set; }
        public string enfermedad_info { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool parq1 { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool parq2 { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool parq3 { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool parq4 { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool parq5 { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool parq6 { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool parq7 { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool parq8 { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool clienteHome { get; set; }
        public string username { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool emailVerificado { get; set; }
        public string password { get; set; }
        public int rol { get; set; }
        public double vo2max { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool virtualCoach { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool avisoVisual { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool avisoAcustico { get; set; }

        public double tiempoSubida { get; set; }
        [JsonConverter(typeof(BoolToBitJsonConverter))]
        public bool tourRealizado { get; set; }

        [Ignore, JsonIgnore]
        public string progreso { get; set; }

        [Ignore, JsonIgnore]
        public string nextSesion { get; set; }

        [Ignore]
        public bool tienePlan { get; set; }
        public bool toSend { get; set; }
        public static int getNumProperties()
        {
            return 58;
        }

        public static string getName()
        {
            return nameof(ClientModel);
        }
        public bool ShouldSerializetoSend()
        {
            return (false);
        }
        public void calculaEdad()
        {
            try
            {
                if (birthdate != null)
                {
                    try { edad = DateTime.Today.AddTicks(-birthdate.Ticks).Year - 1; }
                    catch { edad = 0; }
                }
            }
            catch (Exception)
            {
                
                edad = 0;
            }
        }

        public int getHRMaxTeorica()
        {
            try
            {
                calculaEdad();

                int hrMax = 0;
                if (gender.Equals("H"))
                    hrMax = (int)(214 - (0.8 * edad));
                else
                    hrMax = (int)(209 - (0.7 * edad));

                return hrMax;
            }
            catch (Exception)
            {
                
                return 0;
            }
        }

        
    }
}
