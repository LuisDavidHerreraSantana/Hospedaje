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

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required(ErrorMessage = "El apellido paterno es requerido")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required(ErrorMessage = "El apellido materno es requerido")]
        public string ApellidoMaterno { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }

        [ForeignKey("User")]
        public string IdUsuario { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}