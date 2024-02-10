using System.ComponentModel.DataAnnotations;

namespace StorageApp;

internal class Worker
{
    public int Id { get; set; }
    [MaxLength(30)] public string Login { get; set; }
    [MaxLength(8)] public string Password { get; set; }
    public int NameId { get; set; }
    public int RankId { get; set; }
    [MaxLength(3)] public string OnWork { get; set; }
    public virtual Rank Rank { get; set; }
    public virtual Name Name { get; set; }
}