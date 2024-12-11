using Newtonsoft.Json;
using Pokedex.Models;
using System;

namespace Pokedex.Views;

public partial class PokemonList : ContentPage
{
    public string Url;
    public string UrlTypes;
    public string Next;
    public string Previous;

    private bool _isPageVisible;
    private bool _isNavigatingAway;

    private List<PokemonModel> _modelList;

    public PokemonList()
    {
        InitializeComponent();

        // Rimuove l'intera barra di navigazione
        NavigationPage.SetHasNavigationBar(this, false);

        // Recupera il nome utente da un token
        var username = SecureStorage.GetAsync("username").Result;
        UsernameLabel.Text = "Hi, " + username;

        //Url per risalire a tutti i pokemon
        Url = "https://pokeapi.co/api/v2/pokemon/";

        //Url per risalire a tutti i tipi di pokemon
        UrlTypes = "https://pokeapi.co/api/v2/type";

        _ = GetPokemon(Url);
        _ = GetTypes(UrlTypes);
    }


    //Funzione che ritorna la lista di pokemon con l'aggiunta di tutti i dettagli
    public async Task GetPokemon(string Url)
    {
        try
        {
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(Url);

            if (response.IsSuccessStatusCode)
            {
                var respString = await response.Content.ReadAsStringAsync();
                var json_s = JsonConvert.DeserializeObject<PokemonApiModel>(respString);

                _modelList = new List<PokemonModel>();

                foreach (var poke in json_s.Results)
                {
                    PokemonModel pm = new PokemonModel();
                    pm.Name = poke.Name;
                    string[] parts = poke.Url.ToString().Split('/');
                    pm.Id = parts[parts.Length - 2];
                    pm.Url = poke.Url;
                    pm.UrlImg = "https://img.pokemondb.net/artwork/" + poke.Name + ".jpg";
                    await GetPokemonType(Url, pm.Id, pm);

                    _modelList.Add(pm);
                }

                ListPokemon.ItemsSource = _modelList;
            }
            else
            {
                await DisplayAlert("Errore", "Impossibile recuperare la lista dei Pokémon", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in GetPokemon: {ex.Message}");
        }
    }

    //Funzione che ritorna la lista di tutti i tipi di pokemon e li aggiunge alla lista per il filtraggio
    public async Task GetTypes(string Url)
    {
        try
        {
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(Url);

            if (response.IsSuccessStatusCode)
            {
                var respString = await response.Content.ReadAsStringAsync();
                var json_s = JsonConvert.DeserializeObject<TypeApiModel>(respString);

                List<string> typeNames = new List<string> { "All" };

                foreach (var type in json_s.Results)
                {
                    typeNames.Add(type.Name);
                }

                OptionsFilters.ItemsSource = typeNames;
            }
            else
            {
                await DisplayAlert("Errore", "Impossibile recuperare la lista dei tipi", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in GetTypes: {ex.Message}");
        }
    }

    //Funzione che aggiunge la tipologia al Pokemon
    public async Task GetPokemonType(string Url, string idPokemon, PokemonModel pm)
    {
        try
        {
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(Url + idPokemon + "/");

            if (response.IsSuccessStatusCode)
            {
                var respStringPokemon = await response.Content.ReadAsStringAsync();
                var jsonPokemon = JsonConvert.DeserializeObject<PokemonDetailsModel>(respStringPokemon);

                pm.TypeList = jsonPokemon.Types;
            }
            else
            {
                await DisplayAlert("Errore", "Impossibile recuperare i tipi del Pokémon", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in GetPokemonType: {ex.Message}");
        }
    }

    //In base a cosa viene scelto nel filtraggio
    private void OptionsFilters_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (OptionsFilters.SelectedIndex != -1 && !_isNavigatingAway)
            {
                string selectedOption = OptionsFilters.SelectedItem?.ToString() ?? "";

                if (selectedOption == "All")
                {
                    ListPokemon.ItemsSource = _modelList;
                    return;
                }
                else
                {
                    string selectedType = selectedOption.ToLower();

                    //Controllo la lista di Pokemon e controllo se all'interno dei tipi è presente la tipologia selezionata
                    ListPokemon.ItemsSource =
                        _modelList.Where(p => p.TypeList.Any(t => t.Type.Name.Equals(selectedType, StringComparison.OrdinalIgnoreCase))).ToList();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in OptionsFilters_SelectedIndexChanged: {ex.Message}");

        }
    }

    //Cliccando sul pokemon della lista si va nella pagina con i dettagli
    private void ListPokemon_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is PokemonModel selectPokemon)
        {
            Navigation.PushAsync(new PokemonDetail(selectPokemon));
        }

        ((ListView)sender).SelectedItem = null;
    }

    //Bottone in alto che effettua il logout e rimanda alla schermata di login
    private async void OnLogoutButtonClicked(object sender, EventArgs e)
    {
        SecureStorage.Remove("authToken");
        await Navigation.PushAsync(new LoginPage());
    }

    //Funzione che modifica l'aggiunta o meno del Pokemon nei preferiti
    private void imgStar_Clicked(object sender, EventArgs e)
    {
        // Trova l'immagine che ha generato l'evento
        if (sender is ImageButton clickedButton)
        {
            // Trova il layout genitore per ottenere altri elementi
            var parentGrid = (Grid)clickedButton.Parent;

            // Trova imgStar e imgStarYes all'interno del genitore
            var imgStar = parentGrid.Children.FirstOrDefault(x => x is ImageButton btn && btn.Source.ToString().Contains("star.png")) as ImageButton;
            var imgStarYes = parentGrid.Children.FirstOrDefault(x => x is ImageButton btn && btn.Source.ToString().Contains("star_yes.png")) as ImageButton;

            if (imgStar != null && imgStarYes != null)
            {
                if (imgStar.IsVisible)
                {
                    imgStar.IsVisible = false;
                    imgStarYes.IsVisible = true;
                }
                else
                {
                    imgStar.IsVisible = true;
                    imgStarYes.IsVisible = false;
                }
            }
        }
    }

    //Funzione di ricerca in base al nome del pokemon
    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Devono essere inseriti almeno 3 caratteri per effettuare la ricerca automatica
        if (boxSearch.Text.Length < 3)
        {
            ListPokemon.ItemsSource = _modelList;
        }
        else
        {
            string temp = boxSearch.Text;
            ListPokemon.ItemsSource = _modelList.Where(t => t.Name.Contains(temp)).ToList();
        }
    }
}
