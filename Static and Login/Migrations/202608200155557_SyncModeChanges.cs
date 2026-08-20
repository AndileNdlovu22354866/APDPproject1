namespace Static_and_Login.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncModeChanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PasswordResertOTPs", newName: "PasswordresetOtps");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PasswordresetOtps", newName: "PasswordResertOTPs");
        }
    }
}
