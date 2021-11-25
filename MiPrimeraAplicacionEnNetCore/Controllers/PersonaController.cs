using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionEnNetCore.Models;
using MiPrimeraAplicacionEnNetCore.Clases;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class PersonaController : Controller
    {
        public IActionResult Index()
        {
            List<PersonaCLS> listaPersona = new List<PersonaCLS>();
            using(BDHospitalContext db = new BDHospitalContext())
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
            }
            return View(listaPersona);
        }
    }
}
