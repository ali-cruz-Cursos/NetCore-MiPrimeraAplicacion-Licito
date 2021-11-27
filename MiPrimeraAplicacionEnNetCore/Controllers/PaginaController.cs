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
        public IActionResult Index()
        {
            List<PaginaCLS> listaPaginas = new List<PaginaCLS>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                listaPaginas = (from pagina in db.Paginas
                                where pagina.Bhabilitado == 1
                                select new PaginaCLS
                                {
                                    idPagina = pagina.Iidpagina,
                                    mensaje = pagina.Mensaje,
                                    accion = pagina.Accion,
                                    controlador = pagina.Controlador
                                }).ToList();
            }

                return View(listaPaginas);
        }
    }
}
