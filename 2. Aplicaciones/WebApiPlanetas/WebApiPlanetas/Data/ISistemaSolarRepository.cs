using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPlanetas.Models;

namespace WebApiPlanetas.Data
{
    public interface ISistemaSolarRepository<T>
    {
        /// <summary>
        /// Metodo de insercion del periodo.
        /// </summary>
        /// <param name="periodo">Objeto periodo.</param>
        /// <returns>Mensaje exitoso.</returns>
        Task<string> Insert(Periodo periodo);

        /// <summary>
        /// Metodo que consulta un determinado periodo por id.
        /// </summary>
        /// <param name="id">Codigo del dia.</param>
        /// <returns>Objeto Periodo.</returns>
        Task<Periodo> GetByID(int id);

        /// <summary>
        /// Actualiza el objeto periodo.
        /// </summary>
        /// <param name="periodo">Objeto Periodo.</param>
        /// <returns>Mensaje.</returns>
        Task<string> Update(Periodo periodo);
    }
}
