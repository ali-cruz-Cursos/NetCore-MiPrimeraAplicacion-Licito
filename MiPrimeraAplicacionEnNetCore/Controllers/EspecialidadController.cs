using Microsoft.AspNetCore.Mvc;
using MiPrimeraAplicacionEnNetCore.Clases;
using MiPrimeraAplicacionEnNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class EspecialidadController : Controller
    {
        public IActionResult Index()
        {
            List<EspecialidadCLS> listaEspecialidad = new List<EspecialidadCLS>();

            using(BDHospitalContext bd = new BDHospitalContext())
            {
                listaEspecialidad = (from vEspecialidad in bd.Especialidad
                                     where vEspecialidad.Bhabilitado == 1
                                     select new EspecialidadCLS
                                     {
                                         iidEspecialidad = vEspecialidad.Iidespecialidad,
                                         nombre = vEspecialidad.Nombre,
                                         descripcion = vEspecialidad.Descripcion
                                     }).ToList();
            }
            return View(listaEspecialidad);
        }
    }
}
