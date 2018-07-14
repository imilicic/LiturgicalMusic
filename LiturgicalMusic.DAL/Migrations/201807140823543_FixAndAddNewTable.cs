namespace LiturgicalMusic.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAndAddNewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InstrumentalParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Template = c.Int(nullable: false),
                        Type = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false),
                        Song_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Songs", t => t.Song_Id)
                .Index(t => t.Song_Id);
            
            AddColumn("dbo.Songs", "Lyrics", c => c.String(nullable: false));
            AlterColumn("dbo.Songs", "Template", c => c.Int(nullable: false));
            AlterColumn("dbo.Songs", "OtherParts", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InstrumentalParts", "Song_Id", "dbo.Songs");
            DropIndex("dbo.InstrumentalParts", new[] { "Song_Id" });
            AlterColumn("dbo.Songs", "OtherParts", c => c.Int(nullable: false));
            AlterColumn("dbo.Songs", "Template", c => c.String(nullable: false, maxLength: 4));
            DropColumn("dbo.Songs", "Lyrics");
            DropTable("dbo.InstrumentalParts");
        }
    }
}
