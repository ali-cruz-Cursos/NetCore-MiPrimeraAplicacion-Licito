using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionEnNetCore.Models;
using MiPrimeraAplicacionEnNetCore.Clases;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            using(BDHospitalContext db = new BDHospitalContext())
            {
                if (oPersonaCLS.iidsexo == 0 ||
                    oPersonaCLS.iidsexo == null)
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
        public IActionResult Agregar(PersonaCLS oPersonaCLS)
        {
            llenarSexo();
            try
            {
                using (BDHospitalContext db = new BDHospitalContext())
                {

                    if (!ModelState.IsValid)
                    {
                        return View(oPersonaCLS);
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
                return View(oPersonaCLS);
            }

            return RedirectToAction("Index");
        }
    }
}
