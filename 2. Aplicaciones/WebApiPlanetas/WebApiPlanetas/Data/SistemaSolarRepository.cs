using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApiPlanetas.Models;

namespace WebApiPlanetas.Data
{
    public class SistemaSolarRepository : ISistemaSolarRepository<Periodo>
    {
        /// <summary>
        /// Cadena de conexion.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SistemaSolarRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");            
        }

        /// <summary>
        /// Metodo de insercion del periodo.
        /// </summary>
        /// <param name="periodo">Objeto periodo.</param>
        /// <returns>Mensaje exitoso.</returns>
        public async Task<string> Insert(Periodo periodo)
        {
            string mensaje = string.Empty;            

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "spInsPeriodos",
                        CommandTimeout = 10
                    };
                    cmd.Parameters.AddWithValue("@Dia", periodo.Dia);
                    cmd.Parameters["@Dia"].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@Clima", periodo.Clima.ToString());
                    cmd.Parameters["@Clima"].Direction = System.Data.ParameterDirection.Input;                    
                    await conn.OpenAsync();
                    cmd.ExecuteNonQuery();
                    mensaje = "Registro Insertado";
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al insertar", ex);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
            return mensaje;
        }

        /// <summary>
        /// Metodo que consulta un determinado periodo por id.
        /// </summary>
        /// <param name="id">Codigo del dia.</param>
        /// <returns>Objeto Periodo.</returns>
        public async Task<Periodo> GetByID(int id)
        {
            Periodo periodo = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "spSelPeriodosPorId",
                        CommandTimeout = 10
                    };
                    cmd.Parameters.Add("@Dia", SqlDbType.Int).Value = id;
                    await conn.OpenAsync();
                    cmd.ExecuteScalar();
                    SqlDataReader dt = cmd.ExecuteReader();

                    while (dt.Read())
                    {
                        periodo = new Periodo()
                        {
                            Dia = Convert.ToInt32(dt["Dia"].ToString()),
                            Clima = dt["Clima"].ToString()                            
                        };
                    }

                    if (!dt.HasRows)
                    {
                        periodo = new Periodo();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al consultar", ex);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
            return periodo;
        }

        /// <summary>
        /// Actualiza el objeto periodo.
        /// </summary>
        /// <param name="periodo">Objeto Periodo.</param>
        /// <returns>Mensaje.</returns>
        public async Task<string> Update(Periodo periodo)
        {
            string mensaje = string.Empty;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "spUpdPeriodos",
                        CommandTimeout = 10
                    };
                    cmd.Parameters.AddWithValue("@Dia", periodo.Dia);
                    cmd.Parameters["@Dia"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@Clima", periodo.Clima);
                    cmd.Parameters["@Clima"].Direction = ParameterDirection.Input;
                    await conn.OpenAsync();
                    int value = cmd.ExecuteNonQuery();
                    if (value > 0)
                        mensaje = "Registro Actualizado";
                    else
                        mensaje = "No se encontró Registro";
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar", ex);
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
            return mensaje;
        }
    }
}
