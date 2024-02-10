using System.ComponentModel.DataAnnotations;

namespace StorageApp;

internal abstract class SharedContext
{
    [MaxLength(25)] public static string Name { get; set; }
    public static int Role { get; set; }
}