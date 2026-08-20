namespace Static_and_Login.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPasswordResertOtp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordResertOTPs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        OtpCode = c.String(nullable: false, maxLength: 6),
                        ExpiryTime = c.DateTime(nullable: false),
                        IsUsed = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PasswordResertOTPs");
        }
    }
}
