using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrzychodniaMedyczna.Model
{
    public class Pharmacy : Resource
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int OpenHour { get; set; }
        public int CloseHour { get; set; }
    }
}
