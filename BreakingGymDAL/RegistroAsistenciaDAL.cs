using BreakingGymEN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakingGymDAL
{
    public class RegistroAsistenciaDAL
    {
        public static List<RegistroAsistenciaEN> MostrarAsistencia()
        {
            List<RegistroAsistenciaEN> _Lista = new List<RegistroAsistenciaEN>();
            using (IDbConnection _conn = ComunBD.ObtenerConexion(ComunBD.TipoBD.SqlServer))
            {
                _conn.Open();
                SqlCommand _comando = new SqlCommand("MostrarAsistencia", _conn as SqlConnection);
                _comando.CommandType = CommandType.StoredProcedure;
                IDataReader _reader = _comando.ExecuteReader();
                while (_reader.Read())
                {
                    _Lista.Add(new RegistroAsistenciaEN
                    {
                        Id = _reader.GetInt32(0),
                        IdCliente = _reader.GetInt32(1),
                        FechaAsistencia = _reader.GetDateTime(2)
                    });
                }
                _conn.Close();
            }
            return _Lista;
        }
        public static List<RegistroAsistenciaEN> BuscarAsistencia(String fechaAsistencia)
        {
            List<RegistroAsistenciaEN> _Lista = new List<RegistroAsistenciaEN>();
            using (IDbConnection _conn = ComunBD.ObtenerConexion(ComunBD.TipoBD.SqlServer))
            {
                _conn.Open();
                SqlCommand _comando = new SqlCommand("BuscarAsistencia", _conn as SqlConnection);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.Add(new SqlParameter("@fechaAsistencia", fechaAsistencia));
                IDataReader _reader = _comando.ExecuteReader();
                while (_reader.Read())
                {
                    _Lista.Add(new RegistroAsistenciaEN
                    {
                        Id = _reader.GetInt32(0),
                        IdCliente = _reader.GetInt32(1),
                        FechaAsistencia = _reader.GetDateTime(2)
                    });
                }
                _conn.Close();
            }
            return _Lista;
        }
        public static int AgregarAsistencia(RegistroAsistenciaEN pAsistenciasEN)
        {
            using (IDbConnection _conn = ComunBD.ObtenerConexion(ComunBD.TipoBD.SqlServer))
            {
                _conn.Open();
                SqlCommand _comando = new SqlCommand("RegistrarAsistencia ", _conn as SqlConnection);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.Add(new SqlParameter("@IdCliente", pAsistenciasEN.IdCliente));
                _comando.Parameters.Add(new SqlParameter("@FechaAsistencia", pAsistenciasEN.FechaAsistencia));
                int resultado = _comando.ExecuteNonQuery();
                _conn.Close();
                return resultado;
            }
        }

        public static int ModificarAsistencia(RegistroAsistenciaEN pAsistenciasEN)
        {
            using (IDbConnection _conn = ComunBD.ObtenerConexion(ComunBD.TipoBD.SqlServer))
            {
                _conn.Open();
                SqlCommand _comando = new SqlCommand("ModificarAsistencia", _conn as SqlConnection);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.Add(new SqlParameter("@Id", pAsistenciasEN.Id));
                _comando.Parameters.Add(new SqlParameter("@IdCliente", pAsistenciasEN.IdCliente));
                _comando.Parameters.Add(new SqlParameter("@FechaVencimiento", pAsistenciasEN.FechaAsistencia));

                int resultado = _comando.ExecuteNonQuery();
                _conn.Close();
                return resultado;
            }
        }
    }
}
