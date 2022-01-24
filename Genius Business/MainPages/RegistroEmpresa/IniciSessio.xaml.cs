using Genius_Business.MainPages.Pàgines;
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

namespace Genius_Business.MainPages.RegistroEmpresa
{
    /// <summary>
    /// Interaction logic for IniciSessio.xaml
    /// </summary>
    public partial class IniciSessio : Page
    {
        public IniciSessio()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                string iduserstring = App.idUser.Trim();
                long user = Convert.ToInt64(iduserstring);

                var oEmpresa = new GnB_Empresas();
                var oLicenciasEmpressas = new GnB_EmpresasLicencias();
                using (etddatabaseEntities1 db = new etddatabaseEntities1())
                {

                    List<EmpresasViewModel> lst = new List<EmpresasViewModel>();
                    lst = (from d in db.GnB_Empresas
                           where d.Emailprincipal == Email.Text && d.ContraseñaEmpresa == Pasword.Password
                           select new EmpresasViewModel()

                           {
                               Empresa_idEmpresa = d.Empresa_idEmpresa,
                               Emailprincipal = d.Emailprincipal,
                               Nombre_Empresa = d.Nombre_Empresa,
                               Pais = d.Pais,
                               ContraseñaEmpresa= d.ContraseñaEmpresa,
                               NumTel = d.NumTel,
                               cuentastrial = d.cuentastrial

                           }).ToList();

                    List<LicenciasViewModel> lst2 = new List<LicenciasViewModel>();
                    lst2 = (from d in db.Licencias
                            where d.id == user
                            select new LicenciasViewModel()

                            {
                                testing = d.testing

                            }).ToList();
                    if (lst.Count == 0)
                    {

                        MessageBox.Show("Email  de  empresa  o contraseña incorrectos. No se puede  hacer  la asignación.");


                        return;

                    }
                    try
                    {
                        EmpresasViewModel empresa = lst[0];
                        LicenciasViewModel licencia = lst2[0];



                        if (licencia.testing == true && empresa.cuentastrial <= 0)
                        {

                            MessageBox.Show("Ya no se pueden asignar más cuentass de prueba a esta empresa. Compre la licencia.");
                            return;

                        }

                        if (licencia.testing == true)
                        {
                            oEmpresa.ContraseñaEmpresa = empresa.ContraseñaEmpresa;
                            oEmpresa.cuentastrial = empresa.cuentastrial - 1;
                            oEmpresa.Emailprincipal = empresa.Emailprincipal;
                            oEmpresa.Empresa_idEmpresa = empresa.Empresa_idEmpresa;
                            oEmpresa.Nombre_Empresa = empresa.Nombre_Empresa;
                            oEmpresa.NumTel = empresa.NumTel;
                            oEmpresa.Pais = empresa.Pais;


                            db.Entry(oEmpresa).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();
                        }


                        oLicenciasEmpressas.Emlc_idEmpresa = empresa.Empresa_idEmpresa;




                        oLicenciasEmpressas.Emlc_idlicencia = user;
                        oLicenciasEmpressas.Nombre_Empresa = empresa.Nombre_Empresa;
                        oLicenciasEmpressas.Nombre_Pais = empresa.Pais;
                        db.GnB_EmpresasLicencias.Add(oLicenciasEmpressas);
                        db.SaveChanges();
                        App.idEmpresa = empresa.Empresa_idEmpresa;
                        MessageBox.Show("Asignación finalizada correctamente!");
                        MainWindow win = ((MainWindow)Window.GetWindow(this));
                        win.Bloqueo = false;
                        if (win.home == null)
                            win.home = new Home();
                        win.Navegador.Navigate(win.home);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        }
}
