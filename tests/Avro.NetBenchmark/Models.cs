using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNETBenchmark
{
    public class Contact
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public long HouseNumber { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }

    public class Offering
    {
        public int Id { get; set; }
        public Guid ProductNumber { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public bool Discount { get; set; }
    }

    public class Pet
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Contact Contact { get; set; }
        public List<Offering> Offerings { get; set; }
    }
}
