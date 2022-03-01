using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionEnNetCore.Models;
using MiPrimeraAplicacionEnNetCore.Clases;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class PersonaController : Controller
    {

        public void llenarSexo()
        {
            //Llenar comboBox
            List<SelectListItem> listaSexo = new List<SelectListItem>();

            using(BDHospitalContext db = new BDHospitalContext())
            {
                listaSexo = (from sexo in db.Sexo
                             where sexo.Bhabilitado == 1
                             select new SelectListItem
                             {
                                 Text = sexo.Nombre,
                                 Value = sexo.Iidsexo.ToString()
                             }).ToList();
                listaSexo.Insert(0, new SelectListItem { Text = "Seleccione", Value = "" });
            }
            ViewBag.listaSexo = listaSexo;
        }

        public IActionResult Index(PersonaCLS oPersonaCLS)
        {
            llenarSexo();
            List<PersonaCLS> listaPersona = new List<PersonaCLS>();            
            //var listaPersona = "";

            using (BDHospitalContext db = new BDHospitalContext())
            {
                if (oPersonaCLS.iidsexo == 0 || oPersonaCLS.iidsexo == null)
                {
                    listaPersona = (from vPersona in db.Persona
                                    join sexo in db.Sexo
                                    on vPersona.Iidsexo equals sexo.Iidsexo
                                    where vPersona.Bhabilitado == 1
                                    select new PersonaCLS
                                    {
                                        iidPersona = vPersona.Iidpersona,
                                        nombreCompleto = vPersona.Nombre + " " + vPersona.Appaterno + " " + vPersona.Apmaterno,
                                        email = vPersona.Email,
                                        nombreSexo = sexo.Nombre
                                    }).ToList();
                } else
                {
                    listaPersona = (from vPersona in db.Persona
                                    join sexo in db.Sexo
                                    on vPersona.Iidsexo equals sexo.Iidsexo
                                    where vPersona.Bhabilitado == 1 && vPersona.Iidsexo == oPersonaCLS.iidsexo
                                    select new PersonaCLS
                                    {
                                        iidPersona = vPersona.Iidpersona,
                                        nombreCompleto = vPersona.Nombre + " " + vPersona.Appaterno + " " + vPersona.Apmaterno,
                                        email = vPersona.Email,
                                        nombreSexo = sexo.Nombre
                                    }).ToList();
                }
            }

            return View(listaPersona);
        }

        public IActionResult Agregar()
        {
            llenarSexo();
            return View();
        }

        [HttpPost]
        public IActionResult Guardar(PersonaCLS oPersonaCLS)
        {
            string nombreVista = "";
            int nveces = 0;

            if (oPersonaCLS.iidPersona == 0)
            {
                nombreVista = "Agregar";
            } else
            {
                nombreVista = "Guardar";
            }


            llenarSexo();
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {
                    oPersonaCLS.nombreCompleto = oPersonaCLS.nombre.ToUpper().Trim() + " " + 
                        oPersonaCLS.aPaterno.ToUpper().Trim() + " " + oPersonaCLS.aMaterno.ToUpper().Trim();

                    if (oPersonaCLS.iidPersona == 0)
                    {
                        nveces = db.Persona.Where(p => p.Nombre.ToUpper().Trim() + " " + p.Appaterno.ToUpper().Trim()
                        + " " + p.Apmaterno.ToUpper().Trim() == oPersonaCLS.nombreCompleto).Count();
                    } else
                    {
                        nveces = db.Persona.Where(p => p.Nombre.ToUpper().Trim() + " " + p.Appaterno.ToUpper().Trim()
                        + " " + p.Apmaterno.ToUpper().Trim() == oPersonaCLS.nombreCompleto && p.Iidpersona != oPersonaCLS.iidPersona).Count();
                    }


                    if (!ModelState.IsValid || nveces >= 1)
                    {
                        if (nveces >= 1)
                        {
                            oPersonaCLS.mensajeError = "La persona ya existe";
                        }
                        return View(nombreVista, oPersonaCLS);
                    }
                    else
                    {

                        Persona oPersona = new Persona();

                        oPersona.Nombre = oPersonaCLS.nombre;
                        oPersona.Appaterno = oPersonaCLS.aPaterno;
                        oPersona.Apmaterno = oPersonaCLS.aMaterno;
                        oPersona.Telefonofijo = oPersonaCLS.telefonoFijo;
                        oPersona.Telefonocelular = oPersonaCLS.telefonoCelular;
                        oPersona.Fechanacimiento = oPersonaCLS.fechaNacimiento;
                        oPersona.Iidsexo = oPersonaCLS.iidsexo;
                        oPersona.Email = oPersonaCLS.email;
                        oPersona.Bhabilitado = 1;
                        db.Persona.Add(oPersona);
                        db.SaveChanges();
                    }
                }
            } catch (Exception e)
            {
                return View(nombreVista, oPersonaCLS);
            }

            return RedirectToAction("Index");
        }


        // Editar persona
        public IActionResult Editar(int? id, bool? errorPersona = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            BDHospitalContext db = new BDHospitalContext();
            PersonaCLS oPersonaCLS = new PersonaCLS();

            oPersonaCLS = (from p in db.Persona
                           where p.Iidpersona == id
                           select new PersonaCLS
                           {
                               iidPersona = p.Iidpersona,
                               nombre = p.Nombre,
                               aPaterno = p.Appaterno,
                               aMaterno = p.Apmaterno,
                               telefonoFijo = p.Telefonofijo,
                               telefonoCelular = p.Telefonocelular,
                               fechaNacimiento = p.Fechanacimiento,
                               nombreSexo = p.IidsexoNavigation.Nombre,
                               email = p.Email
                           }).First();

            if (errorPersona.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Este email ya existe previamente registrado. No se permiten emails duplicados";
            }

            return View(oPersonaCLS);
        }

        // Eliminado virtual
        [HttpPost, ActionName("Editar")]
        public IActionResult EditarPost(PersonaCLS oPersonaCLS)
        {
            if (oPersonaCLS.iidPersona == 0)
            {
                return NotFound();
            }

            bool existEmail = false;

            using(BDHospitalContext db = new BDHospitalContext()) 
            {
                Persona oPersona = db.Persona.Where(p => p.Iidpersona == oPersonaCLS.iidPersona).First();

                if (db.Persona.Where(p => p.Email.ToUpper().Trim() == oPersona.Email.ToUpper().Trim() && p.Iidpersona != oPersonaCLS.iidPersona).Count() >= 1)
                {
                    existEmail = true;
                }

                try
                {
                    if (!existEmail)
                    {
                        oPersona.Nombre = oPersonaCLS.nombre;
                        oPersona.Appaterno = oPersonaCLS.aPaterno;
                        oPersona.Apmaterno = oPersonaCLS.aMaterno;
                        oPersona.Telefonofijo = oPersonaCLS.telefonoFijo;
                        oPersona.Telefonocelular = oPersonaCLS.telefonoCelular;
                        oPersona.Fechanacimiento = oPersonaCLS.fechaNacimiento;
                        oPersona.Email = oPersonaCLS.email;
                        oPersona.Iidsexo = oPersonaCLS.iidsexo;
                        oPersona.Bhabilitado = 1;
                        db.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    } else
                    {
                        return RedirectToAction(nameof(Editar), new { id = oPersonaCLS.iidPersona, errorPersona = true });
                    }
                } catch (DbUpdateException /* e */)
                {
                    return RedirectToAction(nameof(Editar), new { id = oPersonaCLS.iidPersona, errorPersona = true });
                }
            }            
        }
    }
}
