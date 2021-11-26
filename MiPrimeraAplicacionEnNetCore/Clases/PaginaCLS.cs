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
        public string mensaje { get; set; }

        [Display(Name = "Accion")]
        public string accion { get; set; }

        [Display(Name = "Controlador")]
        public string controlador { get; set; }


    }
}
