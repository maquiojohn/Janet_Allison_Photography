namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Picture1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductPictures", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductPictures", "Title");
        }
    }
}
