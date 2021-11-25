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
        public string nombreSede { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }
    }
}
