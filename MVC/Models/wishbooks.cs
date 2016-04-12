using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class wishbooks
    {
        public Book book { get; set; }
        public WishList wishlist { get; set; }
    }
}