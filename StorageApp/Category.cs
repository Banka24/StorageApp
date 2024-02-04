using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StorageApp
{
    internal class Category
    {
        public int Id { get; set; }

        [MaxLength (50)]
        public string Name { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
