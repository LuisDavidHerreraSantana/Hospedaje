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

        public ActionResult Reserva()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reserva(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                this.ReservarHabitacion(reserva);
            }

            return View();
        }

        public ActionResult Online()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Online(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                this.ReservarHabitacion(reserva);
            }

            return View();
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
                Cliente.ApellidoPaterno = reserva.Cliente.ApellidoMaterno;

                db.Clientes.Add(Cliente);
                db.SaveChanges();
            }

            Reserva Reserva = new Reserva();
            Reserva.Fecha = DateTime.Now;
            Reserva.IdHabitacion = reserva.IdHabitacion;
            Reserva.Dni = reserva.Dni;
            db.Reservas.Add(Reserva);
            db.SaveChanges();


            Habitacion Habitacion = db.Habitaciones.Find(reserva.IdHabitacion);
            Habitacion.Disponible = false;
            db.Entry(Habitacion).State = EntityState.Modified;
            db.SaveChanges();

            var res = db.Reservas.ToList().LastOrDefault();

            ViewBag.Message = "La reserva se guardó correctamente, con el N°" + res.IdReserva;
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
