using Genius_Business.Model;
using Genius_Business.Resources;
using Genius_Business.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
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
using Image = System.Drawing.Image;

namespace Genius_Business.MainPages.Pàgines
{
    /// <summary>
    /// Interaction logic for FichaProducto.xaml
    /// </summary>
    public partial class FichaProducto : Page
    {

        MainWindow main;
        byte[] imagebytes;
        long idProducto;
        public FichaProducto(long IdProducto)
        {
            this.idProducto = IdProducto;
  
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            refreshTiendas();
            refreshProductos();

        }
        private void refreshProductos()
        {
            List<ViewModel.ProductosViewModel> Productos = new List<ViewModel.ProductosViewModel>();
            try
            {
                using (etddatabaseEntities1 db = new etddatabaseEntities1())
                {

                    var oProducto = db.GbB_productos.Find(idProducto);
                    //db.CommandTimeout = 120;

                    //  Pr_= oProducto.producto_id;
                    Pr_costeproducto.Text = oProducto.producto_costeunidad.ToString();
                    //producto_fecha.Text = oProducto.producto_fecha.;
                    //producto_idempresa = oProducto.producto_idempresa;

                    Pr_iva.Text = oProducto.producto_ivaporcent.ToString();
                    Pr_nombreproducto.Text = oProducto.producto_nombre;
                    Pr_precioproducto.Text = oProducto.producto_preciounidad.ToString();
                    Pr_stock.Text = oProducto.producto_stock.ToString();
                    // Pr_tienda.SelectedItem= oProducto.producto_tiendaid;

                    Pr_tienda.SelectedValue = oProducto.producto_tiendaid;

                    Pr_unidad.SelectedItem = oProducto.producto_unidad;
                    imagebytes = oProducto.producto_imagen;
                    image.Source = ToImage(oProducto.producto_imagen);
                    borderimage.Height = image.Source.Height / 2;
                    borderimage.Width = image.Source.Width / 2;
                    image.Height = image.Source.Height / 2;
                    image.Width = image.Source.Width / 2;


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);


            }


        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapSource image = (BitmapSource)value;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
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
        private void ModificarProducto_Click(object sender, RoutedEventArgs e)
        {

            List<Control> lstControb = new List<Control>();
            lstControb.Add(Pr_costeproducto);
            lstControb.Add(Pr_iva);
            lstControb.Add(Pr_nombreproducto);
            lstControb.Add(Pr_precioproducto);
            lstControb.Add(Pr_Imagen);
            lstControb.Add(Pr_stock);
            lstControb.Add(Pr_costeproducto);


            App.quitarErrores(lstControb);


            if (!String.IsNullOrEmpty(Pr_Imagen.Text) && !File.Exists(Pr_Imagen.Text))
            {

                MessageBox.Show("El fichero seleccionado ya no existe!");
                return;
            }
            CheckCampos ck;

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
                oProducto.producto_imagen = imagebytes;

                if (!String.IsNullOrEmpty(Pr_Imagen.Text))
                {
                    Image img = Image.FromFile(Pr_Imagen.Text);

                    if (img.Height > img.Width)
                        if (img.Height > 400)
                        {
                            int newidth = 400 * img.Width / img.Height;
                            img = App.ResizeImage(img, newidth, 400);
                        }
                        else
                        {
                            int neHeight = 400 * img.Height / img.Width;
                            img = App.ResizeImage(img, 400, img.Width);

                        }

                    ImageConverter imgCon = new ImageConverter();
                    oProducto.producto_imagen = (byte[])imgCon.ConvertTo(img, typeof(byte[]));

                }
                oProducto.producto_id = idProducto;

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
                db.Entry(oProducto).State = System.Data.Entity.EntityState.Modified;

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

                NavigationService nav = NavigationService.GetNavigationService(this);
                Productos r1 = new Productos();
                nav.Navigate(r1);


            }
        }
        private void CancelarrProducto_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            Productos r1 = new Productos();
            nav.Navigate(r1);
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
            // Image img = Image.FromFile(Pr_Imagen.Text);
            if (!String.IsNullOrEmpty(Pr_Imagen.Text))
            {
                Image img = Image.FromFile(Pr_Imagen.Text);

                if (img.Height > img.Width)
                    if (img.Height > 400)
                    {
                        int newidth = 400 * img.Width / img.Height;
                        img = App.ResizeImage(img, newidth, 400);
                    }
                    else
                    {
                        int neHeight = 400 * img.Height / img.Width;
                        img = App.ResizeImage(img, 400, img.Width);

                    }

                ImageConverter imgCon = new ImageConverter();
                image.Source = ToImage((byte[])imgCon.ConvertTo(img, typeof(byte[])));

                borderimage.Height = image.Source.Height / 2;
                borderimage.Width = image.Source.Width / 2;
                image.Height = image.Source.Height / 2;
                image.Width = image.Source.Width / 2;

            }


        }

        private void Pr_unidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBoxItem item = (ComboBoxItem)Pr_unidad.SelectedItem;
            if (item != null)
                if (item.Content.ToString() == Multilanguage.Usa_materiales)
                {
                    Pr_costeproducto.Text = "";
                    Pr_costeproducto.IsEnabled = false;


                }
                else
                    Pr_costeproducto.IsEnabled = true;
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

