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
                                        email = vPersona == null ? "NA" : vPersona.Email,
                                        nombreSexo = sexo == null ? "NA" : sexo.Nombre
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

        public IActionResult Agregar(bool? saveChangesError = false)
        {
            llenarSexo();
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Error al intentar registrar a nueva persona. Duplicado";
            }

            return View();
        }

        [HttpPost]
        public IActionResult Guardar(PersonaCLS oPersonaCLS)
        {
            try
            {
                llenarSexo();

                using (BDHospitalContext db = new BDHospitalContext())
                {
                    if (!string.IsNullOrWhiteSpace(oPersonaCLS.nombre) ||
                        !string.IsNullOrWhiteSpace(oPersonaCLS.aPaterno) ||
                        !string.IsNullOrWhiteSpace(oPersonaCLS.aMaterno) ||
                        !string.IsNullOrWhiteSpace(oPersonaCLS.email))
                    {
                        oPersonaCLS.nombreCompleto = oPersonaCLS.nombre.ToUpper().Trim() + " " +
                            oPersonaCLS.aPaterno.ToUpper().Trim() + " " + oPersonaCLS.aMaterno.ToUpper().Trim();
                    } else
                    {
                        // No puedo validar campos vacios
                        return RedirectToAction(nameof(Agregar));
                    }

                    if (oPersonaCLS.iidPersona != 0)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid && !ExistUserEmail(oPersonaCLS.email, "alta", 0) && 
                        !ExistUserName(oPersonaCLS.nombreCompleto, "alta", 0))
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
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Agregar), new { saveChangesError = true });
                    }
                }                
            } catch (Exception e)
            {
                return RedirectToAction(nameof(Agregar), new { saveChangesError = true });
            }
        }


        // Editar persona
        public IActionResult Editar(int? id, bool? errorPersona = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonaCLS oPersonaCLS = new PersonaCLS();

            using (BDHospitalContext db = new BDHospitalContext())
            {
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
            }

            if (errorPersona.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Este usuario y/o email ya existe previamente registrado. No se permiten duplicados";
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

            using(BDHospitalContext db = new BDHospitalContext()) 
            {
                Persona oPersona = db.Persona.Where(p => p.Iidpersona == oPersonaCLS.iidPersona).First();

                if (oPersona != null)
                {
                    if (!string.IsNullOrWhiteSpace(oPersonaCLS.nombre) ||
                            !string.IsNullOrWhiteSpace(oPersonaCLS.aPaterno) ||
                            !string.IsNullOrWhiteSpace(oPersonaCLS.aMaterno))
                    {
                        var nombreCompleto = oPersonaCLS.nombre.ToUpper().Trim() + " " + 
                            oPersonaCLS.aPaterno.ToUpper().Trim() + " " + 
                            oPersonaCLS.aMaterno.ToUpper().Trim();

                        try
                        {

                            if (!ExistUserName(nombreCompleto, "editar", oPersonaCLS.iidPersona) && 
                                !ExistUserEmail(oPersonaCLS.email, "editar", oPersonaCLS.iidPersona))
                            {
                                oPersona.Nombre = oPersonaCLS.nombre;
                                oPersona.Appaterno = oPersonaCLS.aPaterno;
                                oPersona.Apmaterno = oPersonaCLS.aMaterno;
                                oPersona.Telefonofijo = oPersonaCLS.telefonoFijo;
                                oPersona.Telefonocelular = oPersonaCLS.telefonoCelular;
                                oPersona.Fechanacimiento = oPersonaCLS.fechaNacimiento;
                                oPersona.Email = oPersonaCLS.email;
                                oPersona.Iidsexo = oPersonaCLS.nombreSexo == "MASCULINO" ? 1 : 2;
                                oPersona.Bhabilitado = 1;
                                db.SaveChanges();
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                return RedirectToAction(nameof(Editar), new { id = oPersonaCLS.iidPersona, 
                                    errorPersona = true });
                            }
                        }
                        catch (DbUpdateException /* e */)
                        {
                            return RedirectToAction(nameof(Editar), new { id = oPersonaCLS.iidPersona, 
                                errorPersona = true });
                        }
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // Recibo el string del nombre de usuario completo para validar si ya existe registrado en la tabla Persona
        public bool ExistUserName(string userName, string? tipoMovimiento, int? id)
        {
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if (tipoMovimiento == "alta")
                {
                    if (db.Persona.Where(p => p.Nombre.ToUpper().Trim() + " " +
                        p.Appaterno.ToUpper().Trim() + " " +
                        p.Apmaterno.ToUpper().Trim() == userName.ToUpper().Trim()).Count() > 0)
                    {
                        return true;
                    }
                }
                if (tipoMovimiento == "editar")
                {
                    if (db.Persona.Where(p => p.Nombre.ToUpper().Trim() + " " +
                        p.Appaterno.ToUpper().Trim() + " " +
                        p.Apmaterno.ToUpper().Trim() == userName.ToUpper().Trim() && p.Iidpersona != id).Count() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        // Recibo el string del email de usuario para validar si ya existe registrado en la tabla Persona
        public bool ExistUserEmail(string userEmail, string? tipoMovimiento, int? id)
        {
            using (BDHospitalContext db = new BDHospitalContext())
            {
                if (tipoMovimiento == "alta")
                {
                    if (db.Persona.Where(p => p.Email.ToUpper().Trim() == userEmail.ToUpper().Trim()).Count() > 0)
                    {
                        return true;
                    }
                } else if (tipoMovimiento == "editar")
                {
                    if (db.Persona.Where(p => p.Email.ToUpper().Trim() == userEmail.ToUpper().Trim() && 
                        p.Iidpersona != id).Count() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
