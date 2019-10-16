namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReservedsDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reserveds",
                c => new
                    {
                        ReservedId = c.Int(nullable: false, identity: true),
                        ReservedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReservedId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reserveds");
        }
    }
}
