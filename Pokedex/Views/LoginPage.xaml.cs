using Newtonsoft.Json;
using Pokedex.Models;
using System.Net.Http;
using System.Text;

namespace Pokedex.Views;

public partial class LoginPage : ContentPage
{
    //Url dell'API per il login
    private string LoginUrl = "http://10.105.200.172:80/login";

    public LoginPage()
	{
		InitializeComponent();

        // Rimuove l'intera barra di navigazione
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string username = Username.Text;
        string password = Password.Text;

        // Controlla se i campi sono vuoti o contengono solo spazi bianchi
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ErrorLabel.Text = "Please enter both username and password.";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Login utilizzando le credenziali recuperate, ritorna true o false
        bool loginSuccess = await LoginAsync(username, password);

        if (loginSuccess)
        {
            // Salva il token per mantenere l'utente collegato
            await SecureStorage.SetAsync("authToken", "dummy_token"); //Rimani collegato
            await Navigation.PushAsync(new PokemonList()); // Vai alla pagina principale
        }
        else
        {
            // Errore se il login fallisce
            ErrorLabel.Text = "Invalid username or password.";
            ErrorLabel.IsVisible = true;
        }
    }

    // Metodo per gestire il login effettuando una richiesta HTTP POST all'API
    private async Task<bool> LoginAsync(string username, string password)
    {
        // Crea un'istanza del client HTTP
        HttpClient client = new HttpClient();

        try
        {
            // Crea un oggetto con le credenziali e lo serializza in JSON
            var credentials = new { username, password };
            string json = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Invia la richiesta POST all'API
            var response = await client.PostAsync(LoginUrl, content);

            // Se la risposta � positiva
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserializza la risposta dell'API
                var result = JsonConvert.DeserializeObject<LoginApiModel>(responseContent);

                // Salva il token per restare connesso
                //await SecureStorage.SetAsync("authToken", result.Token);

                // Salva nome utente
                await SecureStorage.SetAsync("username", username);

                // Login riuscito
                return true;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }

        // Login fallito
        return false; 
    }

    private async void OnRegisterButtonClickedAsync(object sender, EventArgs e)
    {
        // Naviga alla pagina di registrazione
        await Navigation.PushAsync(new RegisterPage());
    }
}