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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace mycooking.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CrearRecetaPage : Page
    {
        private const string apiUrl = "http://localhost:9098/recetas";
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
            Debug.WriteLine("Iniciando el proceso para crear una nueva receta...");

            // Obtener los datos de la receta de los controles de la interfaz de usuario
            string nombre = NombreRecetaTextBox.Text;
            string descripcion = DescripcionTextBox.Text;
            string instrucciones = InstruccionesTextBox.Text;
            
            string imagenBase64 = "";

            Debug.WriteLine($"Nombre de la receta: {nombre}");
            Debug.WriteLine($"Descripción: {descripcion}");
            Debug.WriteLine($"Instrucciones: {instrucciones}");
            Debug.WriteLine($"Token de acceso: {_apiService._accessToken}");

            if (imagenSeleccionada != null)
            {
                // Leer el contenido del archivo como una matriz de bytes
                byte[] imagenBytes;
                using (var stream = await imagenSeleccionada.OpenReadAsync())
                {
                    imagenBytes = new byte[stream.Size];
                    await stream.ReadAsync(imagenBytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
                }

                // Convertir la imagen a una cadena base64
                imagenBase64 = Convert.ToBase64String(imagenBytes);
            }
            Debug.WriteLine($"Imagen base64: {imagenBase64}");



            // Crear un objeto JSON con los datos de la receta
            string jsonBody = $"{{\"nombre\":\"{nombre}\",\"descripcion\":\"{descripcion}\",\"instrucciones\":\"{instrucciones}\",\"imagen\":\"{imagenBase64}\"}}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (string.IsNullOrEmpty(_apiService._accessToken))
                    {
                        // El token de acceso está vacío, por lo que no podemos continuar
                        MostrarMensaje("El token de acceso está vacío. Inicia sesión primero.");
                        return;
                    }
                    // Configurar el encabezado de la solicitud para indicar que el cuerpo es JSON
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Crear el contenido de la solicitud HTTP con el cuerpo JSON
                    HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    string accessToken = _apiService.AccessToken;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Realizar la solicitud HTTP POST a la API para crear la nueva receta
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Verificar si la solicitud fue exitosa (código de estado 201 Created)
                    if (response.IsSuccessStatusCode)
                    {
                        
                        MostrarMensaje("Receta creada exitosamente."); 
                    }
                    else
                    {
                        // Si la solicitud no fue exitosa, mostrar un mensaje de error basado en el código de estado
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MostrarMensaje($"Hubo un error al crear la receta. Código de estado: {response.StatusCode}. Detalles: {errorMessage}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    MostrarMensaje($"Error de solicitud HTTP: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción que ocurra durante la solicitud HTTP
                    
                    MostrarMensaje("se produje un error al crear la receta. Detalles:" + ex.Message);
                }

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

            // Mostrar el FileOpenPicker y esperar a que el usuario seleccione un archivo
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
