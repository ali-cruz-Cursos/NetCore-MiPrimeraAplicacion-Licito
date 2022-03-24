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
            string nombreVista = "";
            int nveces = 0;

            try
            {
                if (oEspecialidadCLS.iidEspecialidad == 0) 
                    nombreVista = "Agregar";
                else 
                    nombreVista = "Editar";

                using (BDHospitalContext db = new BDHospitalContext())
                {

                    // Validar si ya existe 
                    
                    if (oEspecialidadCLS.iidEspecialidad == 0)
                    {
                        // Agregar nuevo registro
                        nveces = db.Especialidad
                                    .Where(p => p.Nombre
                                    .ToUpper()
                                    .Trim() == oEspecialidadCLS.nombre
                                    .ToUpper()
                                    .Trim())
                                    .Count();
                    } else
                    {
                        // Editar registro
                        // Buscar las repeticiones del registro PERO descartando el ID consultado

                        nveces = db.Especialidad
                                    .Where(p => p.Nombre
                                    .ToUpper()
                                    .Trim() == oEspecialidadCLS.nombre
                                    .ToUpper()
                                    .Trim() && 
                                    p.Iidespecialidad != oEspecialidadCLS.iidEspecialidad &&
                                    p.Bhabilitado == 1)
                                    .Count();
                    }

                     if (!ModelState.IsValid || nveces >= 1)
                    {
                        if (nveces >= 1)
                        {
                            oEspecialidadCLS.mensajeError = "El nombre especialidad ya existe";
                            return View(nombreVista, oEspecialidadCLS);
                        }
                        
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
                            Especialidad objeto = db.Especialidad
                                                    .Where(p => p.Iidespecialidad == oEspecialidadCLS.iidEspecialidad)
                                                    .First();
                            objeto.Nombre = oEspecialidadCLS.nombre;
                            objeto.Descripcion = oEspecialidadCLS.descripcion;
                            db.SaveChanges();
                        }
                    }
                }
            } catch (Exception e)
            {
                return View(nombreVista, oEspecialidadCLS);
            }

            return RedirectToAction("Index");
        }
    }
}
