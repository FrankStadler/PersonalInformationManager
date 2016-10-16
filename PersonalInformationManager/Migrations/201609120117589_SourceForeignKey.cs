namespace PersonalInformationManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SourceForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shows", "Source_SourceID", "dbo.Sources");
            DropIndex("dbo.Shows", new[] { "Source_SourceID" });
            RenameColumn(table: "dbo.Shows", name: "Source_SourceID", newName: "SourceID");
            AlterColumn("dbo.Shows", "SourceID", c => c.Int(nullable: false));
            CreateIndex("dbo.Shows", "SourceID");
            AddForeignKey("dbo.Shows", "SourceID", "dbo.Sources", "SourceID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shows", "SourceID", "dbo.Sources");
            DropIndex("dbo.Shows", new[] { "SourceID" });
            AlterColumn("dbo.Shows", "SourceID", c => c.Int());
            RenameColumn(table: "dbo.Shows", name: "SourceID", newName: "Source_SourceID");
            CreateIndex("dbo.Shows", "Source_SourceID");
            AddForeignKey("dbo.Shows", "Source_SourceID", "dbo.Sources", "SourceID");
        }
    }
}
