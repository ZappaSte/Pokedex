using Pokedex.Views;

namespace Pokedex
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Imposta una schermata di caricamento come pagina iniziale
            MainPage = new NavigationPage(new PokemonList());

        }

    }
}
