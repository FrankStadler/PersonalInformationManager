namespace PersonalInformationManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShowImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shows", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shows", "Image");
        }
    }
}
