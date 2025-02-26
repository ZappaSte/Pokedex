using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Models
{
    public partial class LoginApiModel
    {
        //Token legato ad ogni login
        [JsonProperty("access_token")]
        public string Token { get; set; }

        //Classe Utente composta da id e Username
        [JsonProperty("user")]
        public User User { get; set; }
    }

    public class User
    {
        //Id utente
        [JsonProperty("id")]
        public int Id { get; set; }

        //Username utente
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
