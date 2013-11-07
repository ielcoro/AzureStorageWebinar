using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace UserPictureStorer.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Age { get; set; }

        public Image Picture { get; set; }
    }
}