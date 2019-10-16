namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserved : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reserveds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdUser = c.String(),
                        IdRoom = c.Int(nullable: false),
                        ReservedTime = c.DateTime(nullable: false),
                        CountPeople = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reserveds");
        }
    }
}
