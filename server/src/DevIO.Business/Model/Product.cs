using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DevIO.Business.Model
{
    public class Product : Entity
    {
        /* Referencia da chave estrangeira */
        public Guid ProviderId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Value { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool Active { get; set; }

        /* Relationship EF */
        public Provider Provider { get; set; }
    }
}
