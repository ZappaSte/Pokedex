using System;
using Pokedex.Models;

namespace Pokedex.Services;

public interface IPokedexApi
{
    Task <List<PokemonModel>> GetPokemon(int offset);

    Task <byte[]> SaveAndGetImgPokemon(PokemonModel pm);

    Task <PokemonModel> GetTypesPokemon(PokemonModel pm);

    Task <List<string>> GetTypes();

    Task <PokemonModel> GetDetailsPokemon(PokemonModel pm);
}
