using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionEnNetCore.Clases;
using MiPrimeraAplicacionEnNetCore.Models;
using Microsoft.EntityFrameworkCore;

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


        [HttpPost]
        public IActionResult Elimiar(int Iidpagina)
        {
            string error;
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    Pagina oPagina = db.Paginas.Where(p => p.Iidpagina == Iidpagina).First();
                    db.Paginas.Remove(oPagina);
                    db.SaveChanges();
                }
            } catch (Exception e)
            {
                error = e.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Agregar(bool? errorAgregar = false)
        {
            if (errorAgregar.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Campo 'Mensaje' duplicado. No se permiten duplicados.";
            }

            return View();
        }

        [HttpPost]
        public IActionResult Agregar(PaginaCLS oPaginaCLS)
        {
            bool existeMensaje = false;

            try
            {
                using(BDHospitalContext db = new BDHospitalContext())
                {
                    if (db.Paginas.Where(p => p.Mensaje.ToUpper().Trim() == oPaginaCLS.mensaje.ToUpper().Trim()).Count() >= 1)
                    {
                        existeMensaje = true;
                    }

                    if (ModelState.IsValid && !existeMensaje)
                    {
                        Pagina oPagina = new Pagina();

                        oPagina.Mensaje = oPaginaCLS.mensaje;
                        oPagina.Controlador = oPaginaCLS.controlador;
                        oPagina.Accion = oPaginaCLS.accion;
                        oPagina.Bhabilitado = 1;
                        db.Paginas.Add(oPagina);
                        db.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Agregar), new { id = oPaginaCLS.idPagina, errorAgregar = true });
                    }
                }
            } catch (Exception e)
            {
                return RedirectToAction(nameof(Agregar), new { id = oPaginaCLS.idPagina, errorAgregar = true });
            }
        }

        public IActionResult Editar(int? id, bool? errorEditar = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            PaginaCLS oPaginaCLS = new PaginaCLS();

            BDHospitalContext db = new BDHospitalContext();

            oPaginaCLS = (from p in db.Paginas
                          where p.Iidpagina == id
                          select new PaginaCLS
                          {
                              idPagina = p.Iidpagina,
                              mensaje = p.Mensaje,
                              accion = p.Accion,
                              controlador = p.Controlador
                          }).First();

            if (errorEditar.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "El campo 'Mensaje' ya existe. No pueden existir duplicados";
            }

            return View(oPaginaCLS);
        }

        // Para editar una pagina, recibe el objeto PaginaCLS
        [HttpPost, ActionName("Editar")]
        public IActionResult EditarPost(PaginaCLS oPaginaCLS)
        {
            // Si id pagina es cero, es porque se trata de un nuevo registro.
            if (oPaginaCLS.idPagina == 0)
            {
                return NotFound();
            }

            // Se prohibe el campo mensaje Duplicado
            // Validar existencia anterior

            bool existeMensaje = false;

            // Instanciar contexto
            BDHospitalContext db = new BDHospitalContext();

            // Validar si existen duplicados
            if (db.Paginas.Where(p => p.Mensaje.ToUpper().Trim() == oPaginaCLS.mensaje.ToUpper().Trim() && 
            p.Iidpagina != oPaginaCLS.idPagina).Count() >= 1)
            {
                existeMensaje = true;
            }

            // Obtener objeto pagina al que se aplicará actualizacion
            Pagina oPagina = db.Paginas.Where(p => p.Iidpagina == oPaginaCLS.idPagina).First();

            if (oPagina == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (!existeMensaje)
                {
                    oPagina.Mensaje = oPaginaCLS.mensaje;
                    oPagina.Accion = oPaginaCLS.accion;
                    oPagina.Controlador = oPaginaCLS.controlador;
                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                } else
                {
                    return RedirectToAction(nameof(Editar), new { id = oPaginaCLS.idPagina, errorEditar = true });
                }
            } catch (DbUpdateException /* e */)
            {
                return RedirectToAction(nameof(Editar), new { id = oPaginaCLS.idPagina, errorEditar = true });
            }            
        }
    }
}
