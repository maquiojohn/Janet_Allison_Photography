namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class picture51 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductPictures", "Picture", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductPictures", "Picture", c => c.Binary());
        }
    }
}
