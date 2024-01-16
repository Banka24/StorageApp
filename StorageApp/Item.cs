using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp
{
    class Item
    {
        public int Id { get; set; }
        public string InventoryNumber { get; set; }
        public string Category { get; set; }
        public byte StatusId { get; set; }
        public int Row { get; set; }
        public int Shelf { get; set; }

        public virtual Status Status { get; set; }
    }
}
