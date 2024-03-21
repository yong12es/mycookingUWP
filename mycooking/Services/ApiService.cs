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




namespace mycooking.Services
{
    public class ApiService
    {
        private HttpClient _client;
        public string _accessToken;

        public string AccessToken
        {
            get { return _accessToken; }
            private set { _accessToken = value; }
        }
        public ApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:9098/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //Login
        public async Task<bool> Login(string correo, string contrasenya)
        {
            var data = new { correo, contrasenya };
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync("login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                _accessToken = JsonConvert.DeserializeObject<TokenResponse>(responseData).AccessToken;
                return true;
            }
            else
            {
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
            // Asegúrate de que tengas el token de autenticación
            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new InvalidOperationException("Debe iniciar sesión antes de crear una receta.");
            }

            // Incluir el token de autenticación en el encabezado de autorización
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            // Convertir la receta a JSON y enviarla a la API
            var json = JsonConvert.SerializeObject(receta);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
           
            HttpResponseMessage response = await _client.PostAsync("recetas", content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error al crear la receta. Código de estado: {response.StatusCode}");
            }
        }

        public class TokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }
    }
}
