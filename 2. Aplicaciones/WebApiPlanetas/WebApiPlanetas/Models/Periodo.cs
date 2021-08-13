using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPlanetas.Models
{
    public class Periodo
    {
        /// <summary>
        /// Dia que se crea.
        /// </summary>
        public int Dia { get; set; }

        /// <summary>
        /// Tipo de clima que hay para ese dia.
        /// </summary>
        public string Clima { get; set; }
    }
}
