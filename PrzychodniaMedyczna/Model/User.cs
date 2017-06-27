using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrzychodniaMedyczna.Model
{
    class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Passw { get; set; }
        public string Type { get; set; }
        public int Wallet { get; set; }
    }
}
