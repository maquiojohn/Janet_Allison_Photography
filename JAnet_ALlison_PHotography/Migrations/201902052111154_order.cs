namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OrderDetails", "Receipt_Id");
            AddForeignKey("dbo.OrderDetails", "Receipt_Id", "dbo.Receipts", "Receipt_Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "Receipt_Id", "dbo.Receipts");
            DropIndex("dbo.OrderDetails", new[] { "Receipt_Id" });
        }
    }
}
