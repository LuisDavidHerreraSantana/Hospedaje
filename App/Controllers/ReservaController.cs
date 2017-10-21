using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using App.Entity;
using App.Models;
using Newtonsoft.Json;

namespace App.Controllers
{
    public class ReservaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reserva
        public ActionResult Index()
        {
            var reservas = db.Reservas.Include(r => r.Cliente).Include(r => r.Habitacion);
            return View(reservas.ToList());
        }

        [ChildActionOnly]
        public ActionResult Habitaciones()
        {
            ICollection<Habitacion> model = db.Habitaciones.ToList();

            return PartialView("_Modal_habitaciones", model);
        }

        [ChildActionOnly]
        public ActionResult Search()
        {
            Reserva Reserva = new Reserva();

            return PartialView("_Search", Reserva);
        }

        [Authorize(Roles = "Empleado")]
        public ActionResult Reserva()
        {
            ViewBag.JsonClientes = JsonConvert.SerializeObject(db.Clientes.ToList(), Formatting.Indented);

            return View();
        }

        [Authorize(Roles = "Empleado")]
        [HttpPost]
        public ActionResult Reserva(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                this.ReservarHabitacion(reserva);
            }

            ViewBag.JsonClientes = JsonConvert.SerializeObject(db.Clientes.ToList(), Formatting.Indented);

            return View();
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult Online()
        {
            return View();
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public ActionResult Online(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                this.ReservarHabitacion(reserva);
            }

            return View();
        }

        [Authorize(Roles = "Empleado")]
        public ActionResult Anular(string criterio = "")
        {

            var Reservas = db.Reservas
                .Where(x => 
                x.Dni.Contains(criterio) || 
                x.Cliente.Nombre.Contains(criterio) || 
                x.Cliente.ApellidoPaterno.Contains(criterio) ||
                x.Cliente.ApellidoMaterno.Contains(criterio)
                ).Where(x => x.Estado == ReservaEstado.RESERVADO.Value).ToList();

            return View(Reservas);
        }

        [Authorize(Roles = "Empleado")]
        public ActionResult Apply(string id)
        {
            if (id != null)
            {
                try
                {
                    int idReserva = int.Parse(id);
                    var Reserva = db.Reservas.Find(idReserva);

                    if (Reserva == null)
                    {
                        throw new Exception();
                    }

                    Reserva.Habitacion.Disponible = true;
                    Reserva.Estado = ReservaEstado.ANULADO.Value;
                    db.Entry(Reserva).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["Reserva.Anular"] = "La reserva N." + Reserva.IdReserva + " se ha anulado correctamente.";
                }
                catch (Exception e) {
                    Session["Reserva.Anular"] = "Ocurrio un error al anular la reserva, por favor intentelo denuevo.";
                }

            }

            return RedirectToAction("Anular");
        }

        protected void ReservarHabitacion(Reserva reserva)
        {
            var userDb = db.Clientes.Find(reserva.Dni);
            if (userDb == null)
            {
                Cliente Cliente = new Cliente();
                Cliente.Dni = reserva.Dni;
                Cliente.Nombre = reserva.Cliente.Nombre;
                Cliente.ApellidoMaterno = reserva.Cliente.ApellidoMaterno;
                Cliente.ApellidoPaterno = reserva.Cliente.ApellidoPaterno;

                db.Clientes.Add(Cliente);
                db.SaveChanges();
            }

            Reserva Reserva = new Reserva();
            Reserva.Fecha = DateTime.Now;
            Reserva.IdHabitacion = reserva.IdHabitacion;
            Reserva.Dni = reserva.Dni;
            Reserva.Estado = ReservaEstado.RESERVADO.Value;
            db.Reservas.Add(Reserva);
            db.SaveChanges();


            Habitacion Habitacion = db.Habitaciones.Find(reserva.IdHabitacion);
            Habitacion.Disponible = false;
            db.Entry(Habitacion).State = EntityState.Modified;
            db.SaveChanges();

            var res = db.Reservas.ToList().LastOrDefault();

            ViewBag.Message = "La reserva se registro correctamente, con el N." + res.IdReserva;
            ModelState.Clear();
        }

        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
