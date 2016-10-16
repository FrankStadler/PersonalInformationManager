namespace PersonalInformationManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Source : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        SourceID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SourceID);
            
            AddColumn("dbo.Shows", "Source_SourceID", c => c.Int());
            CreateIndex("dbo.Shows", "Source_SourceID");
            AddForeignKey("dbo.Shows", "Source_SourceID", "dbo.Sources", "SourceID");
            DropColumn("dbo.Shows", "Source");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shows", "Source", c => c.String());
            DropForeignKey("dbo.Shows", "Source_SourceID", "dbo.Sources");
            DropIndex("dbo.Shows", new[] { "Source_SourceID" });
            DropColumn("dbo.Shows", "Source_SourceID");
            DropTable("dbo.Sources");
        }
    }
}
