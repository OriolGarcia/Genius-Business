//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Genius_Business.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class GbB_ventas
    {
        public long venta_idempresa { get; set; }
        public long venta_id { get; set; }
        public System.DateTime venta_fecha { get; set; }
        public long venta_tiendaid { get; set; }
        public decimal venta_importe { get; set; }
        public Nullable<long> venta_idcliente { get; set; }
    }
}