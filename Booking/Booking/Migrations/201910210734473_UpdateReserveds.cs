namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserveds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "UsersEmails", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reserveds", "UsersEmails");
        }
    }
}
