namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class picture52 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductPictures", "Picture", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductPictures", "Picture", c => c.Binary(nullable: false));
        }
    }
}
