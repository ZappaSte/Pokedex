using System;
using Pokedex.Models;

namespace Pokedex.Services;

public interface IPokedexApi
{
    Task <List<PokemonModel>> GetPokemon(int offset);

    Task <string> GetImgPokemon(string namePokemon);

    Task <List<TypeModel>> GetTypesPokemon(int idPokemon);

    Task <List<PokemonDetailsModel>> GetDetailsPokemon(int idPokemon);
}
