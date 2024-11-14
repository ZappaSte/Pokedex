using Newtonsoft.Json;
using Pokedex.Models;

namespace Pokedex.Views;

public partial class PokemonList : ContentPage
{
    public string Url;
    public string Next;
    public string Previous;

    public PokemonList()
	{
		InitializeComponent();
        Url = "https://pokeapi.co/api/v2/pokemon/";

        _ = GetPokemon(Url);

    }

    public async Task GetPokemon(string Url)
    {
        HttpClient http = new HttpClient();
        var response = await http.GetAsync(Url);

        if (response.IsSuccessStatusCode)
        {
            var respString = await response.Content.ReadAsStringAsync();
            var json_s = JsonConvert.DeserializeObject<PokemonApiModel>(respString);

            List<PokemonListModel> pokemonLists = new List<PokemonListModel>();

            foreach (var poke in json_s.Results)
            {
                PokemonListModel pl = new PokemonListModel();
                pl.Name = poke.Name;
                pl.Url = poke.Url;
                pl.UrlImg = "https://img.pokemondb.net/artwork/" + poke.Name + ".jpg";

                pokemonLists.Add(pl);
            }

            ListPokemon.ItemsSource = pokemonLists;
        }
        else
        {

        }
    }

}