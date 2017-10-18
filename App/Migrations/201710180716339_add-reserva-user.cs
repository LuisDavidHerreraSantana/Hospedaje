namespace App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addreservauser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "IdUsuario", c => c.String(maxLength: 128));
            CreateIndex("dbo.Cliente", "IdUsuario");
            AddForeignKey("dbo.Cliente", "IdUsuario", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cliente", "IdUsuario", "dbo.AspNetUsers");
            DropIndex("dbo.Cliente", new[] { "IdUsuario" });
            DropColumn("dbo.Cliente", "IdUsuario");
        }
    }
}
