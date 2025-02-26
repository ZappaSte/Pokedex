using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Pokedex.Models;
using Pokedex.Views;
using Pokedex.Services;


namespace Pokedex.ViewModel;

public class PokemonListViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private bool _isBusy = false; //Controllo per vedere se non è gia stata effettuata la chiamata per visualizzare altri pokemon
    private bool _isFirst = true; //Controllo se è òla prima chiamata che effettuo o meno
    private int _set = 0; //Contatore che si incrementa per visualizzare i pokemon 20 alla volta

    private ObservableCollection<PokemonModel> _modelList = [];
    public ObservableCollection<PokemonModel> ModelList
    {
        get
        {
            return _modelList;
        }
        set
        {
            _modelList = value;
            OnPropertyChanged(nameof(ModelList));
        }
    }

    private ObservableCollection<PokemonModel> _modelListTemp = new();
    public ObservableCollection<PokemonModel> ModelListTemp
    {
        get
        {
            return _modelListTemp;
        }
        set
        {
            _modelListTemp = value;
            OnPropertyChanged(nameof(ModelListTemp));
        }
    }

    private List<string> _typeNames;
    public List<string> TypeNames
    {
        get
        {
            return _typeNames;
        }
        set
        {
            _typeNames = value;
            OnPropertyChanged(nameof(TypeNames));
        }
    }

    private bool _notLoad = true;
    public bool NotLoad
    {
        get { return _notLoad; }
        set
        {
            _notLoad = value;
            OnPropertyChanged(nameof(NotLoad));
        }
    }


    public PokemonListViewModel()
    {
        _ = GetPokemon();
        _ = GetTypes();
    }

    //Funzione che ritorna la lista di pokemon con l'aggiunta di alcuni dettagli
    public async Task GetPokemon()
    {
        if (!_isBusy)
        {
            _isBusy = true;

            if (!_isFirst) { _set += 20; }
            else { _isFirst = false; _set = 0; }

            try
            {
                var result = await App.GetPokemonApi().GetPokemon(_set);
                if (result != null)
                {
                    foreach (var p in result)
                    {
                        var pokemon = await App.GetPokemonApi().GetTypesPokemon(p);
                        if (pokemon.Types != null)
                        {
                            // var _imgBytes = await App.GetPokemonApi().SaveAndGetImgPokemon(p);

                            // //Percorso dei file nella cache
                            // var cacheDirectory = FileSystem.CacheDirectory;
                            // var cacheFilePath = Path.Combine(cacheDirectory, pokemon.Name + ".jpg");

                            // //Verifico se esiste o meno
                            // if (!File.Exists(cacheFilePath))
                            // {
                            //     await File.WriteAllBytesAsync(cacheFilePath, _imgBytes);
                            // }
                            // else
                            // {
                            //     _imgBytes = await File.ReadAllBytesAsync(cacheFilePath);
                            // }

                            // //Recupero l'image
                            // var stream = new MemoryStream(_imgBytes);
                            // pokemon.Img = ImageSource.FromStream(() => stream);
                            
                            // if (pokemon.Img != null)
                            ModelList.Add(pokemon);
                        }
                    }

                    NotLoad = false;
                    ModelListTemp = ModelList;
                    _isBusy = false;
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                        await Application.Current.MainPage.DisplayAlert("Errore", "Impossibile recuperare la lista dei Pok�mon", "OK")
                        );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore in GetPokemon: {ex.Message}");
            }
        }
    }

    //Funzione che ritorna la lista di tutti i tipi di pokemon e li aggiunge alla lista per il filtraggio
    public async Task GetTypes()
    {
        try
        {

            var result = await App.GetPokemonApi().GetTypes();
            if (result != null)
            {
                TypeNames = result;
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                        await Application.Current.MainPage.DisplayAlert("Errore", "Impossibile recuperare la lista dei tipi", "OK")
                        );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in GetTypes: {ex.Message}");
        }
    }

    //Funzione di ricerca in base al tipo del pokemon
    public void OptionsFilters_SelectedIndexChanged(int type)
    {
        try
        {
            int selectedOption = type;
            ModelListTemp = ModelList;

            if (selectedOption == 0)
            {
                ModelListTemp = ModelList;
                return;
            }
            else
            {
                string select = TypeNames[selectedOption];
                //Controllo la lista di Pokemon e controllo se all'interno dei tipi � presente la tipologia selezionata
                ModelListTemp = new ObservableCollection<PokemonModel>(
                    _modelList.Where(p => p.TypeList.Any(t => t.Type.Name.Equals(select, StringComparison.OrdinalIgnoreCase))).ToList());
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in OptionsFilters_SelectedIndexChanged: {ex.Message}");
        }
    }

    //Funzione di ricerca in base al nome del pokemon
    public void Search_TextChanged(string text)
    {
        ModelListTemp = ModelList;
        ModelListTemp = new ObservableCollection<PokemonModel>(
            _modelList.Where(t => t.Name.Contains(text)).ToList());
    }

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}