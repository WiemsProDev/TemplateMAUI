namespace Template.Models
{
    public class MenuModel
    {
        [SQLite.PrimaryKey]
        public int id { get; set; }
        public int rol { get; set; }
        public string nombre { get; set; }
        public string nombre_ingles { get; set; }
        public string viewmodel { get; set; }
        public string imagen { get; set; }
        public bool visible { get; set; }
        public int orden { get; set; }
        public int idParent { get; set; }
        public bool easy { get; set; }
        public static int getNumProperties()
        {
            return 10;
        }

        public static string getName()
        {
            return nameof(MenuModel);
        }
    }
}
