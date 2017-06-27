using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrzychodniaMedyczna.Model
{
    public class Doctor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Specialisation { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int VisitsAvailable { get; set; }
        public int VisitsTaken { get; set; }
    }
}
