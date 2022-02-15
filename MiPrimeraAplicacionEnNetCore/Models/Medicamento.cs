using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MiPrimeraAplicacionEnNetCore.Models
{
    public partial class Medicamento
    {
        public Medicamento()
        {
            CitaMedicamentos = new HashSet<CitaMedicamento>();
        }
        
        public int Iidmedicamento { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Ingrese la concentracion")]
        public string Concentracion { get; set; }
        public int? Iidformafarmaceutica { get; set; }

        [Required(ErrorMessage = "Precio")]
        public decimal? Precio { get; set; }

        [Required(ErrorMessage = "Stock")]
        public int? Stock { get; set; }

        [Required(ErrorMessage = "Presentacion")]
        public string Presentacion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual FormaFarmaceutica IidformafarmaceuticaNavigation { get; set; }
        public virtual ICollection<CitaMedicamento> CitaMedicamentos { get; set; }
    }
}
