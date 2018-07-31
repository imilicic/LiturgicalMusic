namespace LiturgicalMusic.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Composers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Surname = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Template = c.Int(nullable: false),
                        Type = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false),
                        OtherParts = c.Boolean(nullable: false),
                        Source = c.String(),
                        OtherInformations = c.String(),
                        ComposerId = c.Int(),
                        ArrangerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composers", t => t.ArrangerId)
                .ForeignKey("dbo.Composers", t => t.ComposerId)
                .Index(t => t.ComposerId)
                .Index(t => t.ArrangerId);
            
            CreateTable(
                "dbo.InstrumentalParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Template = c.Int(nullable: false),
                        Position = c.String(nullable: false, maxLength: 20),
                        Type = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false),
                        SongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .Index(t => t.SongId);
            
            CreateTable(
                "dbo.LiturgyCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stanzas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        SongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .Index(t => t.SongId);
            
            CreateTable(
                "dbo.ThemeCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LiturgyEntitySongEntities",
                c => new
                    {
                        LiturgyEntity_Id = c.Int(nullable: false),
                        SongEntity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LiturgyEntity_Id, t.SongEntity_Id })
                .ForeignKey("dbo.LiturgyCategories", t => t.LiturgyEntity_Id, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.SongEntity_Id, cascadeDelete: true)
                .Index(t => t.LiturgyEntity_Id)
                .Index(t => t.SongEntity_Id);
            
            CreateTable(
                "dbo.ThemeEntitySongEntities",
                c => new
                    {
                        ThemeEntity_Id = c.Int(nullable: false),
                        SongEntity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ThemeEntity_Id, t.SongEntity_Id })
                .ForeignKey("dbo.ThemeCategories", t => t.ThemeEntity_Id, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.SongEntity_Id, cascadeDelete: true)
                .Index(t => t.ThemeEntity_Id)
                .Index(t => t.SongEntity_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Songs", "ComposerId", "dbo.Composers");
            DropForeignKey("dbo.Songs", "ArrangerId", "dbo.Composers");
            DropForeignKey("dbo.ThemeEntitySongEntities", "SongEntity_Id", "dbo.Songs");
            DropForeignKey("dbo.ThemeEntitySongEntities", "ThemeEntity_Id", "dbo.ThemeCategories");
            DropForeignKey("dbo.Stanzas", "SongId", "dbo.Songs");
            DropForeignKey("dbo.LiturgyEntitySongEntities", "SongEntity_Id", "dbo.Songs");
            DropForeignKey("dbo.LiturgyEntitySongEntities", "LiturgyEntity_Id", "dbo.LiturgyCategories");
            DropForeignKey("dbo.InstrumentalParts", "SongId", "dbo.Songs");
            DropIndex("dbo.ThemeEntitySongEntities", new[] { "SongEntity_Id" });
            DropIndex("dbo.ThemeEntitySongEntities", new[] { "ThemeEntity_Id" });
            DropIndex("dbo.LiturgyEntitySongEntities", new[] { "SongEntity_Id" });
            DropIndex("dbo.LiturgyEntitySongEntities", new[] { "LiturgyEntity_Id" });
            DropIndex("dbo.Stanzas", new[] { "SongId" });
            DropIndex("dbo.InstrumentalParts", new[] { "SongId" });
            DropIndex("dbo.Songs", new[] { "ArrangerId" });
            DropIndex("dbo.Songs", new[] { "ComposerId" });
            DropTable("dbo.ThemeEntitySongEntities");
            DropTable("dbo.LiturgyEntitySongEntities");
            DropTable("dbo.ThemeCategories");
            DropTable("dbo.Stanzas");
            DropTable("dbo.LiturgyCategories");
            DropTable("dbo.InstrumentalParts");
            DropTable("dbo.Songs");
            DropTable("dbo.Composers");
        }
    }
}
