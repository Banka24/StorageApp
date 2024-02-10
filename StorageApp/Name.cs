using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StorageApp;

internal class Name
{
    public int Id { get; set; }
    [MaxLength(25)] public string FirstName { get; set; }
    [MaxLength(25)] public string LastName { get; set; }

    public virtual ICollection<Worker> Workers { get; set; }
}