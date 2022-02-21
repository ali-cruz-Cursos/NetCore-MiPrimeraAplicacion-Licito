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

        public IActionResult Editar(int id)
        {
            SedeCLS oSedeCLS = new SedeCLS();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                oSedeCLS = (from sede in db.Sedes
                            where sede.Iidsede == id
                            select new SedeCLS
                            {
                                iidSede = sede.Iidsede,
                                nombreSede = sede.Nombre,
                                direccion = sede.Direccion
                            }).First();
            }
            return View(oSedeCLS);
        }


        [HttpPost]
        public IActionResult Guardar(SedeCLS oSedeCLS)
        {

            string nombreVista = "";

            if (oSedeCLS.iidSede == 0)
                nombreVista = "Agregar";
            else
                nombreVista = "Editar";

            if (!ModelState.IsValid)
            {
                return View(nombreVista, oSedeCLS);
            } else
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    if (oSedeCLS.iidSede != 0)
                    {
                        Sede oSede = db.Sedes.Where(p => p.Iidsede == oSedeCLS.iidSede).First();
                        oSede.Nombre = oSedeCLS.nombreSede;
                        oSede.Direccion = oSedeCLS.direccion;
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");

        }
    }
}
