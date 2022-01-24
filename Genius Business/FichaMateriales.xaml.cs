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
using System.Windows.Shapes;
using Image = System.Drawing.Image;
using Genius_Business.Resources;

namespace Genius_Business
{
    /// <summary>
    /// Interaction logic for FichaMateriales.xaml
    /// </summary>
    public partial class FichaMateriales : Window
    {
        MainWindow main;
        byte[] imagebytes;
        long idMaterial;
        public event PropertyChangedEventHandler matopertyChanged;

        public FichaMateriales(long idMaterial, MainWindow main)
        {
            this.idMaterial = idMaterial;
            this.main = main;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            refreshTiendas();
            refreshMateriales();

        }
        private void refreshMateriales()
        {
            List<ViewModel.MaterialesViewModel> Materiales = new List<ViewModel.MaterialesViewModel>();
            try
            {
                using (etddatabaseEntities1 db = new etddatabaseEntities1())
                {

                    var oMaterial = db.GbB_materiales.Find(idMaterial);
                    //db.CommandTimeout = 120;

                    //  mat_= oMaterial.material_id;
                    mat_costematerial.Text = oMaterial.material_costeunidad.ToString();
                    //material_fecha.Text = oMaterial.material_fecha.;
                    //material_idempresa = oMaterial.material_idempresa;

                    mat_nombrematerial.Text = oMaterial.material_nombre;
                    
                    mat_stock.Text = oMaterial.material_stock.ToString();
                    // mat_tienda.SelectedItem= oMaterial.material_tiendaid;

                    mat_tienda.SelectedValue = oMaterial.material_tiendaid;

                    mat_unidad.SelectedItem = oMaterial.material_unidad;
                    imagebytes = oMaterial.material_imagen;
                    image.Source = ToImage(oMaterial.material_imagen);
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



                    mat_tienda.DisplayMemberPath = "tienda_NombreTienda";
                    mat_tienda.SelectedValuePath = "tienda_id";
                    mat_tienda.ItemsSource = Tiendas;
                    mat_tienda.SelectedItem = -1;
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
        private void ModificarMaterial_Click(object sender, RoutedEventArgs e)
        {

            List<Control> lstControb = new List<Control>();
            lstControb.Add(mat_costematerial);
            
            lstControb.Add(mat_nombrematerial);
     
            lstControb.Add(mat_Imagen);
            lstControb.Add(mat_stock);
            lstControb.Add(mat_costematerial);


            App.quitarErrores(lstControb);


            if (!String.IsNullOrEmpty(mat_Imagen.Text) && !File.Exists(mat_Imagen.Text))
            {

                MessageBox.Show("El fichero seleccionado ya no existe!");
                return;
            }
            CheckCampos ck;

            ck = App.checkTextBox(mat_nombrematerial, "Nombre del material", "");
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }
            ck = App.checkTextBox(mat_stock, "Stock", "decimal", false);
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }
            ck = App.checkTextBox(mat_costematerial, " Coste del material", "decimal", mat_costematerial.IsEnabled);
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }
            
            ck = App.checkTextBox(mat_costefabricacion, "Coste fabricación", "decimal", false);
            if (!ck.ok) { MessageBox.Show(ck.mesage); return; }



            var oMaterial = new GbB_materiales();
            using (etddatabaseEntities1 db = new etddatabaseEntities1())
            {
                oMaterial.material_imagen = imagebytes;

                if (!String.IsNullOrEmpty(mat_Imagen.Text))
                {
                    Image img = Image.FromFile(mat_Imagen.Text);

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
                    oMaterial.material_imagen = (byte[])imgCon.ConvertTo(img, typeof(byte[]));

                }
                oMaterial.material_id = idMaterial;

                oMaterial.material_fecha = DateTime.Now;

                Decimal.TryParse(mat_costematerial.Text, out decimal d);
                oMaterial.material_costeunidad = d;

                oMaterial.material_idempresa = App.idEmpresa;

                if (mat_tienda.SelectedIndex != -1)
                {
                    TiendasViewModel item = (TiendasViewModel)mat_tienda.SelectedItem;

                    oMaterial.material_tiendaid = item.tienda_id;
                }
                if (mat_unidad.SelectedIndex != -1)
                {
                    ComboBoxItem item = (ComboBoxItem)mat_unidad.SelectedItem;
                    oMaterial.material_unidad = item.Content.ToString();
                }
                oMaterial.material_nombre = mat_nombrematerial.Text;
               
                Decimal.TryParse(mat_costefabricacion.Text, out d);
                oMaterial.material_costefabricación = d;
                if (!String.IsNullOrEmpty(mat_stock.Text))
                {
                    Decimal.TryParse(mat_stock.Text, out d);
                    oMaterial.material_stock = d;
                }
                db.Entry(oMaterial).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                refreshMateriales();
                lstControb = new List<Control>();
                lstControb.Add(mat_costematerial);
               
                lstControb.Add(mat_nombrematerial);
               
                lstControb.Add(mat_Imagen);
                lstControb.Add(mat_costefabricacion);
                lstControb.Add(mat_stock);
                if (mat_costematerial.IsEnabled)
                    lstControb.Add(mat_costematerial);


                ///main.refreshMateriales();
                this.Close();


            }
        }
        private void CancelarrMaterial_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ExamnarMaterial_Click(object sender, RoutedEventArgs e)
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
                mat_Imagen.Text = openfile.FileName;
            }
            // Image img = Image.FromFile(mat_Imagen.Text);
            if (!String.IsNullOrEmpty(mat_Imagen.Text))
            {
                Image img = Image.FromFile(mat_Imagen.Text);

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

        private void mat_unidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBoxItem item = (ComboBoxItem)mat_unidad.SelectedItem;
            if (item != null)
                if (item.Content.ToString() == Multilanguage.Usa_materiales)
                {
                    mat_costematerial.Text = "";
                    mat_costematerial.IsEnabled = false;


                }
                else
                    mat_costematerial.IsEnabled = true;
        }
    }
}
