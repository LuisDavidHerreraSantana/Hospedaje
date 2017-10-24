using App.Entity;
using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Util
{
    public class Helper
    {

        public static string Id(ApplicationDbContext db)
        {
            int count = 0;

            count = db.Reservas.ToList().Count;

            count += 1;

            string i = "" + count;

            string id = "";
            for (var a = 0; a < 5 - i.Length; a++)
            {
                id += "0";
            }

            return "AN-" +id + i;
        }

    }
}