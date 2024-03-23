using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Windows.Foundation;
using mycooking.Models;
using System.Diagnostics;
using Windows.Storage;

namespace mycooking.Services
{
    public class ApiService
    {
        private static ApiService _instance;
        private readonly HttpClient _client = new HttpClient();
        private string _accessToken;



        public string AccessToken
        {
            get { return _accessToken; }
            private set { _accessToken = value; }
        }
        private ApiService()
        {
            // Configurar la dirección base en el constructor una sola vez
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:9098/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public static ApiService GetInstance()
        {
            // Si no existe una instancia, crear una nueva
            if (_instance == null)
            {
                _instance = new ApiService();
            }
            return _instance;
        }
        public void SetAccessToken(string token)
    {
        AccessToken = token;
    }

        //Login
        public async Task<bool> Login(string correo, string contrasenya)
        {
            try
            {
                var data = new { correo, contrasenya };
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync("login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Respuesta del servidor: " + responseData);

                    // Obtener el token de acceso desde la respuesta y guardarlo en el objeto ApiService
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseData);
                    if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        AccessToken = tokenResponse.AccessToken;

                        // Guardar el token de acceso en el almacenamiento local para persistencia
                        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                        localSettings.Values["AccessToken"] = AccessToken;

                        Debug.WriteLine("Token de acceso obtenido: " + AccessToken);
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("El inicio de sesión fue exitoso pero no se obtuvo el token.");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al iniciar sesión: " + ex.Message);
                return false;
            }
        }

        //Register
        public async Task<string> Register(string correo, string contrasenya)
        {
            var data = new { correo, contrasenya };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("register", content);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                throw new HttpRequestException($"Error al registrar. Código de estado: {response.StatusCode}");
            }
        }

        public async Task CrearReceta(Receta receta)
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                string accessToken = localSettings.Values["AccessToken"] as string;
                // Asegurarse de que se tenga el token de autenticación
                if (string.IsNullOrEmpty(AccessToken))
                {
                    throw new InvalidOperationException("Debe iniciar sesión antes de crear una receta.");
                }

                var json = JsonConvert.SerializeObject(receta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Configurar el encabezado de autorización con el token de acceso
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                // Realizar la solicitud con la instancia existente de HttpClient
                HttpResponseMessage response = await _client.PostAsync("recetas", content);

                string responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Respuesta del servidor: " + responseContent);

                // Verificar si la respuesta indica un error de "Unauthorized"
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("No tienes permiso para realizar esta acción. Por favor, inicia sesión nuevamente.");
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Receta creada exitosamente.");
                    // Aquí puedes mostrar el mensaje de éxito o realizar otras operaciones relacionadas con el éxito
                }
                else
                {
                    throw new HttpRequestException($"Error al crear la receta. Código de estado: {response.StatusCode}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine("Error al crear la receta: " + ex.Message);

                // Manejar la excepción aquí
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al crear la receta: " + ex.Message);
                // Otras excepciones pueden ser manejadas aquí si es necesario
            }
        }

        public class TokenResponse
        {
            [JsonProperty("token")]
            public string AccessToken { get; set; }
        }

    }
}
