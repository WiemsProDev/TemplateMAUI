using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Template.Models
{
    public class SideMenuItemModel:BindableObject
    {
        public int idMenu { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }

        public bool TieneHijos { get; set; }
        public List<SideMenuItemModel> Hijos { get; set; }

        private bool selec;
        public bool Selec
        {
            get { return selec; }
            set
            {
                if (selec != value)
                {
                    selec = value;
                    OnPropertyChanged(nameof(Selec));
                }
            }
        }
    }
}
