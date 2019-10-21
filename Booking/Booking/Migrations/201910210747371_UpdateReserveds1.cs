namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserveds1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "SelectedUsersEmails", c => c.String());
            DropColumn("dbo.Reserveds", "UsersEmails");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reserveds", "UsersEmails", c => c.String());
            DropColumn("dbo.Reserveds", "SelectedUsersEmails");
        }
    }
}
