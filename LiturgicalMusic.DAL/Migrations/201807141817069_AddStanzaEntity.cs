namespace LiturgicalMusic.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStanzaEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stanzas",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Song_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Number)
                .ForeignKey("dbo.Songs", t => t.Song_Id)
                .Index(t => t.Song_Id);
            
            DropColumn("dbo.Songs", "Lyrics");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Songs", "Lyrics", c => c.String(nullable: false));
            DropForeignKey("dbo.Stanzas", "Song_Id", "dbo.Songs");
            DropIndex("dbo.Stanzas", new[] { "Song_Id" });
            DropTable("dbo.Stanzas");
        }
    }
}
