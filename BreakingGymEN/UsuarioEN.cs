using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakingGymEN
{
    public class UsuarioEN : Persona
    {
        public string Cuenta { get; set; } // Nombre de la cuenta
        public string Contrasena { get; set; } //  la contraseña
        public UsuarioEN() : base()
        {
        }
        public UsuarioEN(int Id, int IdRol, int IdTipoDocumento, string Documento, string Nombre, string Apellido, string Celular, string Cuenta, string Contraseña) 
            : base(Id, IdRol, IdTipoDocumento, Documento, Nombre, Apellido, Celular)
        {
            this.Cuenta = Cuenta;
            this.Contrasena = Contraseña;
        }
    }
    
}
