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

namespace Genius_Business.MainPages.RegistroEmpresa
{
    /// <summary>
    /// Interaction logic for PrimeraPregunta.xaml
    /// </summary>
    public partial class PrimeraPregunta : Page
    {
        public PrimeraPregunta()
        {
            InitializeComponent();

            ResourceDictionary dict = new ResourceDictionary();

            if (App.lang == "es")
            {
                dict.Source = new Uri("..\\Resources\\Multilanguage.xaml", UriKind.Relative);
                App.lang = "es";
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("");
            }
            else
            {
                dict.Source = new Uri("..\\Resources\\Multilanguage.EN.xaml", UriKind.Relative);
                App.lang = "en";
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("EN");
            }
        }

        private void Siguiente_Click(object sender, RoutedEventArgs e)
        {
            if (RYes.IsChecked == true)
            {
                NavigationService nav = NavigationService.GetNavigationService(this);
                IniciSessio i1 = new IniciSessio();
                nav.Navigate(i1);
            }
            else
            {
                NavigationService nav = NavigationService.GetNavigationService(this);
                RegistreEmpresa r1 = new RegistreEmpresa();
                nav.Navigate(r1);
            }
        }

        
    }
}
