using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App.Entity
{
    [Table("ReservaDetalle")]
    public class ReservaDetalle
    {
        [Key]
        public int IdReservaDetalle { set; get; }

        public int IdHabitacion { set; get; }

        public virtual Habitacion Habitacion { get; set; }

        public string IdReserva { set; get; }

        public virtual Reserva Reserva { get; set; }
    }
}