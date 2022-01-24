using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Genius_Business.ViewModel
{
    class ProductosViewModel
    {
         public long producto_idempresa { get; set; }
        public long producto_id { get; set; }
        public System.DateTime producto_fecha { get; set; }
        public string producto_tienda { get; set; }
        public long? producto_tiendaid { get; set; }
        public Nullable<decimal> producto_costeunidad { get; set; }
        public decimal producto_preciounidad { get; set; }
        public decimal producto_ivaporcent { get; set; }
        public Byte[] producto_imagen_byte { get; set; }
        public BitmapImage producto_imagen { get; set; }
        public Nullable<decimal> producto_stock { get; set; }
        public string producto_nombre { get; set; }
        public string producto_unidad { get; set; }
        public Nullable<decimal> producto_costefabricación { get; set; }
        public string producto_codigoproducto { get; set; }
    }
}
