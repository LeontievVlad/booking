namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_CreateRoomViewModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rooms", "BusyTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rooms", "BusyTime", c => c.DateTime(nullable: false));
        }
    }
}
