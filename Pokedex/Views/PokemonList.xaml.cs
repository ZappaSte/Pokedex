using Newtonsoft.Json;
using Pokedex.Models;
using Pokedex.ViewModel;
using System;

namespace Pokedex.Views;

public partial class PokemonList : ContentPage
{
    public PokemonListViewModel _viewModel;

    public Command DetailPokemonCommand { get; private set; }
    public Command AddPokemonCommand { get; private set; }
    public Command RemovePokemonCommand { get; private set; }
    public Command RemaingItemCommand { get; private set; }

    public PokemonList()
    {
        //Vai nella pagina di dettaglio di pokemon
        DetailPokemonCommand = new Command<PokemonModel>(async (pokemon) => await Navigation.PushAsync(new PokemonDetail(pokemon)));
        
        //Chiamata per aggiungere il pokemon ai preferiti
        AddPokemonCommand = new Command<PokemonModel>( async (pokemon) =>
        {
            if (_viewModel != null)
            {
                MessageLabel.IsVisible = true;
                _ = _viewModel.AddFavoriteUpdate(pokemon);
            }
        });
        
        //Chiamata per rimuovere il pokemon ai preferiti
        RemovePokemonCommand = new Command<PokemonModel>( async (pokemon) =>
        {
            if (_viewModel != null)
            {
                MessageLabel.IsVisible = true;
                _ = _viewModel.RemoveFavoriteUpdate(pokemon);
            }
        });

        //Chiamata per caricare i successivi 20 pokemon
        // RemaingItemCommand = new Command(async () =>
        // {
        //     if (_viewModel != null)
        //     {
        //         _ = _viewModel.GetPokemon();
        //     }
        // });

        InitializeComponent();

        _viewModel = new PokemonListViewModel();
        BindingContext = _viewModel;

        // Recupera il nome utente da un token
        var username = SecureStorage.GetAsync("username").Result;
        UsernameLabel.Text = "Hi, " + username;
        
        // Rimuove l'intera barra di navigazione
        NavigationPage.SetHasNavigationBar(this, false);

    }

    //Bottone in alto che effettua il logout e rimanda alla schermata di login
    private async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        SecureStorage.Remove("authToken");
        await Navigation.PushAsync(new LoginPage());
    }
    
    //Filtro la lista in base al tipo scelto
    private void Picker_OptionsFilters_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            int index = this.OptionsFilters.SelectedIndex;
            _viewModel.OptionsFilters_SelectedIndexChanged(index);
        }
    }

    //Filtro la lista in base al testi inserito
    private void Entry_Search_TextChanged(object sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            string text = boxSearch.Text;
            _viewModel.Search_TextChanged(text);
        }
    }
    
    //Mostro la lista dei preferiti se il checked=true
    private void CbFavorites_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        
        if (_viewModel != null)
        {
            _viewModel.CheckedChanged(cbFavorites.IsChecked);
        }
    }
}
