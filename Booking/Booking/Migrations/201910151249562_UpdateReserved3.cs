namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserved3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Reserveds");
            AddColumn("dbo.Reserveds", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Reserveds", "ReservedId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Reserveds", "Id");
            DropColumn("dbo.Reserveds", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reserveds", "UserName", c => c.String());
            DropPrimaryKey("dbo.Reserveds");
            AlterColumn("dbo.Reserveds", "ReservedId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Reserveds", "Id");
            AddPrimaryKey("dbo.Reserveds", "ReservedId");
        }
    }
}
