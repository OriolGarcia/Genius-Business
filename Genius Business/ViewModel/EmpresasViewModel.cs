using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Business.ViewModel
{
    class EmpresasViewModel
   {
        public long Empresa_idEmpresa { get; set; }
        public string Nombre_Empresa { get; set; }
        public string Emailprincipal { get; set; }
        public string NumTel { get; set; }
        public string ContraseñaEmpresa { get; set; }
        public string Pais { get; set; }
        public Nullable<int> cuentastrial { get; set; }
    }
}
