using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class currentbook
    {
        public borrowBook borrowedbook { get; set; }
        public Book book { get; set; }
    }
}