namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserved4 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Reserveds");
            AddColumn("dbo.Reserveds", "UsersId", c => c.String());
            AlterColumn("dbo.Reserveds", "ReservedId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Reserveds", "ReservedId");
            DropColumn("dbo.Reserveds", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reserveds", "Id", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.Reserveds");
            AlterColumn("dbo.Reserveds", "ReservedId", c => c.Int(nullable: false));
            DropColumn("dbo.Reserveds", "UsersId");
            AddPrimaryKey("dbo.Reserveds", "Id");
        }
    }
}
