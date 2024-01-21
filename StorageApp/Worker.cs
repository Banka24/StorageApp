namespace StorageApp
{
    class Worker
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int NameId { get; set; }
        public int RankId { get; set; }
        public string OnWork {  get; set; }
        public virtual Rank Rank { get; set; }
        public virtual Name Name { get; set; }
    }
}
