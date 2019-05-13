namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchased1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PurchasedPhotoes", "Customer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurchasedPhotoes", "Customer_Id", c => c.String(nullable: false));
        }
    }
}
