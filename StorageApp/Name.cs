using System.Collections.Generic;

namespace StorageApp
{
    class Name
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }
}
