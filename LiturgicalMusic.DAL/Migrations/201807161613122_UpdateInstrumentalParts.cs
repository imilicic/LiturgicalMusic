namespace LiturgicalMusic.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInstrumentalParts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InstrumentalParts", "Position", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InstrumentalParts", "Position");
        }
    }
}
