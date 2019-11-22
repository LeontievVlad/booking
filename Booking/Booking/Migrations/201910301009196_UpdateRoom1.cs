namespace Booking.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateRoom1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rooms", "Date");
        }

        public override void Down()
        {
            AddColumn("dbo.Rooms", "Date", c => c.DateTime(nullable: false));
        }
    }
}
