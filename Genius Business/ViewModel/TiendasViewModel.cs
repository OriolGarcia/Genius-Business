using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Business.ViewModel
{
    class TiendasViewModel
    {
        public long tienda_idempresa { get; set; }
        public long tienda_id { get; set; }
        public string tienda_NombreTienda { get; set; }
        public string tienda_Direccion { get; set; }
        public string tienda_Población { get; set; }
        public string tienda_Pais { get; set; }
        public long? tienda_idPais { get; set; }

        public string tienda_moneda { get; set; }

    }
}
