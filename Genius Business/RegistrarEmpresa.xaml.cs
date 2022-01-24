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
using System.Windows.Shapes;
using Genius_Business.MainPages.RegistroEmpresa;
namespace Genius_Business
{
    /// <summary>
    /// Interaction logic for RegistrarEmpresa.xaml
    /// </summary>
    public partial class RegistrarEmpresa : Window
    {
        public RegistrarEmpresa()
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

            Navegador.Content= new PrimeraPregunta();
        }
       
    }
}
