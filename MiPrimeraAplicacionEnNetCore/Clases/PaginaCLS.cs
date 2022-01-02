using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAplicacionEnNetCore.Clases
{
    public class PaginaCLS
    {
        [Display(Name = "ID Pagina")]
        public int idPagina { get; set; }

        [Display(Name = "Mensaje")]
        [Required(ErrorMessage = "Ingrese mensaje")]
        public string mensaje { get; set; }

        [Display(Name = "Accion")]
        [Required(ErrorMessage = "Ingrese accion")]
        public string accion { get; set; }

        [Display(Name = "Controlador")]
        [Required(ErrorMessage = "Ingrese controlador")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ingrese entre 3 y 100 caracteres")]
        public string controlador { get; set; }

    }
}
