using Newtonsoft.Json;
using Pokedex.Models;
using Pokedex.ViewModel;
using System;

namespace Pokedex.Views;

public partial class PokemonList : ContentPage
{
    public PokemonListViewModel _viewModel;

    public Command DetailPokemonCommand { get; private set; }
    public Command RemaingItemCommand { get; private set; }

    public PokemonList()
    {
        //Vai nella pagina di dettaglio di pokemon
        DetailPokemonCommand = new Command<PokemonModel>(async (pokemon) => await Navigation.PushAsync(new PokemonDetail(pokemon)));

        //Chiamata per caricare i successivi 20 pokemon
        RemaingItemCommand = new Command(async () =>
        {
            _ = _viewModel.GetPokemon();
        });

        InitializeComponent();

        _viewModel = new PokemonListViewModel();
        BindingContext = _viewModel;

        // Rimuove l'intera barra di navigazione
        NavigationPage.SetHasNavigationBar(this, false);

    }

    private void Picker_OptionsFilters_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            int index = this.OptionsFilters.SelectedIndex;
            _viewModel.OptionsFilters_SelectedIndexChanged(index);
        }
    }

    private void Entry_Search_TextChanged(object sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            string text = boxSearch.Text;
            _viewModel.Search_TextChanged(text);
        }
    }

    //Funzione che modifica l'aggiunta o meno del Pokemon nei preferiti
    private void imgStar_Clicked(object sender, EventArgs e)
    {
        // // Trova l'immagine che ha generato l'evento
        // if (sender is ImageButton clickedButton)
        // {
        //     // Trova il layout genitore per ottenere altri elementi
        //     var parentGrid = (Grid)clickedButton.Parent;

        //     // Trova imgStar e imgStarYes all'interno del genitore
        //     var imgStar = parentGrid.Children.FirstOrDefault(x => x is ImageButton btn && btn.Source.ToString().Contains("star.png")) as ImageButton;
        //     var imgStarYes = parentGrid.Children.FirstOrDefault(x => x is ImageButton btn && btn.Source.ToString().Contains("star_yes.png")) as ImageButton;

        //     if (imgStar != null && imgStarYes != null)
        //     {
        //         if (imgStar.IsVisible)
        //         {
        //             imgStar.IsVisible = false;
        //             imgStarYes.IsVisible = true;
        //         }
        //         else
        //         {
        //             imgStar.IsVisible = true;
        //             imgStarYes.IsVisible = false;
        //         }
        //     }
        // }
    }
}
