namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstChangeDb : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
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
    }
}
