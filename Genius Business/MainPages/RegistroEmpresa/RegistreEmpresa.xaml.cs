using System;
using System.Collections.Generic;
using System.Data;
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
using Genius_Business.MainPages.Pàgines;
using Genius_Business.Model;
using Genius_Business.ViewModel;
namespace Genius_Business.MainPages.RegistroEmpresa
{
    /// <summary>
    /// Interaction logic for RegistreEmpresa.xaml
    /// </summary>
    public partial class RegistreEmpresa : Page
    {
        public RegistreEmpresa()
        {
            InitializeComponent();
           // Page_Loaded();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           List<PaisesViewModel> lst = new List<PaisesViewModel>();
            using (etddatabaseEntities1 db = new etddatabaseEntities1()) {


             

                lst= (from d in db.Paises
                                             select new PaisesViewModel()

                                             {
                                                 paises_id = d.paises_id,
                                                 paises_pais = d.paises_pais,
                                                 paises_moneda = d.paises_moneda
                                             }).ToList()
;



                this.cmbPaises.DisplayMemberPath = "paises_pais";
                this.cmbPaises.SelectedValuePath = "paises_id";
                this.cmbPaises.ItemsSource = lst;
                this.cmbPaises.SelectedItem = -1;

            }

        }
        

        private void AsociarEmpesa_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();

            nombreempresa.Background = (Brush)bc.ConvertFrom("#ffffff");
           
                emailprincipal.Background = (Brush)bc.ConvertFrom("#ffffff");

           
                pasword.Background = (Brush)bc.ConvertFrom("#ffffff");
         
                repetirpasword.Background = (Brush)bc.ConvertFrom("#ffffff");
           
                numTel.Background = (Brush)bc.ConvertFrom("#ffffff");




            if (String.IsNullOrEmpty(nombreempresa.Text) || String.IsNullOrEmpty(emailprincipal.Text) || String.IsNullOrEmpty(numTel.Text) || String.IsNullOrEmpty(pasword.Password) || String.IsNullOrEmpty(repetirpasword.Password)) {
                MessageBox.Show("Debe rellenar todos los campos");

               
               if (String.IsNullOrEmpty(nombreempresa.Text))
                    nombreempresa.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                if (String.IsNullOrEmpty(emailprincipal.Text))
                    emailprincipal.Background = (Brush)bc.ConvertFrom("#ffb9c0");

                if (String.IsNullOrEmpty(pasword.Password))
                    pasword.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                if (String.IsNullOrEmpty(repetirpasword.Password))
                    repetirpasword.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                if (String.IsNullOrEmpty(numTel.Text))
                    numTel.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                return;
            }

            if (cmbPaises.SelectedIndex <0) {
                MessageBox.Show("Debe seleccionar un pais");
                return;

            }
            if (!App.IsValidEmail(emailprincipal.Text)) {
                MessageBox.Show("Email inválido");
                emailprincipal.Background = (Brush)bc.ConvertFrom("#ffb9c0");

                return;

            }
            if (!App.IsValidPassword(pasword.Password)) {
                MessageBox.Show("El password  no es válidor. Debe contener al menos 8 caractere, con numeros y letras"); 
                pasword.Focus();
              
                repetirpasword.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                return;
            }

            if (pasword.Password != repetirpasword.Password) {
                MessageBox.Show("El password  debe coincidir");
                repetirpasword.Focus();
                bc = new BrushConverter();
                repetirpasword.Background= (Brush)bc.ConvertFrom("#ffb9c0");
                return;
            
            
            }





  var oEmpresa = new GnB_Empresas();
            var oLicenciasEmpressas = new GnB_EmpresasLicencias();
            using (etddatabaseEntities1 db = new etddatabaseEntities1())
            {

                List<EmpresasViewModel> lst = new List<EmpresasViewModel>();
                lst = (from d in db.GnB_Empresas
                       where d.Emailprincipal == emailprincipal.Text
                       select new EmpresasViewModel()

                       {
                           Empresa_idEmpresa = d.Empresa_idEmpresa,
                           Emailprincipal = d.Emailprincipal,
                           Nombre_Empresa = d.Nombre_Empresa,
                           Pais = d.Pais


                       }).ToList();

                if (lst.Count > 0) {

                    MessageBox.Show("Este email ya es el email principal de  una empresa. Inicie sesion con ese email.");


                    return;
                
                }
                string iduserstring = App.idUser.Trim();
                long user = Convert.ToInt64(iduserstring);
                List<LicenciasViewModel> lst2 = new List<LicenciasViewModel>();
                lst2 = (from d in db.Licencias
                        where d.id == user
                        select new LicenciasViewModel()

                        {
                            testing = d.testing

                        }).ToList();
                LicenciasViewModel licencia = lst2[0];
                oEmpresa.Nombre_Empresa = nombreempresa.Text;

                oEmpresa.Emailprincipal = emailprincipal.Text;

                oEmpresa.ContraseñaEmpresa = pasword.Password;

                oEmpresa.NumTel = numTel.Text;
                
                string pais= cmbPaises.SelectedValue.ToString();
                oEmpresa.Pais = pais;
                if(licencia.testing == true) 
                    oEmpresa.cuentastrial = 2;
                else
                    oEmpresa.cuentastrial = 3;

                db.GnB_Empresas.Add(oEmpresa);
                db.SaveChanges();

                App.idEmpresa = oEmpresa.Empresa_idEmpresa;
                oLicenciasEmpressas.Emlc_idEmpresa = oEmpresa.Empresa_idEmpresa;
                 
                
                oLicenciasEmpressas.Emlc_idlicencia = user;
                oLicenciasEmpressas.Nombre_Empresa = nombreempresa.Text;
                oLicenciasEmpressas.Nombre_Pais = pais;
                db.GnB_EmpresasLicencias.Add(oLicenciasEmpressas);
                db.SaveChanges();
                MessageBox.Show("Asignación finalizada correctamente!");
                MainWindow win = ((MainWindow)Window.GetWindow(this));
                win.Bloqueo = false; 
                if (win.home == null)
                    win.home = new Home();
                win.Navegador.Navigate(win.home);
            }
        }

    
    }
}
