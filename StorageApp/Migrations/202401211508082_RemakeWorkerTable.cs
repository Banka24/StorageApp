namespace StorageApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemakeWorkerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Workers", "OnWork", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Workers", "OnWork");
        }
    }
}
