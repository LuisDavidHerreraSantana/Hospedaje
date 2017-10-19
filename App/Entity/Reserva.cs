using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App.Entity
{
    [Table("Reserva")]
    public class Reserva
    {
        [Key]
        public int IdReserva { set; get; }

        [Required(ErrorMessage = "El dni es requerido")]
        public string Dni { set; get; }

        public virtual Cliente Cliente { get; set; }

        public int IdHabitacion { set; get; }

        public virtual Habitacion Habitacion { get; set; }

        public DateTime Fecha { get; set; }

    }
}