namespace PersonalInformationManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shows",
                c => new
                    {
                        ShowId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Season = c.String(nullable: false),
                        Source = c.String(),
                        ReleaseDate = c.DateTime(),
                        ViewedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ShowId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Shows");
        }
    }
}
