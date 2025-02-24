using System;
using Pokedex.Models;
using Newtonsoft.Json;


namespace Pokedex.Services;

public class PokedexApi : IPokedexApi
{
    public string Url = "https://pokeapi.co/api/v2/pokemon/?limit=20&offset=";

    public string UrlPokemon = "https://pokeapi.co/api/v2/pokemon/";

    public string UrlTypes = "https://pokeapi.co/api/v2/type";

    public string UrlImg = "https://img.pokemondb.net/artwork/";
    
    HttpClient http = new HttpClient();

    //Chiamata Api che ritorna la lista dei pokemon 
    public async Task<List<PokemonModel>> GetPokemon(int offset)
    {
        var response = await http.GetAsync(Url + offset.ToString());
        List<PokemonModel> _listPokemon = new List<PokemonModel>();

        if (response.IsSuccessStatusCode)
        {
            var respString = await response.Content.ReadAsStringAsync();
            var json_s = JsonConvert.DeserializeObject<PokemonApiModel>(respString);

            foreach (var poke in json_s.Results)
            {
                PokemonModel pm = new PokemonModel();
                string[] parts = poke.Url.ToString().Split('/');
                pm.Id = parts[parts.Length - 2];

                pm.Name = poke.Name;
                pm.UrlImg = UrlImg + pm.Name + ".jpg";
                _listPokemon.Add(pm);
            }
        }

        return _listPokemon;
    }

    //Chiamata Api che assegna la foto NON UTILIZZATA
    public async Task<string> GetImgPokemon(string namePokemon)
    {
        string UrlImg = "https://img.pokemondb.net/artwork/" + namePokemon + ".jpg";
        return UrlImg;
    }

    //Chiamata Api che assegna la tipologia al pokemon passato
    public async Task<PokemonModel> GetTypesPokemon(PokemonModel pm)
    {
        HttpClient http = new HttpClient();
        var response = await http.GetAsync(UrlPokemon + pm.Id + "/");

        if (response.IsSuccessStatusCode)
        {
            var respStringPokemon = await response.Content.ReadAsStringAsync();
            var jsonPokemon = JsonConvert.DeserializeObject<PokemonDetailsModel>(respStringPokemon);

            pm.TypeList = jsonPokemon.Types;
        }

        return pm;
    }

    //Chiamta API che ritorna il nome di tutte le tipologie esistenti
    public async Task<List<string>> GetTypes()
    {
        var response = await http.GetAsync(UrlTypes);
        List<string> _listTypes = new List<string> { "All" };

        if (response.IsSuccessStatusCode)
        {
            var respString = await response.Content.ReadAsStringAsync();
            var json_s = JsonConvert.DeserializeObject<TypeApiModel>(respString);

            foreach (var type in json_s.Results)
            {
                _listTypes.Add(type.Name);
            }
        }

        return _listTypes;
    }

    //Chiamata Api che assegna i dettagli al pokemon passato
    public async Task<PokemonModel> GetDetailsPokemon(PokemonModel p)
    {
		var response = await http.GetAsync(UrlPokemon + p.Id+ "/");

        if (response.IsSuccessStatusCode)
        {
            var respStringPokemon = await response.Content.ReadAsStringAsync();
			var jsonPokemon = JsonConvert.DeserializeObject<PokemonDetailsModel>(respStringPokemon);

			p.Height = jsonPokemon.Height.ToString();
			p.Weight = jsonPokemon.Weight.ToString();
			p.AbilitieList = jsonPokemon.Abilities;
			p.Stats = jsonPokemon.Stats;
			p.MoveList = jsonPokemon.Moves;
        }

        return p;
    }

}