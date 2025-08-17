using BreakingGymDAL;
using BreakingGymEN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakinGymBL
{
    public class RegistroAsistenciaBL
    {
        public List<RegistroAsistenciaEN> MostrarAsistencia()
        {
            return RegistroAsistenciaDAL.MostrarAsistencia();
        }
        public static List<RegistroAsistenciaEN> BuscarAsistencia(string fechaAsistencia)
        {
            return RegistroAsistenciaDAL.BuscarAsistencia(fechaAsistencia);
        }
        public int GuardarInscripcion(RegistroAsistenciaEN pAsistenciasEN)
        {
            return RegistroAsistenciaDAL.AgregarAsistencia(pAsistenciasEN);
        }
        public int ModificarAsistencia(RegistroAsistenciaEN pinscripcionEN)
        {
            return RegistroAsistenciaDAL.ModificarAsistencia(pinscripcionEN);
        }
    }
}
