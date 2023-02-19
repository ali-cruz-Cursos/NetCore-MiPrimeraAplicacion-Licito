using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MiPrimeraAplicacionEnNetCore.Clases
{
    public class SedeCLS
    {
        [Display(Name = "ID Sede")]
        [DisplayName("ID Sede")]
        public int iidSede { get; set; }

        [Display(Name = "Nombre Sede")]
        [DisplayName("Nombre Sede")]
        [Required(ErrorMessage = "Debe ingresar nombre de la sede")]
        public string nombreSede { get; set; }

        [Display(Name = "Dirección")]
        [DisplayName("Direccion")]
        [Required(ErrorMessage = "Ingrese direccion de la sede")]
        public string direccion { get; set; }
    }
}
