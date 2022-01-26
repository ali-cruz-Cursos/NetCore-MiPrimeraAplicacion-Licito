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
        public IActionResult Index(SedeCLS oSedeCLS)
        {
            List<SedeCLS> listaSede = new List<SedeCLS>();

            using(BDHospitalContext db = new BDHospitalContext())
            {
                if (oSedeCLS.nombreSede == null || oSedeCLS.nombreSede == "")
                {
                    listaSede = (from sede in db.Sedes
                                 where sede.Bhabilitado == 1
                                 select new SedeCLS
                                 {
                                     iidSede = sede.Iidsede,
                                     nombreSede = sede.Nombre,
                                     direccion = sede.Direccion
                                 }).ToList();
                    ViewBag.nombreSede = "";
                } else
                {
                    listaSede = (from sede in db.Sedes
                                 where sede.Bhabilitado == 1 && sede.Nombre.Contains(oSedeCLS.nombreSede)
                                 select new SedeCLS
                                 {
                                     iidSede = sede.Iidsede,
                                     nombreSede = sede.Nombre,
                                     direccion = sede.Direccion
                                 }).ToList();
                    ViewBag.nombreSede = oSedeCLS.nombreSede;
                }
            }
            return View(listaSede);
        }

        [HttpPost]
        public IActionResult Eliminar(int idsede)
        {
            using(BDHospitalContext db = new BDHospitalContext())
            {
                Sede oSede = db.Sedes.Where(p => p.Iidsede == idsede).First();
                oSede.Bhabilitado = 0;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
