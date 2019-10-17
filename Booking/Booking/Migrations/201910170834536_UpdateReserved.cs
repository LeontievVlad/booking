namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserved : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reserveds", "ReservedTimeFrom", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Reserveds", "ReservedTimeTo", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reserveds", "ReservedTimeTo", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Reserveds", "ReservedTimeFrom", c => c.DateTime(nullable: false));
        }
    }
}
