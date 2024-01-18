using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp
{
    abstract class SharedContext
    {
        public static string Name { get; set; }
        public static int Role { get; set; }
    }
}
