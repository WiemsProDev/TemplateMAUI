using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Template.Models.Entrenamiento
{
    public partial class EntrenamientoClienteBindableModel : BindableObject
    {
        public EntrenamientoClienteModel entrenamiento { get; set; }
        private bool seleccionado;

        public bool Seleccionado
        {
            get { return seleccionado; }
            set
            {
                if (seleccionado != value)
                {
                    seleccionado = value;
                    OnPropertyChanged(nameof(Seleccionado));
                }
            }
        }

    }
}

