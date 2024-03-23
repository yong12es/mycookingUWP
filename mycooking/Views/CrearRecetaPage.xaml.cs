using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Net.Http.Headers;
using mycooking.Services;
using mycooking.Models;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CrearRecetaPage : Page
    {
        
        private StorageFile imagenSeleccionada;
        private ApiService _apiService;

        public CrearRecetaPage()
        {
            this.InitializeComponent();
            _apiService = new ApiService();
        }

        private void AgregarIngrediente_Click(object sender, RoutedEventArgs e)
        {
            StackPanel nuevoIngredientePanel = new StackPanel();
            nuevoIngredientePanel.Orientation = Orientation.Horizontal;
            nuevoIngredientePanel.Margin = new Windows.UI.Xaml.Thickness(0, 5, 0, 0);

            TextBox nuevoIngredienteTextBox = new TextBox();
            nuevoIngredienteTextBox.PlaceholderText = "Ingrediente";
            nuevoIngredienteTextBox.Width = 150;

            TextBox nuevaCantidadTextBox = new TextBox();
            nuevaCantidadTextBox.PlaceholderText = "Cantidad";
            nuevaCantidadTextBox.Width = 100;
            nuevaCantidadTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 0, 0, 0);

            nuevoIngredientePanel.Children.Add(nuevoIngredienteTextBox);
            nuevoIngredientePanel.Children.Add(nuevaCantidadTextBox);

           
            IngredientesStackPanel.Children.Add(nuevoIngredientePanel);
        }

        private async void CrearReceta_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
             //De aqui no pasa  
                if (string.IsNullOrEmpty(_apiService.AccessToken))
                {
                    // El token de acceso está vacío, mostrar un mensaje de error

                    Debug.WriteLine("_accestoken:" + _apiService._accessToken);
                    Debug.WriteLine("AccesToekn" + _apiService.AccessToken);
                    MostrarMensaje("Debe iniciar sesión antes de crear una receta Receta.Page El token de acceso está vacío.");
                    return;
                }

                string nombre = NombreRecetaTextBox.Text;
            string descripcion = DescripcionTextBox.Text;
            string instrucciones = InstruccionesTextBox.Text;

            // Crear la receta
            Receta receta = new Receta
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Instrucciones = instrucciones
            };

           
                // Crear la receta usando ApiService
                await _apiService.CrearReceta(receta);

                // Mostrar un mensaje de éxito
                MostrarMensaje("Receta creada exitosamente.");
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje de error si ocurre alguna excepción
                MostrarMensaje($"Error al crear la receta: {ex.Message}");
            }
        }

        private async void CargarImagen_Click(object sender, RoutedEventArgs e)
        {

            // Configurar el FileOpenPicker
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpg");

            imagenSeleccionada = await filePicker.PickSingleFileAsync();
            if (imagenSeleccionada != null)
            {
                // Abrir el archivo seleccionado y cargar la imagen en el control de imagen
                using (IRandomAccessStream fileStream = await imagenSeleccionada.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    miImagenControl.Source = bitmapImage; 
                }
            }
        }
        private void MostrarMensaje(string mensaje)
        {
            
            var dialog = new Windows.UI.Popups.MessageDialog(mensaje);
            _ = dialog.ShowAsync();
        }
    }
}
