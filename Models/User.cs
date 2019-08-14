using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaveLink.Models
{
    public class User
    {
        private readonly string RoleUser = "user";

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }   
        
        public string FirstLogining { get; set; }

        public string UserGivesLink { get; set; }

        public string Role
        {
            get
            {
                return RoleUser;
            }
            set
            {
                value = RoleUser;
            }
        }

        public string CreatingAccountForLink { get; set; }

        public ICollection<Category> Categories { get; set; }

        public User()
        {
            Categories = new List<Category>();
        }
    }
}