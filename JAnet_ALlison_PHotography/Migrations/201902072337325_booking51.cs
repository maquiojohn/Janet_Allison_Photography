namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class booking51 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Medium", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Medium");
        }
    }
}
