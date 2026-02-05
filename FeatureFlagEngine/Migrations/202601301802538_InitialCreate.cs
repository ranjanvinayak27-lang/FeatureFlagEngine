namespace FeatureFlagEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Description = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupOverrides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeatureId = c.Int(nullable: false),
                        GroupName = c.String(),
                        State = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserOverrides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeatureId = c.Int(nullable: false),
                        UserId = c.String(),
                        State = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserOverrides");
            DropTable("dbo.GroupOverrides");
            DropTable("dbo.Features");
        }
    }
}
