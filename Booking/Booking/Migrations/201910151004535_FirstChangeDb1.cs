namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstChangeDb1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "EventName", c => c.String());
            AddColumn("dbo.Rooms", "NameRoom", c => c.String());
            DropColumn("dbo.Rooms", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rooms", "Name", c => c.String());
            DropColumn("dbo.Rooms", "NameRoom");
            DropColumn("dbo.Reserveds", "EventName");
        }
    }
}
