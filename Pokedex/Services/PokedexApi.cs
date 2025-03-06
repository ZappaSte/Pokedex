using System;
using Pokedex.Models;
using Newtonsoft.Json;
using System.Runtime.InteropServices.Marshalling;
using System.Text;


namespace Pokedex.Services;


public class PokedexApi : IPokedexApi
{
    public string Url = "https://pokeapi.co/api/v2/pokemon/?limit=151&offset=0";

    public string UrlPokemon = "https://pokeapi.co/api/v2/pokemon/";

    public string UrlTypes = "https://pokeapi.co/api/v2/type";

    public string UrlImg = "https://img.pokemondb.net/artwork/";

    public string UrlDb = "http://10.105.200.172:80/";

    HttpClient http = new HttpClient();
    private byte[]? _imgBytes;

    //Chiamata API che aggiunge il pokemon dai preferiti
    public async Task<bool> AddFavorite(PokemonModel pokemon)
    {
        // Crea un oggetto con le credenziali e lo serializza in JSON
        var data = new { pokemon_id = pokemon.Id};
        string json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await SecureStorage.GetAsync("authToken");
        http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        // Invia la richiesta POST all'API
        var response = await http.PostAsync(UrlDb + "/add-favorite", content);

        // Se la risposta � positiva
        if (response.IsSuccessStatusCode)
        {
            // Aggiunto ai preferiti
            return true;
        }

        // // Aggiunto ai preferiti fallito
        return false;
    }
    
    //Chiamata API che rimuove il pokemon dai preferiti
    public async Task<bool> RemoveFavorite(PokemonModel pokemon)
    {
        // Crea un oggetto con le credenziali e lo serializza in JSON
        var data = new { pokemon_id = pokemon.Id};
        string json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var token = await SecureStorage.GetAsync("authToken");
        http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        // Invia la richiesta POST all'API
        var response = await http.PostAsync(UrlDb + "/remove-favorite", content);

        // Se la risposta � positiva
        if (response.IsSuccessStatusCode)
        {
            // Rimosso dai preferiti
            return true;
        }

        // // Rimosso dai preferiti fallito
        return false;
    }

    //Chiamata API che rtorna id dei pokemon preferiti
    public async Task<List<int>> GetFavorites()
    {
        var token = await SecureStorage.GetAsync("authToken");
        http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await http.GetAsync(UrlDb + "/favorites");

        // Se la risposta � positiva
        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<List<int>>(responseContent);
        }
        
        return new List<int>();
    }
    
    //Chiamata Api che ritorna se login è avvenuto con successo o meno
    public async Task<bool> GetLogin(string username, string password)
    {
        // Crea un oggetto con le credenziali e lo serializza in JSON
        var credentials = new { username, password };
        string json = JsonConvert.SerializeObject(credentials);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Invia la richiesta POST all'API
        var response = await http.PostAsync(UrlDb + "login", content);

        // Se la risposta � positiva
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserializza la risposta dell'API
            var result = JsonConvert.DeserializeObject<LoginApiModel>(responseContent);

            // Salva il token per restare connesso
            await SecureStorage.SetAsync("authToken", result.Token);

            // Salva nome utente
            await SecureStorage.SetAsync("username", username);

            // Login riuscito
            return true;
        }

        // Login fallito
        return false;
    }

    //Chiamata Api che ritorna se la registrazione è avvenuto con successo o meno
    public async Task<bool> GetRegister(string username, string password)
    {
        var credentials = new { username, password };
        string json = JsonConvert.SerializeObject(credentials);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await http.PostAsync(UrlDb + "register", content);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Chiamata Api che ritorna se il token è valido o meno
    public async Task<bool> VerifyToken(string token)
    {
        http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await http.GetAsync(UrlDb + "verify-token");

        // Considera valido il token se il server restituisce 200 OK
        return response.IsSuccessStatusCode;
    }

    //Chiamata Api che ritorna la lista dei pokemon 
    public async Task<List<PokemonModel>> GetPokemon(int offset)
    {
        var response = await http.GetAsync(Url);
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

    //Chiamata Api che assegna la foto
    public async Task<byte[]> SaveAndGetImgPokemon(PokemonModel pm)
    {
        return _imgBytes = await http.GetByteArrayAsync(UrlImg + pm.Name + ".jpg");
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
        var response = await http.GetAsync(UrlPokemon + p.Id + "/");

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