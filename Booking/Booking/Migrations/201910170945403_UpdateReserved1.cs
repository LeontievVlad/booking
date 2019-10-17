namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserved1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "OwnerId", c => c.String());
            DropColumn("dbo.Reserveds", "UsersId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reserveds", "UsersId", c => c.String());
            DropColumn("dbo.Reserveds", "OwnerId");
        }
    }
}
