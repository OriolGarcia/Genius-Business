using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Genius_Business.ViewModel
{
    class MaterialesViewModel
    {
        public long material_idempresa { get; set; }
        public long material_id { get; set; }
        public System.DateTime material_fecha { get; set; }
        public Nullable<long> material_tiendaid { get; set; }
        public string material_tienda { get; set; }
        public Nullable<decimal> material_costeunidad { get; set; }
        public BitmapImage material_imagen { get; set; }
        public Byte[] material_imagen_byte { get; set; }
        public Nullable<decimal> material_stock { get; set; }
        public string material_nombre { get; set; }
        public string material_unidad { get; set; }
        public Nullable<decimal> material_costefabricación { get; set; }
    }
}
