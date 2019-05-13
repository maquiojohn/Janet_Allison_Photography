namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zipfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ZipFiles",
                c => new
                    {
                        PurchasedPic_Id = c.Int(nullable: false, identity: true),
                        Customer_Id = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Picture = c.Binary(),
                        UploadDate = c.DateTime(nullable: false),
                        DateTaken = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchasedPic_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ZipFiles");
        }
    }
}
