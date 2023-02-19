using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAplicacionEnNetCore.Clases
{
    public class EspecialidadCLS
    {
        [Display(Name = "ID Especialidad")]
        [DisplayName("ID Especialidad")]
        public int iidEspecialidad { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Ingrese el nombre de la especialidad")]
        [DisplayName("Nombre de la especialidad")]
        public string nombre { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Ingrese descripcion")]
        [DisplayName("Descripcion de la especialidad")]
        public string descripcion { get; set; }

        [Display(Name = "Mensaje Error")]
        [DisplayName("Mensaje de Error")]
        public string mensajeError { get; set; }


    }
}
