using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Aquí puedes verificar las credenciales
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Ejemplo de verificación de credenciales (¡reemplaza con tu lógica real!)
            if (username == "usuario" && password == "contraseña")
            {
                // Las credenciales son válidas, puedes redirigir a otra página
                //Frame.Navigate(typeof(OtraPagina)); // Reemplaza 'OtraPagina' con el nombre de tu siguiente página
            }
            else
            {
                // Las credenciales son inválidas, muestra un mensaje de error
                txtMessage.Text = "Credenciales incorrectas. Por favor, inténtalo de nuevo.";
            }
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }
    }
}
