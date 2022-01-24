using Genius_Business.MainPages.RegistroEmpresa;
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
    /// Interaction logic for ValidarLicencia.xaml
    /// </summary>
    public partial class ValidarLicencia : Page
    {
        public ValidarLicencia()
        {
            InitializeComponent();
        }



        private void Activar_Click(object sender, RoutedEventArgs e)
        {
            CheckLicencia checkLic = new CheckLicencia();
            if (checkLic.checkLicence(true, txtemail.Text, txtkey.Text, "", new DateTime(0)))
            {

                ((MainWindow)Window.GetWindow(this)).DesbloquearPantalla();

                App.licenceOk = true;
                MessageBox.Show("Producto activado!");
                if (((MainWindow)Window.GetWindow(this)).comprobarEmpresa(App.idUser))
                {


                    MainWindow win = ((MainWindow)Window.GetWindow(this));
                    win.Bloqueo = false;
                    if (win.home == null)
                        win.home = new Home();
                    win.Navegador.Navigate(win.home);
                }

                else
                {

                    MainWindow win = ((MainWindow)Window.GetWindow(this));
                    win.Bloqueo = true;
                    PrimeraPregunta r1 = new PrimeraPregunta();
                    win.Navegador.Navigate(r1);

                }
            }

            else
            {
                App.licenceOk = false;
                MessageBox.Show("El producto no está activado. Compre la clave y la recibirá en su email.");
                ((MainWindow)Window.GetWindow(this)).BloquearPantalla();
            }

        }

        private void ComprarLicenciar_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wb = new WebBrowser();
            MainWindow win = ((MainWindow)Window.GetWindow(this));
            win.Navegador.Navigate(wb);
        }
        private void Prueba3dias_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser wb = new WebBrowser("https://supremappagency.com/paypal/prueba-3dias.html");
            MainWindow win = ((MainWindow)Window.GetWindow(this));
            win.Navegador.Navigate(wb);
        }
    }
}
