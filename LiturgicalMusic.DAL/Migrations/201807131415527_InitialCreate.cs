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
                        Template = c.String(nullable: false, maxLength: 4),
                        Type = c.String(nullable: false, maxLength: 20),
                        Code = c.String(nullable: false),
                        OtherParts = c.Int(nullable: false),
                        Source = c.String(),
                        OtherInformations = c.String(),
                        Arranger_Id = c.Int(),
                        Composer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composers", t => t.Arranger_Id)
                .ForeignKey("dbo.Composers", t => t.Composer_Id)
                .Index(t => t.Arranger_Id)
                .Index(t => t.Composer_Id);
            
            CreateTable(
                "dbo.LiturgyCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.Songs", "Composer_Id", "dbo.Composers");
            DropForeignKey("dbo.Songs", "Arranger_Id", "dbo.Composers");
            DropForeignKey("dbo.ThemeEntitySongEntities", "SongEntity_Id", "dbo.Songs");
            DropForeignKey("dbo.ThemeEntitySongEntities", "ThemeEntity_Id", "dbo.ThemeCategories");
            DropForeignKey("dbo.LiturgyEntitySongEntities", "SongEntity_Id", "dbo.Songs");
            DropForeignKey("dbo.LiturgyEntitySongEntities", "LiturgyEntity_Id", "dbo.LiturgyCategories");
            DropIndex("dbo.ThemeEntitySongEntities", new[] { "SongEntity_Id" });
            DropIndex("dbo.ThemeEntitySongEntities", new[] { "ThemeEntity_Id" });
            DropIndex("dbo.LiturgyEntitySongEntities", new[] { "SongEntity_Id" });
            DropIndex("dbo.LiturgyEntitySongEntities", new[] { "LiturgyEntity_Id" });
            DropIndex("dbo.Songs", new[] { "Composer_Id" });
            DropIndex("dbo.Songs", new[] { "Arranger_Id" });
            DropTable("dbo.ThemeEntitySongEntities");
            DropTable("dbo.LiturgyEntitySongEntities");
            DropTable("dbo.ThemeCategories");
            DropTable("dbo.LiturgyCategories");
            DropTable("dbo.Songs");
            DropTable("dbo.Composers");
        }
    }
}
