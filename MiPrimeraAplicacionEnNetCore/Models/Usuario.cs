using System;
using System.Collections.Generic;

#nullable disable

namespace MiPrimeraAplicacionEnNetCore.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Cita = new HashSet<Citum>();
            HistorialCita = new HashSet<HistorialCitum>();
            Personas = new HashSet<Persona>();
        }

        public int Iidusuario { get; set; }
        public int Iidtipousuario { get; set; }
        public string Nombreusuario { get; set; }
        public string Contraseña { get; set; }
        public int? Bhabilitado { get; set; }
        public int? Iidpersona { get; set; }

        public virtual Persona IidpersonaNavigation { get; set; }
        public virtual TipoUsuario IidtipousuarioNavigation { get; set; }
        public virtual ICollection<Citum> Cita { get; set; }
        public virtual ICollection<HistorialCitum> HistorialCita { get; set; }
        public virtual ICollection<Persona> Personas { get; set; }
    }
}
