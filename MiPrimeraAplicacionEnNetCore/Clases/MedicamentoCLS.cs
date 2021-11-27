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
        public string nombre { get; set; }

        [Display(Name = "Precio")]
        public double precio { get; set; }

        [Display(Name = "Stock")]
        public int stock { get; set; }

        [Display(Name = "Forma Farmaceutica")]
        public string formaFarmaceutica { get; set; }

    }
}
