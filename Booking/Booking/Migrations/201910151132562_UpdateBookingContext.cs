namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBookingContext : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "RoomId", c => c.Int());
            CreateIndex("dbo.Reserveds", "RoomId");
            AddForeignKey("dbo.Reserveds", "RoomId", "dbo.Rooms", "RoomId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reserveds", "RoomId", "dbo.Rooms");
            DropIndex("dbo.Reserveds", new[] { "RoomId" });
            DropColumn("dbo.Reserveds", "RoomId");
        }
    }
}
