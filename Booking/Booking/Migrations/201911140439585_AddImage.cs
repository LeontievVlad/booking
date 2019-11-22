namespace Booking.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "Description", c => c.String());
            AddColumn("dbo.Rooms", "Image", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Rooms", "Image");
            DropColumn("dbo.Rooms", "Description");
        }
    }
}
