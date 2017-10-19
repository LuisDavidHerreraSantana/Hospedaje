using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App.Entity
{
    [Table("Habitacion")]
    public class Habitacion
    {
        [Key]
        public int IdHabitacion { set; get; }

        [Required(ErrorMessage = "Seleccione una habitacion")]
        public string Numero { get; set; }

        public bool Disponible { get; set; }

        public int IdHospedaje { set; get; }

        public virtual Hospedaje Hospedaje { get; set; }

        public int IdTipoHabitacion { set; get; }

        public virtual TipoHabitacion TipoHabitacion { get; set; }
    }
}