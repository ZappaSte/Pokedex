using Microsoft.Maui.Graphics.Text;
using Newtonsoft.Json;
using Pokedex.Models;
using Pokedex.ViewModel;
using System.Drawing;


namespace Pokedex.Views;

public partial class PokemonDetail : ContentPage
{
    public string UrlPokemon;
    public PokemonModel pokemon;

    public PokemonDetailViewModel _viewModel;

    public PokemonDetail(PokemonModel selectPokemon)
    {
        InitializeComponent();

        _viewModel = new PokemonDetailViewModel(selectPokemon);
        BindingContext = _viewModel;
    }
}