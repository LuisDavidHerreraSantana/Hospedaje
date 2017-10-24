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
using Microsoft.AspNet.Identity;
using System.Collections.ObjectModel;

namespace App.Controllers
{
    public class ReservaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
        public ActionResult Reserva(Reserva reserva, List<int> Habitaciones)
        {
            var noHabs = Habitaciones != null ? Habitaciones.Count() < 1 : true;

            if (noHabs)
            {
                ViewBag.ErrorHab = "Seleccione al menos una habitación";
            }


            if (ModelState.IsValid && !noHabs)
            {
                this.ReservarHabitacion(reserva, Habitaciones);
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
        public ActionResult Online(Reserva reserva, List<int> Habitaciones)
        {
            var noHabs = Habitaciones != null ? Habitaciones.Count() < 1 : true;

            if (noHabs)
            {
                ViewBag.ErrorHab = "Seleccione al menos una habitación";
            }


            if (ModelState.IsValid && !noHabs)
            {
                this.ReservarHabitacion(reserva, Habitaciones);
            }

            return View();
        }

        [Authorize(Roles = "Empleado")]
        public ActionResult Anular()
        {

            var Reservas = db.Reservas
                .Where(x => x.Estado == ReservaEstado.RESERVADO.Value).ToList();


            List<Object> Res = new List<Object>();

            foreach (var Reserva in Reservas)
            {
                if (Reserva.Estado != ReservaEstado.ANULADO.Value)
                {

                    var habitaciones = Reserva.Detalles;

                    List<Object> habs = new List<Object>();

                    foreach (var h in habitaciones)
                    {
                        if (h.Habitacion.Disponible == false )
                        {
                            habs.Add(new  { IdHabitacion = h.Habitacion.IdHabitacion, Numero = h.Habitacion.Numero } );
                        }
                    }

                    var obj = new
                    {
                        IdReserva = Reserva.IdReserva,
                        Cliente = Reserva.Cliente,
                        Fecha = Reserva.Fecha.ToString("yyyy-MM-dd"),
                        Hora = Reserva.Fecha.ToString("HH:ss"),
                        Habitaciones = habs
                    };

                    Res.Add(obj);
                }
                
            }

            ViewBag.JsReservas = JsonConvert.SerializeObject(Res, Formatting.Indented);

            return View(Reservas);
        }

        [Authorize(Roles = "Empleado")]
        public ActionResult Apply(string IdReserva, List<int> IdHabitacion)
        {
            
            if (IdReserva != null && IdHabitacion.Count > 0)
            {
                try
                {
                    //int idReserva = int.Parse(id);
                    var Reserva = db.Reservas.Find(IdReserva);

                    if (Reserva == null)
                    {
                        throw new Exception();
                    }

                    //Reserva.Habitacion.Disponible = true;

                    //var Detalles = new Collection<ReservaDetalle>();

                    var last = 0;
                    var now = 0;

                    foreach (var Det in Reserva.Detalles)
                    {
                        var Hab = Det.Habitacion;
                        if (Hab.Disponible)
                        {

                            last++;
                            continue;
                        }
                            
                        foreach (var IdHab in IdHabitacion)
                        {
                            if (Hab.IdHabitacion == IdHab)
                            {
                                Hab.Disponible = true;
                                db.Entry(Hab).State = EntityState.Modified;
                                db.SaveChanges();
                                now++;
                            }

                        }
                    }

                    if ( (last+now) == Reserva.Detalles.Count)
                    {
                        Reserva.Estado = ReservaEstado.ANULADO.Value;
                        db.Entry(Reserva).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["Reserva.Anular"] = "La reserva con el Cod. " + Reserva.IdReserva + " se ha anulado correctamente.";
                    }
                    else
                    {
                        Session["Reserva.Anular"] = "La reserva con el Cod. " + Reserva.IdReserva + " todavia tiene Habitaciones reservadas";
                    }
                    
                }
                catch (Exception e) {
                    Session["Reserva.Anular"] = "Ocurrio un error al anular la reserva, por favor intentelo denuevo.";
                }

            }

            return RedirectToAction("Anular");
        }

        protected void ReservarHabitacion(Reserva reserva, List<int> Habitaciones)
        {
            var userDb = db.Clientes.Find(reserva.Dni);
            if (userDb == null)
            {
                Cliente Cliente = new Cliente();
                Cliente.Dni = reserva.Dni;
                Cliente.Nombre = reserva.Cliente.Nombre;
                Cliente.ApellidoMaterno = reserva.Cliente.ApellidoMaterno;
                Cliente.ApellidoPaterno = reserva.Cliente.ApellidoPaterno;
                Cliente.IdUsuario = User.Identity.GetUserId();
                Cliente.CreatedAt = DateTime.Now;

                db.Clientes.Add(Cliente);
                db.SaveChanges();
            }

            Reserva Reserva = new Reserva();
            //Reserva.Fecha = DateTime.Now;
            Reserva.Fecha = reserva.Fecha;
            Reserva.Dni = reserva.Dni;
            Reserva.createdAt = DateTime.Now;
            Reserva.Estado = ReservaEstado.RESERVADO.Value;

            Reserva.Detalles = reserva.Detalles;

            ICollection<ReservaDetalle> ReservaDetalles = new Collection<ReservaDetalle>();

            foreach (var Detalle in Habitaciones)
            {
                var ReservaDetalle = new ReservaDetalle();
                ReservaDetalle.IdHabitacion = Detalle;

                ReservaDetalles.Add(ReservaDetalle);

                Habitacion Habitacion = db.Habitaciones.Find(Detalle);
                Habitacion.Disponible = false;
                db.Entry(Habitacion).State = EntityState.Modified;
                db.SaveChanges();

            }

            Reserva.Detalles = ReservaDetalles;

            Reserva.IdReserva = Util.Helper.Id(db);

            db.Reservas.Add(Reserva);
            db.SaveChanges();

            var res = db.Reservas.ToList().LastOrDefault();

            ViewBag.Message = "La reserva se registro correctamente, con el Cod. " + res.IdReserva;
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
