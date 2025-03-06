using System;
using Pokedex.Models;

namespace Pokedex.Services;

public interface IPokedexApi
{
    Task <bool> GetLogin(string username, string passwrod);

    Task <bool> GetRegister(string username, string passwrod);

    Task <bool> VerifyToken(string token);
    
    Task <List<PokemonModel>> GetPokemon(int offset);

    Task <byte[]> SaveAndGetImgPokemon(PokemonModel pm);

    Task <PokemonModel> GetTypesPokemon(PokemonModel pm);

    Task <List<string>> GetTypes();

    Task <PokemonModel> GetDetailsPokemon(PokemonModel pm);
    
    Task <bool> AddFavorite(PokemonModel pm);
    Task <bool> RemoveFavorite(PokemonModel pm);
    
    Task <List<int>> GetFavorites();
}
