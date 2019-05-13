namespace JAnet_ALlison_PHotography.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class booking1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "PhotoType", c => c.String(nullable: false));
            AddColumn("dbo.Bookings", "NumPeople", c => c.Int(nullable: false));
            DropColumn("dbo.Bookings", "TimeSlot");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "TimeSlot", c => c.Int(nullable: false));
            DropColumn("dbo.Bookings", "NumPeople");
            DropColumn("dbo.Bookings", "PhotoType");
        }
    }
}
