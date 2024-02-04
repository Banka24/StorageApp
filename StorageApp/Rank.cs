using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StorageApp
{
    internal class Rank
    {
        public int Id { get; set; }
        [MaxLength (15)]
        public string Title { get; set; }
        [MaxLength (3)]
        public string Root { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }
}
