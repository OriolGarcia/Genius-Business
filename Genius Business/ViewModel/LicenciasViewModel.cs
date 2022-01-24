using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Business.ViewModel
{
    class LicenciasViewModel
    {

        public string id_App { get; set; }
        public string email { get; set; }
        public string program_key { get; set; }
        public string mac { get; set; }
        public Nullable<System.DateTime> limitdate { get; set; }
        public string version { get; set; }
        public long id { get; set; }
        public Nullable<bool> testing { get; set; }
    }
}
