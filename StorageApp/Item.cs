using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp
{
    public struct Location
    {
        public short row;
        public short shelf;
    }
    class Item
    {
        protected string category;
        protected string status;
        protected string inventory_id;
        protected Location Location;

        public Item(string category, string status, string inventory_id, short row, short shelf)
        {
            this.category = category;
            this.inventory_id = inventory_id;
            this.status = status;
            Location.row = row;
            Location.shelf = shelf;
        }
    }
}
