namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserveds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "UserId", c => c.Int());
            AddColumn("dbo.Reserveds", "ApplicationUserManager_UserLockoutEnabledByDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reserveds", "ApplicationUserManager_MaxFailedAccessAttemptsBeforeLockout", c => c.Int(nullable: false));
            AddColumn("dbo.Reserveds", "ApplicationUserManager_DefaultAccountLockoutTimeSpan", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reserveds", "ApplicationUserManager_DefaultAccountLockoutTimeSpan");
            DropColumn("dbo.Reserveds", "ApplicationUserManager_MaxFailedAccessAttemptsBeforeLockout");
            DropColumn("dbo.Reserveds", "ApplicationUserManager_UserLockoutEnabledByDefault");
            DropColumn("dbo.Reserveds", "UserId");
        }
    }
}
