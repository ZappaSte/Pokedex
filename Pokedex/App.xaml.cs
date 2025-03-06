using Pokedex.Views;
using Pokedex.Models;
using Pokedex.Services;

namespace Pokedex
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Imposta una schermata di caricamento come pagina iniziale
            //MainPage = new NavigationPage(new LoginPage());
            MainPage = new NavigationPage(new LoadingPage());

            // Controlla il token
            CheckTokenAsync();


        }

        private async void CheckTokenAsync()
        {
            try
            {
                await Task.Delay(10000);

                // Recupera il token dal SecureStorage
                var token = await SecureStorage.GetAsync("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    // Se il token non esiste, mostra la pagina di login
                    MainThread.BeginInvokeOnMainThread(() => { MainPage = new NavigationPage(new LoginPage()); });
                    
                    return;
                }

                // Verifica se il token è valido
                bool isValid = await App.GetPokemonApi().VerifyToken(token);

                if (isValid)
                {
                    // Se il token è valido, mostra la pagina di lista Pokémon
                    MainThread.BeginInvokeOnMainThread(() => { MainPage = new NavigationPage(new PokemonList()); });

                }
                else
                {
                    // Se il token non è valido, mostra la pagina di login
                    MainThread.BeginInvokeOnMainThread(() => { MainPage = new NavigationPage(new LoginPage()); });

                }
            }
            catch (Exception ex)
            {
                // In caso di errore, mostra la pagina di login
                MainThread.BeginInvokeOnMainThread(() => { MainPage = new NavigationPage(new LoginPage()); });
                Console.WriteLine($"Error checking token: {ex.Message}");
            }
        }

        public static IPokedexApi GetPokemonApi() => MauiProgram.serviceProvider.GetService<IPokedexApi>();

    }
}
