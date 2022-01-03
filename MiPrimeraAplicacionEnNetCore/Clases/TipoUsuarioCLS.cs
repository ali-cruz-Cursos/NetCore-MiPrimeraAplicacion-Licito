using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAplicacionEnNetCore.Clases
{
    public class TipoUsuarioCLS
    {
        [Display(Name = "ID Tipo Usuario")]
        public int idTipoUsuario { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string nombre { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Ingrese una descripcion")]
        public string descripcion { get; set; }

    }
}
