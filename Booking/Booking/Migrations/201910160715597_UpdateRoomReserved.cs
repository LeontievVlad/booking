namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRoomReserved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "ReservedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reserveds", "ReservedTimeFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reserveds", "ReservedTimeTo", c => c.DateTime(nullable: false));
            AddColumn("dbo.Rooms", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Reserveds", "ReservedTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reserveds", "ReservedTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Rooms", "Date");
            DropColumn("dbo.Reserveds", "ReservedTimeTo");
            DropColumn("dbo.Reserveds", "ReservedTimeFrom");
            DropColumn("dbo.Reserveds", "ReservedDate");
        }
    }
}
