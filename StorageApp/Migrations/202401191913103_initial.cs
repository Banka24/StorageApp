namespace StorageApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryNumber = c.String(),
                        CategoryId = c.Int(nullable: false),
                        StatusId = c.Byte(nullable: false),
                        Row = c.Int(nullable: false),
                        Shelf = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Names",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        NameId = c.Int(nullable: false),
                        RankId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Names", t => t.NameId, cascadeDelete: true)
                .ForeignKey("dbo.Ranks", t => t.RankId, cascadeDelete: true)
                .Index(t => t.NameId)
                .Index(t => t.RankId);
            
            CreateTable(
                "dbo.Ranks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Root = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Workers", "RankId", "dbo.Ranks");
            DropForeignKey("dbo.Workers", "NameId", "dbo.Names");
            DropForeignKey("dbo.Items", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Items", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Workers", new[] { "RankId" });
            DropIndex("dbo.Workers", new[] { "NameId" });
            DropIndex("dbo.Items", new[] { "StatusId" });
            DropIndex("dbo.Items", new[] { "CategoryId" });
            DropTable("dbo.Ranks");
            DropTable("dbo.Workers");
            DropTable("dbo.Names");
            DropTable("dbo.Status");
            DropTable("dbo.Items");
            DropTable("dbo.Categories");
        }
    }
}
