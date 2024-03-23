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
        private static readonly HttpClient _client = new HttpClient();
        public string _accessToken;

        public string AccessToken
        {
            get { return _accessToken; }
            private set { _accessToken = value; }
        }
        public ApiService()
        {
            // Configura la dirección base en el constructor una sola vez
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:9098/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
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
                    _accessToken = JsonConvert.DeserializeObject<TokenResponse>(responseData).AccessToken;
                    Debug.WriteLine("Token de acceso obtenido: " + _accessToken);

                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    localSettings.Values["AccessToken"] = _accessToken;


                    // Deserializar la respuesta del servidor para obtener el token
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseData);

                    // Verificar si se obtuvo el token correctamente
                    if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        _accessToken = tokenResponse.AccessToken;
                        Debug.WriteLine("Token obtenido después del inicio de sesión: " + _accessToken);
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
                // Manejar cualquier excepción que pueda ocurrir durante el inicio de sesión
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
                // Asegúrate de que tengas el token de autenticación
                if (string.IsNullOrEmpty(_accessToken))
                {
                    throw new InvalidOperationException("Debe iniciar sesión antes de crear una receta.CrearRecetaApiservice");
                }

                var json = JsonConvert.SerializeObject(receta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Configura el encabezado de autorización aquí
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                // Realiza la solicitud con la instancia existente de HttpClient
                HttpResponseMessage response = await _client.PostAsync("recetas", content);
                if (response.IsSuccessStatusCode)
                {
                    // Receta creada exitosamente
                    Debug.WriteLine("Receta creada exitosamente.");
                }
                else
                {
                    throw new HttpRequestException($"Error al crear la receta. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al crear la receta: " + ex.Message);
                throw; // Re-lanzar la excepción para que la aplicación pueda manejarla adecuadamente
            }
        }

        public class TokenResponse
        {
            [JsonProperty("token")]
            public string AccessToken { get; set; }
        }

    }
}
