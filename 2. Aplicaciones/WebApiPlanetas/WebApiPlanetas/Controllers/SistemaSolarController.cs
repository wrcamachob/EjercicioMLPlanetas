using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPlanetas.Data;
using WebApiPlanetas.Models;

namespace WebApiPlanetas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SistemaSolarController : ControllerBase
    {
        /// <summary>
        /// Interfaz Sistema solar.
        /// </summary>
        private readonly ISistemaSolarRepository<Periodo> _periodo;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="periodo"></param>
        public SistemaSolarController(ISistemaSolarRepository<Periodo> periodo)
        {
            this._periodo = periodo;
        }

        /// <summary>
        /// Metodo de insercion del periodo.
        /// </summary>
        /// <param name="periodo">Objeto periodo.</param>
        /// <returns>Mensaje exitoso.</returns>
        [HttpPost]
        public async Task<string> Post(Periodo periodo)
        {
            string retorno = string.Empty;            
            retorno = await _periodo.Insert(periodo);            
            return retorno;
        }

        /// <summary>
        /// Metodo que consulta un determinado periodo por id.
        /// </summary>
        /// <param name="id">Codigo del dia.</param>
        /// <returns>Objeto Periodo.</returns>
        [HttpGet("{id}")]
        public async Task<Periodo> Get(Int32 id)
        {
            Periodo periodo = await _periodo.GetByID(id);
            if (periodo != null)
            {
                Response.Headers.Add("MensajeError", "Registro no existe");
            }
            return periodo;
        }

        /// <summary>
        /// Actualiza el objeto periodo.
        /// </summary>
        /// <param name="periodo">Objeto Periodo.</param>
        /// <returns>Mensaje.</returns>
        [HttpPut]
        public async Task<string> Put(Periodo periodo)
        {
            return await _periodo.Update(periodo);
        }
    }
}
