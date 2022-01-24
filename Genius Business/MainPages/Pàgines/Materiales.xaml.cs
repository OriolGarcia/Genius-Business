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
    /// Interaction logic for Materiales.xaml
    /// </summary>
    public partial class Materiales : Page
    {
        public Materiales()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            refreshMateriales();
        }

        private void ExamnarMAterial_Click(object sender, RoutedEventArgs e)
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
        }

        private void AñadirMAterial_Click(object sender, RoutedEventArgs e)
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



            var omaterial = new GbB_materiales();
            using (etddatabaseEntities1 db = new etddatabaseEntities1())
            {


                if (!String.IsNullOrEmpty(mat_Imagen.Text))
                {
                    Image img = Image.FromFile(mat_Imagen.Text);

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
                    omaterial.material_imagen = (byte[])imgCon.ConvertTo(img, typeof(byte[]));

                }


                omaterial.material_fecha = DateTime.Now;

                Decimal.TryParse(mat_costematerial.Text, out decimal d);
                omaterial.material_costeunidad = d;

                omaterial.material_idempresa = App.idEmpresa;

                if (mat_tienda.SelectedIndex != -1)
                {
                    TiendasViewModel item = (TiendasViewModel)mat_tienda.SelectedItem;

                    omaterial.material_tiendaid = item.tienda_id;
                }
                if (mat_unidad.SelectedIndex != -1)
                {
                    ComboBoxItem item = (ComboBoxItem)mat_unidad.SelectedItem;
                    omaterial.material_unidad = item.Content.ToString();
                }
                omaterial.material_nombre = mat_nombrematerial.Text;
                Decimal.TryParse(mat_costefabricacion.Text, out d);
                omaterial.material_costefabricación = d;
                if (!String.IsNullOrEmpty(mat_stock.Text))
                {
                    Decimal.TryParse(mat_stock.Text, out d);
                    omaterial.material_stock = d;
                }
                db.GbB_materiales.Add(omaterial);

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


                App.quitarErrores(lstControb);
                App.Limpiar(lstControb);


            }

        }
        private void Fichamaterial_Click(object sender, RoutedEventArgs e)
        {
            if (LMateriales.SelectedIndex != -1)
            {

                MaterialesViewModel oMaterial = (MaterialesViewModel)LMateriales.SelectedItem;

               // FichaMateriales ficha = new FichaMateriales(oMaterial.material_id);
                //ficha.Show();
            }

        }
        private void EliminarMaterial_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (LMateriales.SelectedIndex != -1)

                    if (MessageBox.Show("Eliminar Registro?", "¿Realemte deseas elminar este registro ?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        bool tieneDependencias = false;

                        if (tieneDependencias) { MessageBox.Show("Este registro tiene dependencias y no se puede eliminar."); return; }

                        ViewModel.MaterialesViewModel oMAteriales = (MaterialesViewModel)LMateriales.SelectedItem; ;




                        using (etddatabaseEntities1 db = new etddatabaseEntities1())
                        {
                            var material = db.GbB_materiales.Find(oMAteriales.material_id);
                            db.GbB_materiales.Remove(material);
                            db.SaveChanges();
                            refreshMateriales();
                        }
                    }
            }
            catch (Exception er)
            {

                Console.Write(er.Message);
            }
        }

        private void MaterialesMateriales_Click(object sender, RoutedEventArgs e)
        {

        }
        public void refreshMateriales()
        {
            List<ViewModel.MaterialesViewModel> Materiales = new List<ViewModel.MaterialesViewModel>();
            try
            {
                using (etddatabaseEntities1 db = new etddatabaseEntities1())
                {
                    //db.CommandTimeout = 120;
                    Materiales = (from d in db.GbB_materiales
                                  join c in db.GbB_Tiendas
                         on d.material_tiendaid equals c.tienda_id into ps
                                  from c in ps.DefaultIfEmpty()
                                  where d.material_idempresa == App.idEmpresa
                                  select new MaterialesViewModel()

                                  {
                                      material_id = d.material_id,
                                      material_costeunidad = d.material_costeunidad,
                                      material_fecha = d.material_fecha,
                                      material_idempresa = d.material_idempresa,


                                      material_nombre = d.material_nombre,

                                      material_stock = d.material_stock,
                                      material_tiendaid = d.material_tiendaid,
                                      material_tienda = c.tienda_NombreTienda,
                                      material_unidad = d.material_unidad,
                                      material_imagen_byte = d.material_imagen,
                                      material_costefabricación = d.material_costefabricación
                                  }).ToList();

                    foreach (ViewModel.MaterialesViewModel mat in Materiales)
                    {
                        if (mat.material_imagen_byte != null)
                        {
                            MemoryStream ms = new MemoryStream(mat.material_imagen_byte, 0, mat.material_imagen_byte.Length);
                            ms.Write(mat.material_imagen_byte, 0, mat.material_imagen_byte.Length);

                            if (Encoding.UTF8.GetString(mat.material_imagen_byte) != "C:")

                                mat.material_imagen = ToImage(mat.material_imagen_byte);
                        }


                    }
                    LMateriales.ItemsSource = Materiales;
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
        private void mat_unidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            ComboBoxItem item = (ComboBoxItem)mat_unidad.SelectedItem;

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
