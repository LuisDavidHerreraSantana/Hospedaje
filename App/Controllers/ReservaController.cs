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


        public ActionResult Reserva()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reserva(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                
                Cliente Cliente = new Cliente();
                Cliente.Dni = reserva.Dni;
                Cliente.Nombre = reserva.Cliente.Nombre;
                Cliente.ApellidoMaterno = reserva.Cliente.ApellidoMaterno;
                Cliente.ApellidoPaterno = reserva.Cliente.ApellidoMaterno;

                db.Clientes.Add(Cliente);
                db.SaveChanges();

                Reserva Reserva = new Reserva();
                Reserva.Fecha = DateTime.Now;
                Reserva.IdHabitacion = 1;
                Reserva.Dni = reserva.Dni;

                //reserva.Fecha = DateTime.Now;
                //reserva.IdHabitacion = 1;
                db.Reservas.Add(Reserva);
                db.SaveChanges();
            }

            return View();
        }

        // GET: Reserva/Create
        public ActionResult Create()
        {
            ViewBag.Dni = new SelectList(db.Clientes, "Dni", "Nombre");
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "IdHabitacion", "IdHabitacion");
            return View();
        }

        // POST: Reserva/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdReserva,Dni,IdHabitacion,Fecha")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                db.Reservas.Add(reserva);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dni = new SelectList(db.Clientes, "Dni", "Nombre", reserva.Dni);
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "IdHabitacion", "IdHabitacion", reserva.IdHabitacion);
            return View(reserva);
        }

        // GET: Reserva/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dni = new SelectList(db.Clientes, "Dni", "Nombre", reserva.Dni);
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "IdHabitacion", "IdHabitacion", reserva.IdHabitacion);
            return View(reserva);
        }

        // POST: Reserva/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdReserva,Dni,IdHabitacion,Fecha")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reserva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dni = new SelectList(db.Clientes, "Dni", "Nombre", reserva.Dni);
            ViewBag.IdHabitacion = new SelectList(db.Habitaciones, "IdHabitacion", "IdHabitacion", reserva.IdHabitacion);
            return View(reserva);
        }

        // GET: Reserva/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // POST: Reserva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserva reserva = db.Reservas.Find(id);
            db.Reservas.Remove(reserva);
            db.SaveChanges();
            return RedirectToAction("Index");
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
