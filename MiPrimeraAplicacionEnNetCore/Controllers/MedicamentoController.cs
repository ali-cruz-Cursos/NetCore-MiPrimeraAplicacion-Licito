using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionEnNetCore.Clases;
using MiPrimeraAplicacionEnNetCore.Models;

namespace MiPrimeraAplicacionEnNetCore.Controllers
{
    public class MedicamentoController : Controller
    {
        public IActionResult Index()
        {
            List<MedicamentoCLS> listaMedicamento = new List<MedicamentoCLS>();
            using(BDHospitalContext db = new BDHospitalContext())
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
            }   
            return View(listaMedicamento);
        }
    }
}
