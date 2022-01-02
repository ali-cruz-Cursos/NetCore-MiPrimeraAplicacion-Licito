using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionEnNetCore.Clases;
using MiPrimeraAplicacionEnNetCore.Models;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class PaginaController : Controller
    {
        public IActionResult Index(PaginaCLS oPaginaCLS)
        {
            List<PaginaCLS> listaPaginas = new List<PaginaCLS>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                ViewBag.Mensaje = oPaginaCLS.mensaje;
                
                    listaPaginas = (from pagina in db.Paginas
                                    where pagina.Bhabilitado == 1 &&
                                    ((string.IsNullOrEmpty(oPaginaCLS.mensaje)) || 
                                    pagina.Mensaje.Contains(oPaginaCLS.mensaje))
                                    select new PaginaCLS
                                    {
                                        idPagina = pagina.Iidpagina,
                                        mensaje = pagina.Mensaje,
                                        accion = pagina.Accion,
                                        controlador = pagina.Controlador
                                    }).ToList();
                    
                

                //if (oPaginaCLS.mensaje == null)
                //{
                //    listaPaginas = (from pagina in db.Paginas
                //                    where pagina.Bhabilitado == 1 &&
                //                    ((string.IsNullOrEmpty(oPaginaCLS.mensaje)) || 
                //                    select new PaginaCLS
                //                    {
                //                        idPagina = pagina.Iidpagina,
                //                        mensaje = pagina.Mensaje,
                //                        accion = pagina.Accion,
                //                        controlador = pagina.Controlador
                //                    }).ToList();
                //    ViewBag.Mensaje = "";
                //} else
                //{
                //    listaPaginas = (from pagina in db.Paginas
                //                    where pagina.Bhabilitado == 1 &&
                //                    pagina.Mensaje.Contains(oPaginaCLS.mensaje)
                //                    select new PaginaCLS
                //                    {
                //                        idPagina = pagina.Iidpagina,
                //                        mensaje = pagina.Mensaje,
                //                        accion = pagina.Accion,
                //                        controlador = pagina.Controlador
                //                    }).ToList();
                //    ViewBag.Mensaje = oPaginaCLS.mensaje;
                //}
            }

                return View(listaPaginas);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(PaginaCLS oPaginaCLS)
        {
            try
            {
                using(BDHospitalContext db = new BDHospitalContext())
                {
                    if (!ModelState.IsValid)
                    {
                        return View(oPaginaCLS);
                    }
                    else
                    {
                        Pagina oPagina = new Pagina();

                        oPagina.Mensaje = oPaginaCLS.mensaje;
                        oPagina.Controlador = oPaginaCLS.controlador;
                        oPagina.Accion = oPaginaCLS.accion;
                        oPagina.Bhabilitado = 1;
                        db.Paginas.Add(oPagina);
                        db.SaveChanges();
                    }
                }

            } catch (Exception e)
            {
                return View(oPaginaCLS);
            }

            return RedirectToAction("Index");
        }
    }
}
