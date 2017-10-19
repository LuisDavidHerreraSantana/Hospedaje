namespace App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvalidationcliente : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reserva", "Dni", "dbo.Cliente");
            DropIndex("dbo.Reserva", new[] { "Dni" });
            AlterColumn("dbo.Cliente", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Cliente", "ApellidoPaterno", c => c.String(nullable: false));
            AlterColumn("dbo.Cliente", "ApellidoMaterno", c => c.String(nullable: false));
            AlterColumn("dbo.Reserva", "Dni", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Reserva", "Dni");
            AddForeignKey("dbo.Reserva", "Dni", "dbo.Cliente", "Dni", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reserva", "Dni", "dbo.Cliente");
            DropIndex("dbo.Reserva", new[] { "Dni" });
            AlterColumn("dbo.Reserva", "Dni", c => c.String(maxLength: 128));
            AlterColumn("dbo.Cliente", "ApellidoMaterno", c => c.String());
            AlterColumn("dbo.Cliente", "ApellidoPaterno", c => c.String());
            AlterColumn("dbo.Cliente", "Nombre", c => c.String());
            CreateIndex("dbo.Reserva", "Dni");
            AddForeignKey("dbo.Reserva", "Dni", "dbo.Cliente", "Dni");
        }
    }
}
