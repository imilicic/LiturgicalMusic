namespace LiturgicalMusic.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedManyToManyRelationships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LiturgyEntitySongEntities", "LiturgyEntity_Id", "dbo.LiturgyCategories");
            DropForeignKey("dbo.LiturgyEntitySongEntities", "SongEntity_Id", "dbo.Songs");
            DropForeignKey("dbo.ThemeEntitySongEntities", "ThemeEntity_Id", "dbo.ThemeCategories");
            DropForeignKey("dbo.ThemeEntitySongEntities", "SongEntity_Id", "dbo.Songs");
            DropIndex("dbo.LiturgyEntitySongEntities", new[] { "LiturgyEntity_Id" });
            DropIndex("dbo.LiturgyEntitySongEntities", new[] { "SongEntity_Id" });
            DropIndex("dbo.ThemeEntitySongEntities", new[] { "ThemeEntity_Id" });
            DropIndex("dbo.ThemeEntitySongEntities", new[] { "SongEntity_Id" });
            CreateTable(
                "dbo.SongLiturgies",
                c => new
                    {
                        SongId = c.Int(nullable: false),
                        LiturgyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongId, t.LiturgyId })
                .ForeignKey("dbo.LiturgyCategories", t => t.LiturgyId, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.LiturgyId);
            
            CreateTable(
                "dbo.SongThemes",
                c => new
                    {
                        SongId = c.Int(nullable: false),
                        ThemeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SongId, t.ThemeId })
                .ForeignKey("dbo.Songs", t => t.SongId, cascadeDelete: true)
                .ForeignKey("dbo.ThemeCategories", t => t.ThemeId, cascadeDelete: true)
                .Index(t => t.SongId)
                .Index(t => t.ThemeId);
            
            DropTable("dbo.LiturgyEntitySongEntities");
            DropTable("dbo.ThemeEntitySongEntities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ThemeEntitySongEntities",
                c => new
                    {
                        ThemeEntity_Id = c.Int(nullable: false),
                        SongEntity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ThemeEntity_Id, t.SongEntity_Id });
            
            CreateTable(
                "dbo.LiturgyEntitySongEntities",
                c => new
                    {
                        LiturgyEntity_Id = c.Int(nullable: false),
                        SongEntity_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LiturgyEntity_Id, t.SongEntity_Id });
            
            DropForeignKey("dbo.SongThemes", "ThemeId", "dbo.ThemeCategories");
            DropForeignKey("dbo.SongThemes", "SongId", "dbo.Songs");
            DropForeignKey("dbo.SongLiturgies", "SongId", "dbo.Songs");
            DropForeignKey("dbo.SongLiturgies", "LiturgyId", "dbo.LiturgyCategories");
            DropIndex("dbo.SongThemes", new[] { "ThemeId" });
            DropIndex("dbo.SongThemes", new[] { "SongId" });
            DropIndex("dbo.SongLiturgies", new[] { "LiturgyId" });
            DropIndex("dbo.SongLiturgies", new[] { "SongId" });
            DropTable("dbo.SongThemes");
            DropTable("dbo.SongLiturgies");
            CreateIndex("dbo.ThemeEntitySongEntities", "SongEntity_Id");
            CreateIndex("dbo.ThemeEntitySongEntities", "ThemeEntity_Id");
            CreateIndex("dbo.LiturgyEntitySongEntities", "SongEntity_Id");
            CreateIndex("dbo.LiturgyEntitySongEntities", "LiturgyEntity_Id");
            AddForeignKey("dbo.ThemeEntitySongEntities", "SongEntity_Id", "dbo.Songs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ThemeEntitySongEntities", "ThemeEntity_Id", "dbo.ThemeCategories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LiturgyEntitySongEntities", "SongEntity_Id", "dbo.Songs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LiturgyEntitySongEntities", "LiturgyEntity_Id", "dbo.LiturgyCategories", "Id", cascadeDelete: true);
        }
    }
}
