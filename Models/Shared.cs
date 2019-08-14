using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaveLink.Models
{
    public class Shared
    {
        public int Id { get; set; }
        //отправитель
        public string Sender { get; set; }
        //получатель
        public string Recipient { get; set; }

        public DateTime DateTime { get; set; }

        public string Description { get; set; }

        public string DelChatForOne { get; set; }

        public string DelChatForTwo { get; set; }
        //статус прочитано ли уведомлени
        public string ReadStatus { get; set; }
        //последние новые перессылки
        public string NewShared { get; set; }

        //связь многие ко многим с таблицей LInk
        public virtual ICollection<Link> Links { get; set; }
        public Shared()
        {
            Links = new List<Link>();
        }
    }
}