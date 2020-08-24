using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DevIO.Business.Model
{
    public class Provider : Entity
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public TypeProvider TypeProvider { get; set; }
        public Address Address { get; set; }
        public bool Active { get; set; }

        /* Relationship EF */
        public IEnumerable<Product> Products { get; set; }
    }
}
