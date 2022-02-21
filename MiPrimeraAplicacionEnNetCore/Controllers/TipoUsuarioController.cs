using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionEnNetCore.Models;
using MiPrimeraAplicacionEnNetCore.Clases;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class TipoUsuarioController : Controller
    {
        public IActionResult Index(TipoUsuarioCLS oTipoUsuarioCLS)
        {
            List<TipoUsuarioCLS> listaTipoUsuarios = new List<TipoUsuarioCLS>();

            using(BDHospitalContext db = new BDHospitalContext())
            {
                listaTipoUsuarios = (from listaTipoUsuario in db.TipoUsuarios
                                     where listaTipoUsuario.Bhabilitado == 1
                                     select new TipoUsuarioCLS
                                     {
                                         idTipoUsuario = listaTipoUsuario.Iidtipousuario,
                                         nombre = listaTipoUsuario.Nombre,
                                         descripcion = listaTipoUsuario.Descripcion
                                     }).ToList();

                if (oTipoUsuarioCLS.nombre == null && oTipoUsuarioCLS.descripcion == null &&
                    oTipoUsuarioCLS.idTipoUsuario == 0)
                {
                    ViewBag.Nombre = "";
                    ViewBag.Descripcion = "";
                    ViewBag.idTipoUser = 0;
                } else
                {
                    if (oTipoUsuarioCLS.nombre != null)
                    {
                        listaTipoUsuarios = listaTipoUsuarios.Where(p => p.nombre.Contains(oTipoUsuarioCLS.nombre)).ToList();
                    }

                    if (oTipoUsuarioCLS.idTipoUsuario != 0) {
                        listaTipoUsuarios = listaTipoUsuarios.Where(p => p.idTipoUsuario == oTipoUsuarioCLS.idTipoUsuario).ToList();
                    }

                    if (oTipoUsuarioCLS.descripcion != null)
                    {
                        listaTipoUsuarios = listaTipoUsuarios.Where(p => p.descripcion.Contains(oTipoUsuarioCLS.descripcion)).ToList();
                    }

                    ViewBag.Nombre = oTipoUsuarioCLS.nombre;
                    ViewBag.Id = oTipoUsuarioCLS.idTipoUsuario;
                    ViewBag.Descripcion = oTipoUsuarioCLS.descripcion;
                }
            }
            return View(listaTipoUsuarios);
        }


        [HttpPost]
        public IActionResult Eliminar(int idTipoUsuario)
        {
            using(BDHospitalContext db = new BDHospitalContext())
            {
                TipoUsuario oTipoUsuario = db.TipoUsuarios.Where(p => p.Iidtipousuario == idTipoUsuario).First();
                db.TipoUsuarios.Remove(oTipoUsuario);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Agregar()
        {
            return View();
        }

        // Guarda nuevo usuario en la bd.
        [HttpPost]
        public IActionResult Guardar(TipoUsuarioCLS oTipoUsuarioCLS)
        {
            string nombreVista = "";
            int nvecesNombre = 0;
            int nvecesDescription = 0;

            try
            {
                // Nuevo registro
                if (oTipoUsuarioCLS.idTipoUsuario == 0)
                {
                    nombreVista = "Agregar";
                } else
                {
                    nombreVista = "Editar";
                }
                
                using (BDHospitalContext bd = new BDHospitalContext())
                {
                    if (oTipoUsuarioCLS.idTipoUsuario == 0)
                    {
                        // Nuevo registro
                        nvecesNombre = bd.TipoUsuarios.Where(p => p.Nombre.ToUpper().Trim() == oTipoUsuarioCLS.nombre.ToUpper().Trim()).Count();
                        nvecesDescription = bd.TipoUsuarios.Where(p => p.Descripcion.ToUpper().Trim() == oTipoUsuarioCLS.descripcion.ToUpper().Trim()).Count();
                    } else
                    {
                        // Edicion
                        nvecesNombre = bd.TipoUsuarios.Where(p => p.Nombre.ToUpper().Trim() == oTipoUsuarioCLS.nombre.ToUpper().Trim() && p.Iidtipousuario != oTipoUsuarioCLS.idTipoUsuario).Count();
                        nvecesDescription = bd.TipoUsuarios.Where(p => p.Descripcion.ToUpper().Trim() == oTipoUsuarioCLS.descripcion.ToUpper().Trim() && p.Iidtipousuario != oTipoUsuarioCLS.idTipoUsuario).Count();
                    }


                    if (!ModelState.IsValid || nvecesNombre >= 1 || nvecesDescription >= 1)
                    {
                        if (nvecesNombre >= 1)
                        {
                            oTipoUsuarioCLS.mensajeErrorNombre = "Nombre ya existe";
                        }
                        if (nvecesDescription >= 1)
                        {
                            oTipoUsuarioCLS.mensajeErrorDescripcion = "Descripcion ya existe";
                        }

                        return View(nombreVista, oTipoUsuarioCLS);
                    } else {
                        TipoUsuario oTipoUsuario = new TipoUsuario()
                        {
                            Nombre = oTipoUsuarioCLS.nombre,
                            Descripcion = oTipoUsuarioCLS.descripcion,
                            Bhabilitado = 1
                        };
                        bd.TipoUsuarios.Add(oTipoUsuario);
                        bd.SaveChanges();
                    }
                }
            } catch (Exception e)
            {
                return View(nombreVista, oTipoUsuarioCLS);
            }

            return RedirectToAction("Index");
        }
    }
}
