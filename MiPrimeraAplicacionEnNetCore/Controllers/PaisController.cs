using Microsoft.AspNetCore.Mvc;
using MiPrimeraAplicacionEnNetCore.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class PaisController : Controller
    {
        public IActionResult Inicio()
        {
            return View();
        }

        public IActionResult Lista()
        {
            return View();
        }

        public string bienvenido()
        {
            return "Bienvenido al curso";
        }

        // localhost/Pais/saludoPersona/?nombre=Ali&paterno=C&materno=M


        public string saludoPersona(InstructorCLS instructor)
        {
            return "Hola " + instructor.nombre + " " + instructor.paterno + " " + instructor.materno;
        }

        public string saludoPais(string nombre)
        {
            return "Hola pais " + nombre + "!";
        }

        public InstructorCLS mostrarInstructor()
        {
            InstructorCLS oInstructorCLS = new InstructorCLS();
            oInstructorCLS.nombre = "Ali";
            oInstructorCLS.paterno = "Cruz";
            oInstructorCLS.materno = "M";
            return oInstructorCLS;
        }

        public List<InstructorCLS> listaInstructores()
        {
            List<InstructorCLS> lista = new List<InstructorCLS>();
            
            InstructorCLS oInstructorCLS = new InstructorCLS();
            oInstructorCLS.nombre = "Ali";
            oInstructorCLS.paterno = "Cruz";
            oInstructorCLS.materno = "Monter";
            lista.Add(oInstructorCLS);

            oInstructorCLS = new InstructorCLS();
            oInstructorCLS.nombre = "Sara";
            oInstructorCLS.paterno = "Cruz";
            oInstructorCLS.materno = "D";
            lista.Add(oInstructorCLS);

            return lista;

        }
    }
}
