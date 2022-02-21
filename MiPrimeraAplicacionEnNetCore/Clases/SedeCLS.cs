using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAplicacionEnNetCore.Clases
{
    public class SedeCLS
    {
        [Display(Name = "ID Sede")]
        public int iidSede { get; set; }

        [Display(Name = "Nombre Sede")]
        [Required(ErrorMessage = "Debe ingresar nombre de la sede")]
        public string nombreSede { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Ingrese direccion de la sede")]
        public string direccion { get; set; }
    }
}
