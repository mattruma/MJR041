using Newtonsoft.Json;
using System;

namespace ClassLibrary1
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        [JsonProperty("id")]
        public TKey Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        protected Entity()
        {
            this.CreatedOn = DateTime.UtcNow;
        }
    }
}
