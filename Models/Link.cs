using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaveLink.Models
{
    public class Link
    {
        //private readonly DateTime add_link_time = DateTime.Now;

        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        //статус розшаренная сылка или нет
        public string ShareLink { get; set; }

        //public DateTime AddLinkTime {
        //    get
        //    {
        //        return add_link_time;
        //    }
        //    set
        //    {
        //        value = add_link_time;
        //    }
        //}


        //связь многие ко многим с таблицей Shared
        public virtual ICollection<Shared> Shareds { get; set; }
        public Link()
        {
            Shareds = new List<Shared>();
        }
    }
}