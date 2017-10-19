using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App.Entity
{
    [Table("TipoHabitacion")]
    public class TipoHabitacion
    {
        [Key]
        public int IdTipoHabitacion { set; get; }

        public string Descripcion { get; set; }

    }
}