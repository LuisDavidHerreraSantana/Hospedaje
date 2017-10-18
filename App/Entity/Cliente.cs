using App.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App.Entity
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public string Dni { set; get; }

        public string Nombre { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }

        [ForeignKey("User")]
        public string IdUsuario { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}