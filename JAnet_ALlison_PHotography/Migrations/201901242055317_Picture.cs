namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Picture : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductPictures",
                c => new
                    {
                        Picture_Id = c.Int(nullable: false, identity: true),
                        Picture = c.Binary(),
                        Price = c.Double(nullable: false),
                        Description = c.String(nullable: false),
                        employee_Id_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Picture_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.employee_Id_Id, cascadeDelete: true)
                .Index(t => t.employee_Id_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductPictures", "employee_Id_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProductPictures", new[] { "employee_Id_Id" });
            DropTable("dbo.ProductPictures");
        }
    }
}
