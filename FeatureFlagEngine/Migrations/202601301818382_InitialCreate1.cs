namespace FeatureFlagEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Features", "Name", c => c.String());
            AlterColumn("dbo.Features", "State", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Features", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Features", "Description", c => c.Int(nullable: false));
            AlterColumn("dbo.Features", "State", c => c.Int(nullable: false));
            AlterColumn("dbo.Features", "Name", c => c.Int(nullable: false));
        }
    }
}
