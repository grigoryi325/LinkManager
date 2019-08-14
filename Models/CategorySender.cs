using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaveLink.Models
{
    public class CategorySender
    {
        public string Sender { get; set; }

        public CategorySender(string sender)
        {
            Sender = sender;
        }
    }
}