using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrzychodniaMedyczna.Model
{
    public class Advice : Resource
    {
        public string[] Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Other { get; set; }
    }
}
