namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRoom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "Name", c => c.String());
            AddColumn("dbo.Rooms", "BusyTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rooms", "MinTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rooms", "MaxTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Rooms", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rooms", "Time", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Rooms", "MaxTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Rooms", "MinTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Rooms", "BusyTime");
            DropColumn("dbo.Rooms", "Name");
        }
    }
}
