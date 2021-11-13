using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserPanel.Models
{
    public class User
    {
        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Histories = new List<History>();
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public List<History> Histories { get; set; }

    }
}
