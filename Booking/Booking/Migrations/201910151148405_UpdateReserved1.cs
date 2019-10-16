namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserved1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserveds", "UserName", c => c.String());
            DropColumn("dbo.Reserveds", "UserId");
            DropColumn("dbo.Reserveds", "ApplicationUserManager_UserLockoutEnabledByDefault");
            DropColumn("dbo.Reserveds", "ApplicationUserManager_MaxFailedAccessAttemptsBeforeLockout");
            DropColumn("dbo.Reserveds", "ApplicationUserManager_DefaultAccountLockoutTimeSpan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reserveds", "ApplicationUserManager_DefaultAccountLockoutTimeSpan", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Reserveds", "ApplicationUserManager_MaxFailedAccessAttemptsBeforeLockout", c => c.Int(nullable: false));
            AddColumn("dbo.Reserveds", "ApplicationUserManager_UserLockoutEnabledByDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reserveds", "UserId", c => c.Int());
            DropColumn("dbo.Reserveds", "UserName");
        }
    }
}
