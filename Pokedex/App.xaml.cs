using Pokedex.Views;

namespace Pokedex
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PokemonList());
        }
    }
}
