namespace PersonalInformationManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BooksAndMovies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        ReleaseDate = c.DateTime(),
                        ViewedDate = c.DateTime(),
                        Image = c.Binary(),
                        SourceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.Sources", t => t.SourceID, cascadeDelete: true)
                .Index(t => t.SourceID);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ReleaseDate = c.DateTime(),
                        ViewedDate = c.DateTime(),
                        Image = c.Binary(),
                        SourceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MovieID)
                .ForeignKey("dbo.Sources", t => t.SourceID, cascadeDelete: true)
                .Index(t => t.SourceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "SourceID", "dbo.Sources");
            DropForeignKey("dbo.Books", "SourceID", "dbo.Sources");
            DropIndex("dbo.Movies", new[] { "SourceID" });
            DropIndex("dbo.Books", new[] { "SourceID" });
            DropTable("dbo.Movies");
            DropTable("dbo.Books");
        }
    }
}
