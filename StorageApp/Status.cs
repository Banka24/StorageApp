using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StorageApp;

internal class Status
{
    public byte Id { get; set; }
    [MaxLength(25)] public string Name { get; set; }
    public virtual ICollection<Item> Items { get; set; }
}