using Microsoft.Maui.Graphics.Text;
using Newtonsoft.Json;
using Pokedex.Models;
using System.Drawing;


namespace Pokedex.Views;

public partial class PokemonDetail : ContentPage
{
    public string UrlPokemon;
    public PokemonModel pokemon;

    public PokemonDetail(PokemonModel selectPokemon)
    {
        InitializeComponent();

        pokemon = selectPokemon;

        UrlPokemon = "https://pokeapi.co/api/v2/pokemon/" + selectPokemon.Id + "/";

        _ = InitializePokemonDetails(selectPokemon);

    }

    private async Task InitializePokemonDetails(PokemonModel selectPokemon)
    {
        await GetPokemonDetails(UrlPokemon, selectPokemon);

        // Imposta il BindingContext solo dopo aver recuperato i dettagli
        BindingContext = selectPokemon;
    }

    public async Task GetPokemonDetails(string UrlPokemon, PokemonModel selectPokemon)
    {
        HttpClient http = new HttpClient();
        var response = await http.GetAsync(UrlPokemon);

        if (response.IsSuccessStatusCode)
        {
            var respStringPokemon = await response.Content.ReadAsStringAsync();
            var jsonPokemon = JsonConvert.DeserializeObject<PokemonDetailsModel>(respStringPokemon);

            //selectPokemon.TypeList = jsonPokemon.Types;
            selectPokemon.Height = jsonPokemon.Height.ToString();
            selectPokemon.Weight = jsonPokemon.Weight.ToString();
            selectPokemon.AbilitieList = jsonPokemon.Abilities;
            selectPokemon.Stats = jsonPokemon.Stats;
            selectPokemon.MoveList = jsonPokemon.Moves;
        }
        else
        {
            await DisplayAlert("Errore", "Impossibile recuperare i dettagli del Pokémon", "OK");
        }
    }

    //Cambia lo sfondo dell'immagine in base al tipo del Pokemon
    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        switch (pokemon.TypeList[0].Type.Name)
        {
            case "normal":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Gray;
                break;
            case "fighting":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Gold;
                break;
            case "flying":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.SkyBlue;
                break;
            case "poison":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Violet;
                break;
            case "ground":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Brown;
                break;
            case "rock":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Silver;
                break;
            case "bug":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Green;
                break;
            case "ghost":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.DarkBlue;
                break;
            case "steel":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.SteelBlue;
                break;
            case "fire":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Red;
                break;
            case "water":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Blue;
                break;
            case "grass":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.ForestGreen;
                break;
            case "electric":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Yellow;
                break;
            case "psychic":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.DeepPink;
                break;
            case "ice":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.AliceBlue;
                break;
            case "dragon":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.OrangeRed;
                break;
            case "dark":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Black;
                break;
            case "fairy":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Pink;
                break;
            case "stellar":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.Gold;
                break;
            case "unknown":
                Sfumatura.Color = Microsoft.Maui.Graphics.Colors.WhiteSmoke;
                break;
        }
    }
}