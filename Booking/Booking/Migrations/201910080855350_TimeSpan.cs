namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeSpan : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rooms", "MinTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Rooms", "Time", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Rooms", "MaxTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rooms", "MaxTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rooms", "Time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rooms", "MinTime", c => c.DateTime(nullable: false));
        }
    }
}
