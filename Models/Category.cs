using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaveLink.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public string DeleteCategory { get; set; }

        public DateTime AddCatOrLinkTime { get; set; }

        public ICollection<Link> Links { get; set; }
        public Category()
        {
            Links = new List<Link>();
        }
    }
}