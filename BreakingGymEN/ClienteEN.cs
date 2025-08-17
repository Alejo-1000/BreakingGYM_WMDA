using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakingGymEN
{ 
        public class ClienteEN : Persona
        {

        public ClienteEN() : base()
        {
        }
        public ClienteEN(int Id, int IdRol, int IdTipoDocumento, string Documento, string Nombre, string Apelllido, string Celular) : base(Id, IdRol, IdTipoDocumento, Documento, Nombre, Apelllido, Celular)
            {
            }
            
        }
}
