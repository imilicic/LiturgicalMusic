namespace LiturgicalMusic.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InstrumentalParts", "Song_Id", "dbo.Songs");
            DropForeignKey("dbo.Stanzas", "Song_Id", "dbo.Songs");
            DropIndex("dbo.InstrumentalParts", new[] { "Song_Id" });
            DropIndex("dbo.Stanzas", new[] { "Song_Id" });
            RenameColumn(table: "dbo.Songs", name: "Arranger_Id", newName: "ArrangerId");
            RenameColumn(table: "dbo.Songs", name: "Composer_Id", newName: "ComposerId");
            RenameColumn(table: "dbo.InstrumentalParts", name: "Song_Id", newName: "SongId");
            RenameColumn(table: "dbo.Stanzas", name: "Song_Id", newName: "SongId");
            RenameIndex(table: "dbo.Songs", name: "IX_Composer_Id", newName: "IX_ComposerId");
            RenameIndex(table: "dbo.Songs", name: "IX_Arranger_Id", newName: "IX_ArrangerId");
            DropPrimaryKey("dbo.Stanzas");
            AlterColumn("dbo.InstrumentalParts", "SongId", c => c.Int(nullable: false));
            AlterColumn("dbo.Stanzas", "Number", c => c.Int(nullable: false));
            AlterColumn("dbo.Stanzas", "SongId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Stanzas", new[] { "Number", "SongId" });
            CreateIndex("dbo.InstrumentalParts", "SongId");
            CreateIndex("dbo.Stanzas", "SongId");
            AddForeignKey("dbo.InstrumentalParts", "SongId", "dbo.Songs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Stanzas", "SongId", "dbo.Songs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stanzas", "SongId", "dbo.Songs");
            DropForeignKey("dbo.InstrumentalParts", "SongId", "dbo.Songs");
            DropIndex("dbo.Stanzas", new[] { "SongId" });
            DropIndex("dbo.InstrumentalParts", new[] { "SongId" });
            DropPrimaryKey("dbo.Stanzas");
            AlterColumn("dbo.Stanzas", "SongId", c => c.Int());
            AlterColumn("dbo.Stanzas", "Number", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.InstrumentalParts", "SongId", c => c.Int());
            AddPrimaryKey("dbo.Stanzas", "Number");
            RenameIndex(table: "dbo.Songs", name: "IX_ArrangerId", newName: "IX_Arranger_Id");
            RenameIndex(table: "dbo.Songs", name: "IX_ComposerId", newName: "IX_Composer_Id");
            RenameColumn(table: "dbo.Stanzas", name: "SongId", newName: "Song_Id");
            RenameColumn(table: "dbo.InstrumentalParts", name: "SongId", newName: "Song_Id");
            RenameColumn(table: "dbo.Songs", name: "ComposerId", newName: "Composer_Id");
            RenameColumn(table: "dbo.Songs", name: "ArrangerId", newName: "Arranger_Id");
            CreateIndex("dbo.Stanzas", "Song_Id");
            CreateIndex("dbo.InstrumentalParts", "Song_Id");
            AddForeignKey("dbo.Stanzas", "Song_Id", "dbo.Songs", "Id");
            AddForeignKey("dbo.InstrumentalParts", "Song_Id", "dbo.Songs", "Id");
        }
    }
}
