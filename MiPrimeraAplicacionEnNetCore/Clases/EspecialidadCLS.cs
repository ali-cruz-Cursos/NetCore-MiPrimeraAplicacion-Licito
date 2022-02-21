using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAplicacionEnNetCore.Clases
{
    public class EspecialidadCLS
    {
        [Display(Name = "ID Especialidad")]
        public int iidEspecialidad { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Ingrese el nombre de la especialidad")]
        public string nombre { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Ingrese descripcion")]
        public string descripcion { get; set; }

        public string mensajeError { get; set; }


    }
}
