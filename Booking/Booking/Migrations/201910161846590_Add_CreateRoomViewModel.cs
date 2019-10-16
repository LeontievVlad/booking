namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CreateRoomViewModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rooms", "NameRoom", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rooms", "NameRoom", c => c.String(nullable: false));
        }
    }
}
