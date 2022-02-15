using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionEnNetCore.Clases;
using MiPrimeraAplicacionEnNetCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class MedicamentoController : Controller
    {

        public List<SelectListItem> listaFormaFarmaceutica()
        {
            List<SelectListItem> listaFormaFarmaceutica = new List<SelectListItem>();

            using(BDHospitalContext db = new BDHospitalContext())
            {
                listaFormaFarmaceutica = (from listaFormasMarmaceuticas in db.FormaFarmaceuticas
                                          where listaFormasMarmaceuticas.Bhabilitado == 1
                                          select new SelectListItem
                                          {
                                              Text = listaFormasMarmaceuticas.Nombre,
                                              Value = listaFormasMarmaceuticas.Iidformafarmaceutica.ToString()
                                          }).ToList();
            }

            listaFormaFarmaceutica.Insert(0, new SelectListItem { Text = "Seleccionar", Value = "" });

            return listaFormaFarmaceutica;            
        }

        // GET: MedicamentoController/Agregar
        public IActionResult Agregar()
        {
            ViewBag.listaFormas = listaFormaFarmaceutica();
            return View();
        }


        // GET: MedicamentoController/Editar/5
        public IActionResult Editar(int id)
        {
            MedicamentoCLS oMedicamentoCLS = new MedicamentoCLS();

            using(BDHospitalContext db = new BDHospitalContext())
            {
                oMedicamentoCLS = (from medicamento in db.Medicamentos
                                   where medicamento.Iidmedicamento == id
                                   select new MedicamentoCLS
                                   {
                                       idMedicamento = medicamento.Iidmedicamento,
                                       nombre = medicamento.Nombre,
                                       concentracion = medicamento.Concentracion,
                                       idFormaFarmaceutica = medicamento.Iidformafarmaceutica,
                                       precio = (double)medicamento.Precio,
                                       stock = medicamento.Stock,
                                       presentacion = medicamento.Presentacion
                                   }).First();

            }
            ViewBag.listaFormas = listaFormaFarmaceutica();
            return View(oMedicamentoCLS);
        }

        // POST MedicamentosController/Agregar
        [HttpPost]
        public IActionResult Guardar(MedicamentoCLS oMedicamentoCLS) 
        {
            string nombreVista = "";

            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    if (oMedicamentoCLS.idMedicamento == 0)
                    {
                        nombreVista = "Agregar";
                    } else
                    {
                        nombreVista = "Editar";
                    }

                    if (!ModelState.IsValid)
                    {
                        ViewBag.listaFormas = listaFormaFarmaceutica();
                        return View(nombreVista, oMedicamentoCLS);
                    }
                    else
                    {
                        if (oMedicamentoCLS.idMedicamento == 0)
                        {
                            Medicamento oMedicamento = new Medicamento();

                            oMedicamento.Nombre = oMedicamentoCLS.nombre;
                            oMedicamento.Concentracion = oMedicamentoCLS.concentracion;
                            oMedicamento.Iidformafarmaceutica = oMedicamentoCLS.idFormaFarmaceutica;
                            oMedicamento.Precio = (int)oMedicamentoCLS.precio;
                            oMedicamento.Stock = oMedicamentoCLS.stock;
                            oMedicamento.Presentacion = oMedicamentoCLS.presentacion;
                            oMedicamento.Bhabilitado = 1;
                            db.Medicamentos.Add(oMedicamento);
                            db.SaveChanges();
                        } else
                        {
                            Medicamento oMedicamento = db.Medicamentos
                                                            .Where(p => p.Iidmedicamento == oMedicamentoCLS.idMedicamento).First();
                            oMedicamento.Nombre = oMedicamentoCLS.nombre;
                            oMedicamento.Concentracion = oMedicamentoCLS.concentracion;
                            oMedicamento.Iidformafarmaceutica = oMedicamentoCLS.idFormaFarmaceutica;
                            oMedicamento.Precio = (decimal)oMedicamentoCLS.precio;
                            oMedicamento.Stock = oMedicamentoCLS.stock;
                            oMedicamento.Presentacion = oMedicamentoCLS.presentacion;
                            db.SaveChanges();
                        }
                    }
                }
            } catch (Exception ex)
            {
                return View(nombreVista, oMedicamentoCLS);

            }

            return RedirectToAction("Index");
        }


        public IActionResult Index(MedicamentoCLS omedicamentoCLS)
        {
            
            ViewBag.listaFormasFarmaceuticas = listaFormaFarmaceutica();
            List<MedicamentoCLS> listaMedicamento = new List<MedicamentoCLS>();
            using(BDHospitalContext db = new BDHospitalContext())
            {
                if (omedicamentoCLS.idFormaFarmaceutica == null || 
                    omedicamentoCLS.idFormaFarmaceutica == 0)
                {
                    listaMedicamento = (from medicamento in db.Medicamentos
                                        join formaFarmaceutica in db.FormaFarmaceuticas
                                        on medicamento.Iidformafarmaceutica equals formaFarmaceutica.Iidformafarmaceutica
                                        where medicamento.Bhabilitado == 1
                                        select new MedicamentoCLS
                                        {
                                            idMedicamento = medicamento.Iidmedicamento,
                                            nombre = medicamento.Nombre,
                                            precio = (double)medicamento.Precio,
                                            stock = (int)medicamento.Stock,
                                            formaFarmaceutica = formaFarmaceutica.Nombre
                                        }).ToList();
                } else
                {
                    listaMedicamento = (from medicamento in db.Medicamentos
                                        join formaFarmaceutica in db.FormaFarmaceuticas
                                        on medicamento.Iidformafarmaceutica equals formaFarmaceutica.Iidformafarmaceutica
                                        where medicamento.Bhabilitado == 1 && 
                                        medicamento.Iidformafarmaceutica == omedicamentoCLS.idFormaFarmaceutica
                                        select new MedicamentoCLS
                                        {
                                            idMedicamento = medicamento.Iidmedicamento,
                                            nombre = medicamento.Nombre,
                                            precio = (double)medicamento.Precio,
                                            stock = (int)medicamento.Stock,
                                            formaFarmaceutica = formaFarmaceutica.Nombre
                                        }).ToList();
                }
            }   
            return View(listaMedicamento);
        }

        public IActionResult Eliminar(int idMedicamento)
        {
            using(BDHospitalContext db = new BDHospitalContext())
            {
                Medicamento oMedicamento = db.Medicamentos.Where(m => m.Iidmedicamento == idMedicamento).First();
                db.Medicamentos.Remove(oMedicamento);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
