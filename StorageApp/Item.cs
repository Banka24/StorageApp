namespace StorageApp
{
    class Item
    {
        public int Id { get; set; }
        public string InventoryNumber { get; set; }
        public int CategoryId { get; set; }
        public byte StatusId { get; set; }
        public int Row { get; set; }
        public int Shelf { get; set; }
        public virtual Status Status { get; set; }
        public virtual Category Category { get; set; }
    }
}
