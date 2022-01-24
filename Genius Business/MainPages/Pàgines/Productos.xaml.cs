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
using Genius_Business.Model;
using Genius_Business.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Drawing;
using Image = System.Drawing.Image;
using Microsoft.Win32;
using static Genius_Business.App;
using Genius_Business.Assets;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Reflection;
using Genius_Business.Resources;
//using Microsoft.Win32;

namespace Genius_Business.MainPages.Pàgines
{
    /// <summary>
    /// Interaction logic for Productos.xaml
    /// </summary>
    public partial class Productos : Page
    {
        public Productos()
        {
            InitializeComponent();
        }
        public void refreshProductos()
        {
            List<ViewModel.ProductosViewModel> Productos = new List<ViewModel.ProductosViewModel>();
            try
            {
                using (etddatabaseEntities1 db = new etddatabaseEntities1())
                {
                    //db.CommandTimeout = 120;
                    Productos = (from d in db.GbB_productos
                                 join c in db.GbB_Tiendas
                        on d.producto_tiendaid equals c.tienda_id into ps
                                 from c in ps.DefaultIfEmpty()
                                 where d.producto_idempresa == App.idEmpresa
                                 select new ProductosViewModel()

                                 {
                                     producto_id = d.producto_id,
                                     producto_costeunidad = d.producto_costeunidad,
                                     producto_fecha = d.producto_fecha,
                                     producto_idempresa = d.producto_idempresa,
                                     producto_codigoproducto = d.producto_codigoproducto,
                                     producto_ivaporcent = d.producto_ivaporcent,
                                     producto_nombre = d.producto_nombre,
                                     producto_preciounidad = d.producto_preciounidad,
                                     producto_stock = d.producto_stock,
                                     producto_tiendaid = d.producto_tiendaid,
                                     producto_tienda = c.tienda_NombreTienda,
                                     producto_unidad = d.producto_unidad,
                                     producto_imagen_byte = d.producto_imagen,
                                     producto_costefabricación = d.producto_costefabricación
                                 }).ToList();

                    foreach (ViewModel.ProductosViewModel pr in Productos)
                    {
                        if (pr.producto_imagen_byte != null)
                        {
                            MemoryStream ms = new MemoryStream(pr.producto_imagen_byte, 0, pr.producto_imagen_byte.Length);
                            ms.Write(pr.producto_imagen_byte, 0, pr.producto_imagen_byte.Length);

                            if (Encoding.UTF8.GetString(pr.producto_imagen_byte) != "C:")

                                pr.producto_imagen = ToImage(pr.producto_imagen_byte);
                        }


                    }
                    LProductos.ItemsSource = Productos;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);


            }


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


                   // LTiendas.ItemsSource = Tiendas;

                    Pr_tienda.DisplayMemberPath = "tienda_NombreTienda";
                    Pr_tienda.SelectedValuePath = "tienda_id";
                    Pr_tienda.ItemsSource = Tiendas;
                    Pr_tienda.SelectedItem = -1;

                 
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);


            }

        }
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
 

        private void ExamnarProducto_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = Directory.GetParent(path).ToString();
            }
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = path;
            openfile.Filter = "Images|*.jpg;*.jpeg;*.png;";
            openfile.FilterIndex = 1;
            openfile.RestoreDirectory = true;
            bool? result = openfile.ShowDialog();
            if (result == true)
            {
                Pr_Imagen.Text = openfile.FileName;
            }
        }

        private void AñadirProducto_Click(object sender, RoutedEventArgs e)
        {


            List<Control> lstControb = new List<Control>();
            lstControb.Add(Pr_costeproducto);
            lstControb.Add(Pr_iva);
            lstControb.Add(Pr_nombreproducto);
            lstControb.Add(Pr_precioproducto);
            lstControb.Add(Pr_Imagen);
            lstControb.Add(Pr_stock);
            lstControb.Add(Pr_costeproducto);
            lstControb.Add(Pr_codigoproducto);

            App.quitarErrores(lstControb);


            if (!String.IsNullOrEmpty(Pr_Imagen.Text) && !File.Exists(Pr_Imagen.Text))
            {

                MessageBox.Show("El fichero seleccionado ya no existe!");
                return;
            }
            CheckCampos ck;

            ck = App.checkTextBox(Pr_codigoproducto, "Código Producto", "",false);
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }

            ck = App.checkTextBox(Pr_nombreproducto, "Nombre del producto", "");
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }
            ck = App.checkTextBox(Pr_stock, "Stock", "decimal", false);
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }
            ck = App.checkTextBox(Pr_costeproducto, " Coste del producto", "decimal", Pr_costeproducto.IsEnabled);
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }
            ck = App.checkTextBox(Pr_precioproducto, "Precio del producto", "decimal");
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }
            ck = App.checkTextBox(Pr_iva, "IVA", "decimal");
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }
            ck = App.checkTextBox(Pr_costefabricacion, "Coste fabricación", "decimal", false);
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }



            var oProducto = new GbB_productos();
            using (etddatabaseEntities1 db = new etddatabaseEntities1())
            {


                if (!String.IsNullOrEmpty(Pr_Imagen.Text))
                {
                    Image img = Image.FromFile(Pr_Imagen.Text);

                    if (img.Height > img.Width)
                        if (img.Height > 400)
                        {
                            int newidth = 400 * img.Width / img.Height;
                            img = ResizeImage(img, newidth, 400);
                        }
                        else
                        {
                            int neHeight = 400 * img.Height / img.Width;
                            img = ResizeImage(img, 400, img.Width);

                        }

                    ImageConverter imgCon = new ImageConverter();
                    oProducto.producto_imagen = (byte[])imgCon.ConvertTo(img, typeof(byte[]));

                }


                oProducto.producto_fecha = DateTime.Now;

                Decimal.TryParse(Pr_costeproducto.Text, out decimal d);
                oProducto.producto_costeunidad = d;

                oProducto.producto_idempresa = App.idEmpresa;

                if (Pr_tienda.SelectedIndex != -1)
                {
                    TiendasViewModel item = (TiendasViewModel)Pr_tienda.SelectedItem;

                    oProducto.producto_tiendaid = item.tienda_id;
                }
                if (Pr_unidad.SelectedIndex != -1)
                {
                    ComboBoxItem item = (ComboBoxItem)Pr_unidad.SelectedItem;
                    oProducto.producto_unidad = item.Content.ToString();
                }
                oProducto.producto_codigoproducto = Pr_codigoproducto.Text;
                oProducto.producto_nombre = Pr_nombreproducto.Text;
                Decimal.TryParse(Pr_precioproducto.Text, out d);
                oProducto.producto_preciounidad = d;
                Decimal.TryParse(Pr_iva.Text, out d);
                oProducto.producto_ivaporcent = d;
                Decimal.TryParse(Pr_costefabricacion.Text, out d);
                oProducto.producto_costefabricación = d;
                if (!String.IsNullOrEmpty(Pr_stock.Text))
                {
                    Decimal.TryParse(Pr_stock.Text, out d);
                    oProducto.producto_stock = d;
                }
                db.GbB_productos.Add(oProducto);

                db.SaveChanges();
                refreshProductos();
                lstControb = new List<Control>();
                lstControb.Add(Pr_costeproducto);
                lstControb.Add(Pr_iva);
                lstControb.Add(Pr_nombreproducto);
                lstControb.Add(Pr_precioproducto);
                lstControb.Add(Pr_Imagen);
                lstControb.Add(Pr_costefabricacion);
                lstControb.Add(Pr_stock);
                if (Pr_costeproducto.IsEnabled)
                    lstControb.Add(Pr_costeproducto);


                App.quitarErrores(lstControb);
                App.Limpiar(lstControb);


            }
        }

        private void EliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LProductos.SelectedIndex != -1)

                    if (MessageBox.Show("Eliminar Registro?", "¿Realemte deseas elminar este registro ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        bool tieneDependencias = false;

                        if (tieneDependencias) { MessageBox.Show("Este registro tiene dependencias y no se puede eliminar."); return; }

                        ViewModel.ProductosViewModel oProducto = (ProductosViewModel)LProductos.SelectedItem; ;




                        using (etddatabaseEntities1 db = new etddatabaseEntities1())
                        {
                            var producto = db.GbB_productos.Find(oProducto.producto_id);
                            db.GbB_productos.Remove(producto);
                            db.SaveChanges();
                            refreshProductos();
                        }
                    }
            }
            catch (Exception er)
            {

                Console.Write(er.Message);
            }
        }

        private void MaterialesProducto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FichaProducto_Click(object sender, RoutedEventArgs e)
        {
            if (LProductos.SelectedIndex != -1)
            {

                ProductosViewModel oProducto = (ProductosViewModel)LProductos.SelectedItem;
                 NavigationService nav = NavigationService.GetNavigationService(this);
            FichaProducto r1 = new FichaProducto(oProducto.producto_id);
            nav.Navigate(r1);
              //  FichaProducto ficha = new FichaProducto(oProducto.producto_id, this);
               //ficha.Show();
            }
        }

        private void Pr_unidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            ComboBoxItem item = (ComboBoxItem)Pr_unidad.SelectedItem;

            if (item.Content.ToString() == Multilanguage.Usa_materiales)
            {
                Pr_costeproducto.Text = "";
                Pr_costeproducto.IsEnabled = false;


            }
            else
                Pr_costeproducto.IsEnabled = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            refreshProductos();
            refreshTiendas();
        }
    }
}
