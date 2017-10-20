using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Entity
{
    public class ReservaEstado
    {
        public static readonly ReservaEstado RESERVADO = new ReservaEstado("RESERVADO");
        public static readonly ReservaEstado ANULADO = new ReservaEstado("ANULADO");

        private ReservaEstado(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}