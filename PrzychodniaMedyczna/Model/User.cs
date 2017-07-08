using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrzychodniaMedyczna.Model
{
    public class User : Resource
    {
        public string Login { get; set; }
        public string Passw { get; set; }
        public bool IsAdmin { get; set; }
        public int Wallet { get; set; }
        public List<UserVisit> Visits { get; set; }
    }
}
