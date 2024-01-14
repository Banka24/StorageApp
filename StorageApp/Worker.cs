using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp
{
    class Worker : Person
    {
        private string rank;
        private byte shift;
        public Worker(string firstName, string lastName, string rank, byte shift) : base(firstName, lastName)
        {
            this.rank = rank;
            this.shift = shift;
        }
    }
}
