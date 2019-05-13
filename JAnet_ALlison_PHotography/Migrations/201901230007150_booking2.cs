namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class booking2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Name");
        }
    }
}
