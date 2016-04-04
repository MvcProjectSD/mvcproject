using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class logg
    {
        public User user { get; set; }
        public Login login { get; set; }

        public ImageToUpload image { get; set; }
    }
}