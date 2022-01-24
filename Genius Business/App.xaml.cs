using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace Genius_Business
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summçary


    public class CheckCampos
    {

        public CheckCampos(bool ok, string msg)
        {
            this.ok = ok;
            this.mesage = msg;


            }

        public bool ok { get; set; }
        public string mesage { get; set; }
    }


    public partial class App : Application
    {
        public static bool licenceOk;
        public static string Email;
        public static string idUser;
        public static long idEmpresa;
        public static Window main;
        public static string lang;


        public static bool IsValidPassword(string plainText)
        {
            Regex regex = new Regex(@"((?=.*\d)(?=.*[A-Z]).{8,50})");
            Match match = regex.Match(plainText);
            return match.Success;
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static CheckCampos checkTextBox(Control control, string nombreCampo, string formato, bool necesario = true) {
           

            if ((control.GetType() == typeof(ComboBox))) { 
              


                if (((ComboBox)control).SelectedIndex == -1 && necesario)
                {

                    return new CheckCampos(false, "El campo " + nombreCampo + " se necesita para esta operación");
                }
                return new CheckCampos(true, "");


            }



            if ((control.GetType() == typeof(TextBox))) {
                BrushConverter bc = new BrushConverter();
                TextBox txtb = (TextBox)control;
            if (necesario && String.IsNullOrEmpty(txtb.Text))
            {


                txtb.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                    return new CheckCampos(false, "El campo " + nombreCampo + " se necesita para esta operación");
                
            }
            else if (!necesario && String.IsNullOrEmpty(txtb.Text)) { return new CheckCampos(true, ""); }

            bool ok;
            switch (formato) {
                case "":
                        return new CheckCampos(true, "");
                   
                case "int":
                    ok = Int32.TryParse(txtb.Text, out int result);
                    if (!ok) {
                        txtb.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                      
                            return new CheckCampos(false, "El campo " + nombreCampo + " requiere ser un número");

                    }
                    break;
                case "long":
                    ok = Int64.TryParse(txtb.Text, out long result2);
                    if (!ok)
                    {
                        txtb.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                      
                            return new CheckCampos(false, "El campo " + nombreCampo + " requiere ser un número");

                    }
                    break;
                case "decimal":
                    txtb.Text = txtb.Text.Replace(".", ",");
                    ok = Decimal.TryParse(txtb.Text, out Decimal result3);
                    if (!ok)
                    {

                        txtb.Background = (Brush)bc.ConvertFrom("#ffb9c0");
                      
                            return new CheckCampos(false, "El campo " + nombreCampo + " requiere ser un decimal");
                    }
                    break;
                default:
                        return new CheckCampos(true, "");
                        break;



            } }

            return new CheckCampos(true, "");


        }

        public static void quitarErrores(List<Control> lst) {
        
                foreach(Control control in lst)
            {
                if ((control.GetType() == typeof(TextBox)))
                {

                    TextBox txtb = (TextBox)control;
                    BrushConverter bc = new BrushConverter();

                    txtb.Background = (Brush)bc.ConvertFrom("#ffffff");
                }

            }
        
        }
        public static void Limpiar(List<Control> lst)
        {

            foreach (Control control in lst) { 
                
                
             
                if ((control.GetType() == typeof(TextBox)))
                {

                    TextBox txtb = (TextBox)control;
                  
                    txtb.Text = "";
                }

            }

        }

        public static System.Drawing.Image ResizeImage
        (System.Drawing.Image srcImage, int newWidth, int newHeight)
        {
            using (Bitmap imagenBitmap =
               new Bitmap(newWidth, newHeight))
            {
                imagenBitmap.SetResolution(
                   Convert.ToInt32(srcImage.HorizontalResolution),
                   Convert.ToInt32(srcImage.HorizontalResolution));

                using (Graphics imagenGraphics =
                        Graphics.FromImage(imagenBitmap))
                {
                    imagenGraphics.SmoothingMode =
                       SmoothingMode.AntiAlias;
                    imagenGraphics.InterpolationMode =
                       InterpolationMode.HighQualityBicubic;
                    imagenGraphics.PixelOffsetMode =
                       PixelOffsetMode.HighQuality;
                    imagenGraphics.DrawImage(srcImage,
                       new System.Drawing.Rectangle(0, 0, newWidth, newHeight),
                       new System.Drawing.Rectangle(0, 0, srcImage.Width, srcImage.Height),
                       GraphicsUnit.Pixel);
                    MemoryStream imagenMemoryStream = new MemoryStream();
                    imagenBitmap.Save(imagenMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    srcImage = System.Drawing.Image.FromStream(imagenMemoryStream);
                }
            }
            return srcImage;
        }
    }
}

