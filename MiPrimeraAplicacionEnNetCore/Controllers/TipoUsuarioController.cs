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
    }
}
