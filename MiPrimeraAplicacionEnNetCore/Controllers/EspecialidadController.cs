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
        public IActionResult Index(EspecialidadCLS oEspecialidadCLS)
        {
            ViewBag.mensaje = "Mensaje desde el controlador hacia la vista";


            List<EspecialidadCLS> listaEspecialidad = new List<EspecialidadCLS>();

            using(BDHospitalContext bd = new BDHospitalContext())
            {
                if (oEspecialidadCLS.nombre == null || oEspecialidadCLS.nombre == "")
                {
                    listaEspecialidad = (from vEspecialidad in bd.Especialidad
                                         where vEspecialidad.Bhabilitado == 1
                                         select new EspecialidadCLS
                                         {
                                             iidEspecialidad = vEspecialidad.Iidespecialidad,
                                             nombre = vEspecialidad.Nombre,
                                             descripcion = vEspecialidad.Descripcion
                                         }).ToList();
                    ViewBag.nombreEspecialidad = "";
                } else
                {
                    listaEspecialidad = (from vEspecialidad in bd.Especialidad
                                         where vEspecialidad.Bhabilitado == 1 &&
                                         vEspecialidad.Nombre.Contains(oEspecialidadCLS.nombre)
                                         select new EspecialidadCLS
                                         {
                                             iidEspecialidad = vEspecialidad.Iidespecialidad,
                                             nombre = vEspecialidad.Nombre,
                                             descripcion = vEspecialidad.Descripcion
                                         }).ToList();
                    ViewBag.nombreEspecialidad = oEspecialidadCLS.nombre;
                }
            }
            return View(listaEspecialidad);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Eliminar(int Iidespecialidad)
        {
            string error;

            try
            {
                using(BDHospitalContext db = new BDHospitalContext())
                {
                    Especialidad oEspecialidad = db.Especialidad
                        .Where(p => p.Iidespecialidad == Iidespecialidad).First();
                                                    oEspecialidad.Bhabilitado = 0;
                    db.SaveChanges();
                }

            } catch(Exception ex)
            {
                error = ex.Message;

            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Agregar(EspecialidadCLS oEspecialidadCLS)
        {
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                     if (!ModelState.IsValid)
                    {
                        return View(oEspecialidadCLS);
                    } else
                    {
                        Especialidad objetoEspecialidad = new Especialidad();
                        objetoEspecialidad.Nombre = oEspecialidadCLS.nombre;
                        objetoEspecialidad.Descripcion = oEspecialidadCLS.descripcion;
                        objetoEspecialidad.Bhabilitado = 1;
                        db.Especialidad.Add(objetoEspecialidad);
                        db.SaveChanges();
                    }
                }
            } catch (Exception e)
            {
                return View(oEspecialidadCLS);
            }

            return RedirectToAction("Index");
        }
    }
}
