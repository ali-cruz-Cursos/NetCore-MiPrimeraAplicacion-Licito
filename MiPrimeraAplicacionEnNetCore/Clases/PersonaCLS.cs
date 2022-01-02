using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAplicacionEnNetCore.Clases
{
    public class PersonaCLS
    {
        [Display(Name = "ID Persona")]
        public int iidPersona { get; set; }

        [Display(Name = "Nombre")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Ingrese entre 3 y 25 caracteres")]
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string nombre { get; set;}

        [Required(ErrorMessage = "Ingrese un paterno")]
        [Display(Name = "Paterno")]
        public string aPaterno { get; set; }

        [Required(ErrorMessage = "Ingrese un materno")]
        [Display(Name = "Materno")]
        public string aMaterno { get; set; }

        [Display(Name = "Telefono fijo")]
        public string telefonoFijo { get; set; }

        [Display(Name = "Celular")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Ingrese solo 12 caracteres")]
        [Required(ErrorMessage = "Ingrese un celular")]
        public string telefonoCelular { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]        
        [Required(ErrorMessage = "Ingrese una fecha")]
        public DateTime? fechaNacimiento { get; set; }

        [Required(ErrorMessage = "Ingrese un sexo valido")]
        [Display(Name = "Sexo")]
        public int? iidsexo { get; set; }

        [Display(Name = "Nombre Completo")]
        public string nombreCompleto { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        [Required(ErrorMessage = "Ingrese un email")]
        public string email { get; set; }

        [Display(Name = "Sexo")]
        public string nombreSexo { get; set; }

    } 
}
