using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAplicacionEnNetCore.Clases
{
    public class MedicamentoCLS
    {
        [Display(Name = "ID Medicamento")]
        
        public int idMedicamento { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Ingrese nombre del medicamento")]
        public string nombre { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "Ingrese el precio")]
        public double precio { get; set; }

        [Display(Name = "Stock")]
        [Required(ErrorMessage = "Ingrese el stock")]
        [Range(0,10000, ErrorMessage = "Debe estar en el rango de 0 a 10000")]
        public int stock { get; set; }

        [Display(Name = "Forma Farmaceutica")]
        public string formaFarmaceutica { get; set; }

        [Display(Name = "Seleccione forma farmacuetica")]
        [Required(ErrorMessage = "Ingrese la forma farmaceutica")]
        public int idFormaFarmaceutica { get; set; }

        [Display(Name = "Concentracion")]
        public string concentracion { get; set; }

        [Display(Name = "Presentacion")]
        public string presentacion { get; set; }






    }
}
