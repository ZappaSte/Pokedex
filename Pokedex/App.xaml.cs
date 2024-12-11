using Pokedex.Views;

namespace Pokedex
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Imposta una schermata di caricamento come pagina iniziale
            MainPage = new NavigationPage(new LoginPage());

            // Controlla il token
            //CheckTokenAsync();
        }

        private async void CheckTokenAsync()
        {
            try
            {
                // Recupera il token dal SecureStorage
                var token = await SecureStorage.GetAsync("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    // Se il token non esiste, mostra la pagina di login
                    MainPage = new NavigationPage(new LoginPage());
                    return;
                }

                // Verifica se il token è valido
                bool isValid = await VerifyTokenAsync(token);

                if (isValid)
                {
                    // Se il token è valido, mostra la pagina di lista Pokémon
                    MainPage = new NavigationPage(new PokemonList());
                }
                else      
                {
                    // Se il token non è valido, mostra la pagina di login
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                // In caso di errore, mostra la pagina di login
                MainPage = new NavigationPage(new LoginPage());
                Console.WriteLine($"Error checking token: {ex.Message}");
            }
        }

        private async Task<bool> VerifyTokenAsync(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("http://192.168.1.34:5000/verify-token");

                // Considera valido il token se il server restituisce 200 OK
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying token: {ex.Message}");
                return false;
            }
        }
    }
}
