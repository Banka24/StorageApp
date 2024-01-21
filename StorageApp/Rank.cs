using System.Collections.Generic;

namespace StorageApp
{
    class Rank
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Root { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }
}
