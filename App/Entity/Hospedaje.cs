using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace App.Entity
{
    [Table("Hospedaje")]
    public class Hospedaje
    {
        [Key]
        public int IdHospedaje { set; get; }

        public virtual ICollection<Habitacion> Habitaciones { get; set; }
    }
}