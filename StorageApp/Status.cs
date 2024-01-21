using System.Collections.Generic;

namespace StorageApp
{
    class Status
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
