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
    public sealed partial class CrearRecetaPage : Page
    {
        public CrearRecetaPage()
        {
            this.InitializeComponent();
        }

        private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            // Crear un nuevo StackPanel para contener los campos del nuevo ingrediente
            StackPanel nuevoIngredientePanel = new StackPanel();
            nuevoIngredientePanel.Orientation = Orientation.Horizontal;
            nuevoIngredientePanel.Margin = new Windows.UI.Xaml.Thickness(0, 5, 0, 0);

            // Crear los TextBox para el nuevo ingrediente y cantidad
            TextBox nuevoIngredienteTextBox = new TextBox();
            nuevoIngredienteTextBox.PlaceholderText = "Ingrediente";
            nuevoIngredienteTextBox.Width = 150;

            TextBox nuevaCantidadTextBox = new TextBox();
            nuevaCantidadTextBox.PlaceholderText = "Cantidad";
            nuevaCantidadTextBox.Width = 100;
            nuevaCantidadTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 0, 0, 0);

            // Agregar los TextBox al StackPanel
            nuevoIngredientePanel.Children.Add(nuevoIngredienteTextBox);
            nuevoIngredientePanel.Children.Add(nuevaCantidadTextBox);

            // Agregar el nuevo StackPanel al StackPanel de ingredientes
            IngredientesStackPanel.Children.Add(nuevoIngredientePanel);
        }

        private void CrearReceta_Click(object sender, RoutedEventArgs e)
        {
            // Aquí puedes agregar la lógica para crear la receta con los datos ingresados
            string nombreReceta = NombreRecetaTextBox.Text;
            string descripcion = DescripcionTextBox.Text;
            string instrucciones = InstruccionesTextBox.Text;

            // Puedes acceder a los ingredientes de la siguiente manera
            foreach (StackPanel ingredientesPanel in IngredientesStackPanel.Children)
            {
                TextBox ingredienteTextBox = ingredientesPanel.Children[0] as TextBox;
                TextBox cantidadTextBox = ingredientesPanel.Children[1] as TextBox;

                string ingrediente = ingredienteTextBox.Text;
                string cantidad = cantidadTextBox.Text;

                // Aquí puedes hacer algo con cada ingrediente (por ejemplo, agregarlo a una lista)
            }
        }


        }
}
