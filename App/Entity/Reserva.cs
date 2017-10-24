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
        public string IdReserva { set; get; }

        [Required(ErrorMessage = "El dni es requerido")]
        public string Dni { set; get; }

        public virtual Cliente Cliente { get; set; }

        [Required(ErrorMessage = "Ingrese la fecha de reserva")]
        public DateTime Fecha { get; set; }

        public DateTime createdAt { get; set; }

        public string Estado { get; set; }

        public virtual ICollection<ReservaDetalle> Detalles { get; set; }

    }
}