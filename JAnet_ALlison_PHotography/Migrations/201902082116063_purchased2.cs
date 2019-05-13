namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchased2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchasedPhotoes", "Customer_Id", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchasedPhotoes", "Customer_Id");
        }
    }
}
