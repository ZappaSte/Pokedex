using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Pokedex.Models;

namespace Pokedex.ViewModel;

public class PokemonDetailViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	private PokemonModel _model;
	public PokemonModel Model
	{
		get
		{
			return _model;
		}
		set
		{
			_model = value;
			OnPropertyChanged(nameof(Model));
		}
	}
	
	public PokemonDetailViewModel(PokemonModel selectPokemon)
	{
		_ = GetPokemonDetails(selectPokemon);
	}

    //Funzione che ritorna la lista di pokemon con l'aggiunta di tutti i dettagli
	public async Task GetPokemonDetails(PokemonModel selectPokemon)
	{
		try
        {
            var result = await App.GetPokemonApi().GetDetailsPokemon(selectPokemon);
            if (result != null)
            {
				SetTypeColor(selectPokemon);
                Model = result;
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                        await Application.Current.MainPage.DisplayAlert("Errore", "Impossibile recuperare i dettagli del pokemom", "OK")
                        );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in GetDetailsPokemon: {ex.Message}");
        }
	}

	//Funzione che assegna un colore in base al Type del Pokemon
	private void SetTypeColor(PokemonModel selectPokemon)
	{
		switch (selectPokemon.TypeList[0].Type.Name)
		{
			case "normal":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Gray;
				break;
			case "fighting":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Gold;
				break;
			case "flying":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.SkyBlue;
				break;
			case "poison":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Violet;
				break;
			case "ground":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Brown;
				break;
			case "rock":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Silver;
				break;
			case "bug":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Green;
				break;
			case "ghost":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.DarkBlue;
				break;
			case "steel":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.SteelBlue;
				break;
			case "fire":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Red;
				break;
			case "water":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Blue;
				break;
			case "grass":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.ForestGreen;
				break;
			case "electric":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Yellow;
				break;
			case "psychic":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.DeepPink;
				break;
			case "ice":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.AliceBlue;
				break;
			case "dragon":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.OrangeRed;
				break;
			case "dark":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Black;
				break;
			case "fairy":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Pink;
				break;
			case "stellar":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.Gold;
				break;
			case "unknown":
				selectPokemon.TypeColor = Microsoft.Maui.Graphics.Colors.WhiteSmoke;
				break;
		}
	}

	public void OnPropertyChanged([CallerMemberName] string name = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}