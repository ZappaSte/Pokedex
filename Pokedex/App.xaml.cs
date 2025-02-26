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
            // MainPage = new NavigationPage(new PokemonList());
            MainPage = new NavigationPage(new LoginPage());


        }
        public static IPokedexApi GetPokemonApi() => MauiProgram.serviceProvider.GetService<IPokedexApi>();

    }
}
