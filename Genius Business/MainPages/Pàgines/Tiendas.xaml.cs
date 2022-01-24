using Genius_Business.Model;
using Genius_Business.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Genius_Business.MainPages.Pàgines
{
    /// <summary>
    /// Interaction logic for Tiendas.xaml
    /// </summary>
    public partial class Tiendas : Page
    {
        public Tiendas()
        {
            InitializeComponent();
        }


        private void refreshTiendas()
        {


            List<ViewModel.TiendasViewModel> Tiendas = new List<ViewModel.TiendasViewModel>();
            try
            {
                using (etddatabaseEntities1 db = new etddatabaseEntities1())
                {

                    db.Configuration.UseDatabaseNullSemantics = true;
                    //db.CommandTimeout = 120;
                    Tiendas = (from d in db.GbB_Tiendas
                               from c in db.Paises.Where(c => c.paises_id == d.tienda_Pais).DefaultIfEmpty()
                               where (d.tienda_idempresa == App.idEmpresa)
                               select new TiendasViewModel()

                               {
                                   tienda_id = d.tienda_id,
                                   tienda_Direccion = d.tienda_Direccion,
                                   tienda_NombreTienda = d.tienda_NombreTienda,
                                   tienda_Pais = c.paises_pais,
                                   tienda_idempresa = d.tienda_idempresa,
                                   tienda_Población = d.tienda_Población,
                                   tienda_idPais = c.paises_id,
                                   tienda_moneda = c.paises_moneda,


                               }).ToList();


                    LTiendas.ItemsSource = Tiendas;

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);


            }

        }
        private void EliminarTienda_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FichaTienda_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
