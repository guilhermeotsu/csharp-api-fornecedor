using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Reflection;
using System.Text;

namespace DevIO.Business.Model
{
    public class Address : Entity
    {
        /* Chave estrangeira do fornecedor */
        public Guid ProviderId { get; set; }

        public string Logradouro { get; set; }
        public string Number { get; set; }
        public string  Complement { get; set; }
        public string Cep { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        /* Relationship EF (Propriedade de navegacao) */
        public Provider Provider { get; set; }
    }
}
