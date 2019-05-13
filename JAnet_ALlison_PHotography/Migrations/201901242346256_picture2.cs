namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class picture2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductPictures", "employee_Id_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ProductPictures", new[] { "employee_Id_Id" });
            AddColumn("dbo.ProductPictures", "employee_Id", c => c.String(nullable: false));
            DropColumn("dbo.ProductPictures", "employee_Id_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductPictures", "employee_Id_Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.ProductPictures", "employee_Id");
            CreateIndex("dbo.ProductPictures", "employee_Id_Id");
            AddForeignKey("dbo.ProductPictures", "employee_Id_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
