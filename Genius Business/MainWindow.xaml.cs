using Genius_Business.MainPages.Pàgines;
using Genius_Business.MainPages.RegistroEmpresa;
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

namespace Genius_Business
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       public bool Bloqueo = false;

        public Home home;
        public Ventas ventas;
        public Productos productos;
        public Materiales materiales;
        public Finanzas finanzas;
        public Mensajeria mensajeria;
        public LogOut logout;
        public Tiendas tiendas;
        public Configuración configuracion;

        public MainWindow()
        {
            InitializeComponent();
            Navegador.JournalOwnership = JournalOwnership.OwnsJournal;
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (Tg_Btn.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_finances.Visibility = Visibility.Collapsed;
                tt_bandages.Visibility = Visibility.Collapsed;
                tt_products.Visibility = Visibility.Collapsed;
                tt_materials.Visibility = Visibility.Collapsed;
                tt_messages.Visibility = Visibility.Collapsed;
                tt_settings.Visibility = Visibility.Collapsed;
                tt_signout.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_finances.Visibility = Visibility.Visible;
                tt_bandages.Visibility = Visibility.Visible;
                tt_products.Visibility = Visibility.Visible;
                tt_materials.Visibility = Visibility.Visible;
                tt_messages.Visibility = Visibility.Visible;
                tt_settings.Visibility = Visibility.Visible;
                tt_signout.Visibility = Visibility.Visible;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {



            if (!File.Exists("Security.GNB"))
            {
                Navegador.Content = new ValidarLicencia();
                Bloqueo = true;
                return;
                App.licenceOk = false;
                ;
            }
            else
            {

                try
                {




                    String text = File.ReadAllText("Security.GNB");

                    string desencriptado = CheckLicencia.Desencriptar(text);
                    if (String.IsNullOrEmpty(desencriptado))
                    {
                        Bloqueo = true;
                        return;
                    }
                    string[] separado = desencriptado.Replace("\r\n", "\n").Split("\n".ToCharArray());
                    App.Email = separado[0];
                    App.main = this;
                    App.idUser = separado[8];

                    CheckLicencia checkLic = new CheckLicencia();

                    DateTime lim;
                    string fecha = separado[6];
                    if (fecha == "0001-01-01 00:00:00")
                    {
                        lim = new DateTime(0);
                    }
                    else
                    {
                        lim = DateTime.ParseExact(separado[6], "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    if (checkLic.checkLicence(false, separado[0], separado[2], separado[4], lim))
                    {
                        App.licenceOk = true;

                        if (comprobarEmpresa(App.idUser))
                        {
                            Bloqueo = false;
                            Navegador.Content = new Home();
                        }

                        else
                        {
                            Bloqueo = true;
                            Navegador.Content = new PrimeraPregunta();
                        }

                    }
                    else
                    {
                        Bloqueo = true;
                        Navegador.Content = new ValidarLicencia();

                        App.licenceOk = false;
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

            }

        }
        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {

                case "ca-ES":
                case "es-ES":
                case "eu-ES":
                case "es-AR":
                case "es-BO":
                case "es-CL":
                case "es-CO":
                case "es-CR":
                case "es-DO":
                case "es-EC":
                case "es-GT":
                case "es-HN":
                case "es-MX":
                case "es-NI":
                case "es-PA":
                case "es-PE":
                case "es-PR":
                case "es-PY":
                case "es-SV":
                case "es-US":
                case "es-UY":
                case "es-VE":
                    dict.Source = new Uri("..\\Resources\\Multilanguage.xaml", UriKind.Relative);
                    App.lang = "es";
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("");
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\Multilanguage.EN.xaml", UriKind.Relative);
                    App.lang = "en";
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("EN");
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
        public void BloquearPantalla()
        {

            Bloqueo = true;

        }
        public void DesbloquearPantalla()
        {
            Bloqueo = false;

        }
        public bool comprobarEmpresa(string iduser)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Azure"].ConnectionString))
                {

                    con.Open();

                    using (SqlCommand command = new SqlCommand("SELECT Emlc_idEmpresa  FROM GnB_EmpresasLicencias inner join Licencias on Emlc_idlicencia= Licencias.id where Licencias.id = @idUser", con))
                    {
                        command.Parameters.AddWithValue("@idUser", iduser);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {


                                return false;
                            }
                            else { App.idEmpresa = reader.GetInt64(0); return true; }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                return false;
            };
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

            var item = sender as ListViewItem;
            if (!Bloqueo && item != null && item.IsSelected)
            {

                switch (item.Name)
                {
                    case "Home":
                        if (home == null)
                            home = new Home();
                        Navegador.Navigate(home);
                        // Navegador.Navigate(home);
                        break;
                    case "Productos":
                        if (productos == null)
                            productos = new Productos();
                        Navegador.Navigate(productos);
                        break;
                    case "Finanzas":
                        if (finanzas == null)
                            finanzas = new Finanzas();
                        Navegador.Navigate(finanzas);
                        break;
                    case "Ventas":
                        if (ventas == null)
                            ventas = new Ventas();
                        Navegador.Navigate(ventas);
                        break;
                    case "Tiendas":
                        if (tiendas == null)
                            tiendas = new Tiendas();
                        Navegador.Navigate(tiendas);
                        break;
                    case "Mensajes":
                        if (mensajeria == null)
                            mensajeria = new Mensajeria();
                        Navegador.Navigate(mensajeria);
                        break;
                    case "Materiales":
                        if (materiales == null)
                            materiales = new Materiales();
                        Navegador.RemoveBackEntry();
                        Navegador.Navigate(materiales);
                        break;
                    case "Configuracion":
                        if (configuracion == null)
                            configuracion = new Configuración();
                        Navegador.Navigate(configuracion);
                        break;
                    case "Log_Out":
                        if (logout == null)
                            logout = new LogOut();
                        Navegador.Navigate(logout);

                        break;

                }
            }
        }

   

        private void Paypal_Click(object sender, RoutedEventArgs e)
        {


            MainPages.Pàgines.WebBrowser browser = new MainPages.Pàgines.WebBrowser();
            Navegador.Navigate(browser);
        }
    }
    }
