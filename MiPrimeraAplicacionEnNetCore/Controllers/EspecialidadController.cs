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

        public IActionResult Editar(int id)
        {
            EspecialidadCLS oEspecialidadCLS = new EspecialidadCLS();
            using(BDHospitalContext db = new BDHospitalContext())
            {
                oEspecialidadCLS = (from especialidad in db.Especialidad
                                    where especialidad.Iidespecialidad == id
                                    select new EspecialidadCLS
                                    {
                                        iidEspecialidad = especialidad.Iidespecialidad,
                                        nombre = especialidad.Nombre,
                                        descripcion = especialidad.Descripcion
                                    }).First();

            }
            return View(oEspecialidadCLS);
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
        public IActionResult Guardar(EspecialidadCLS oEspecialidadCLS)
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
                        if (oEspecialidadCLS.iidEspecialidad == 0)
                        {
                            Especialidad objetoEspecialidad = new Especialidad();
                            objetoEspecialidad.Nombre = oEspecialidadCLS.nombre;
                            objetoEspecialidad.Descripcion = oEspecialidadCLS.descripcion;
                            objetoEspecialidad.Bhabilitado = 1;
                            db.Especialidad.Add(objetoEspecialidad);
                            db.SaveChanges();
                        } else
                        {
                            Especialidad objeto = db.Especialidad.Where(p => p.Iidespecialidad == oEspecialidadCLS.iidEspecialidad).First();
                            objeto.Nombre = oEspecialidadCLS.nombre;
                            objeto.Descripcion = oEspecialidadCLS.descripcion;
                            db.SaveChanges();
                        }
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
