using System;
using Pokedex.Models;
using Newtonsoft.Json;


namespace Pokedex.Services;

public class PokedexApi : IPokedexApi
{
    HttpClient http = new HttpClient();

    public async Task<List<PokemonModel>> GetPokemon(int offset)
    {
        string Url = "https://pokeapi.co/api/v2/pokemon/?limit=20&offset=";
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

                _listPokemon.Add(pm);
            }
        }                

        return _listPokemon;
    }

    public async Task<string> GetImgPokemon(string namePokemon)
    {
        string UrlImg = "https://img.pokemondb.net/artwork/" + namePokemon + ".jpg";
        return UrlImg;
    }

    public async Task<List<TypeModel>> GetTypesPokemon(int idPokemon)
    {
        string UrlTypes = "https://pokeapi.co/api/v2/type";

        return new List<TypeModel>();
    }

    public async Task<List<PokemonDetailsModel>> GetDetailsPokemon(int idPokemon)
    {
        string UrlPokemon = "https://pokeapi.co/api/v2/pokemon/";

        return new List<PokemonDetailsModel>();
    }

}