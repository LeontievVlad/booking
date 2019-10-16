namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoomReserved : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reserveds",
                c => new
                    {
                        ReservedId = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        ReservedDate = c.DateTime(nullable: false),
                        ReservedTimeFrom = c.DateTime(nullable: false),
                        ReservedTimeTo = c.DateTime(nullable: false),
                        RoomId = c.Int(),
                        UsersId = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReservedId)
                .ForeignKey("dbo.Rooms", t => t.RoomId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.RoomId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        NameRoom = c.String(),
                        Date = c.DateTime(nullable: false),
                        MinTime = c.DateTime(nullable: false),
                        BusyTime = c.DateTime(nullable: false),
                        MaxTime = c.DateTime(nullable: false),
                        MaxPeople = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoomId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reserveds", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reserveds", "RoomId", "dbo.Rooms");
            DropIndex("dbo.Reserveds", new[] { "User_Id" });
            DropIndex("dbo.Reserveds", new[] { "RoomId" });
            DropTable("dbo.Rooms");
            DropTable("dbo.Reserveds");
        }
    }
}
