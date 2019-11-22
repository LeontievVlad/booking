namespace Booking.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateReserved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "Description", c => c.String());
            AddColumn("dbo.Reserveds", "AcceptedEmails", c => c.String());
            AddColumn("dbo.Reserveds", "DeniedEmails", c => c.String());
            AddColumn("dbo.Reserveds", "IsPrivate", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Reserveds", "IsPrivate");
            DropColumn("dbo.Reserveds", "DeniedEmails");
            DropColumn("dbo.Reserveds", "AcceptedEmails");
            DropColumn("dbo.Reserveds", "Description");
        }
    }
}
