using Microsoft.AspNetCore.Mvc;
using MiPrimeraAplicacionEnNetCore.Clases;
using MiPrimeraAplicacionEnNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class SedeController : Controller
    {
        public IActionResult Index()
        {
            List<SedeCLS> listaSede = new List<SedeCLS>();

            using(BDHospitalContext db = new BDHospitalContext())
            {
                listaSede = (from sede in db.Sedes
                            where sede.Bhabilitado == 1
                            select new SedeCLS
                            {
                                iidSede = sede.Iidsede,
                                nombreSede = sede.Nombre,
                                direccion = sede.Direccion
                            }).ToList();
            }
            return View(listaSede);
        }
    }
}
