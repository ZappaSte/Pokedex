using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Models
{
    public class PokemonModel
    {
        //Nome Pokemon
        [JsonProperty("name")]
        public string Name { get; set; }

        //Id Pokemon
        [JsonProperty("id")]
        public string Id { get; set; }

        //Url contente tutte le info del Pokemon
        [JsonProperty("url")]
        public Uri Url { get; set; }

        //Url contente l'immagine specifica
        [JsonProperty("urlImg")]
        public string UrlImg { get; set; }

        //Immagine specifica
        [JsonProperty("img")]
        public ImageSource Img { get; set; }
        
        //Una lista contentente con .Name il tipo del pokemon
        //Un Pokemon puo avere più di un tipo
        [JsonProperty("types")]
        public TypeElement[] TypeList { get; set; }
        //Concacete tutti valori di TypeList per una maggiore visualizzazione
        public string Types => TypeList != null
        ? string.Join(", ", TypeList.Select(t => t.Type.Name))
        : string.Empty;

        //Colore Tipologia del Pokemon
        [JsonProperty("typeColor")]
        public Color TypeColor { get; set; }

        //Altezza del Pokemon
        [JsonProperty("height")]
        public string Height { get; set; }

        //Peso del Pokemon
        [JsonProperty("weight")]
        public string Weight { get; set; }

        //Abilità del Pokemon
        [JsonProperty("abilities")]
        public Ability[] AbilitieList { get; set; }
        public string Abilities => AbilitieList != null
        ? string.Join(", ", AbilitieList.Select(a => a.AbilityAbility.Name))
        : string.Empty;

        //Statistiche del Pokemon, ogni pokemon ha una statistica ed essa contiene una lista di valori
        [JsonProperty("stats")]
        public Stat[] Stats { get; set; }
        public string StatsList => Stats != null
        ? string.Join(", ", Stats.Select(s => $"{s.StatStat.Name}: {s.BaseStat}"))
        : string.Empty;

        //Mosse che il pokemon può apprendere
        [JsonProperty("moves")]
        public Move[] MoveList { get; set; }
        public string Moves => MoveList != null
        ? string.Join(", ", MoveList.Select(m => m.MoveMove.Name))
        : string.Empty;

        public static implicit operator List<object>(PokemonModel v)
        {
            throw new NotImplementedException();
        }
    }
}
