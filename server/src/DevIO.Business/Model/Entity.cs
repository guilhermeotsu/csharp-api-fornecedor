using System;
using System.Net.Cache;

namespace DevIO.Business.Model
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
