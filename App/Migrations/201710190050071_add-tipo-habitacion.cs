namespace App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtipohabitacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoHabitacion",
                c => new
                    {
                        IdTipoHabitacion = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.IdTipoHabitacion);
            
            AddColumn("dbo.Habitacion", "Numero", c => c.String());
            AddColumn("dbo.Habitacion", "IdTipoHabitacion", c => c.Int(nullable: false));
            CreateIndex("dbo.Habitacion", "IdTipoHabitacion");
            AddForeignKey("dbo.Habitacion", "IdTipoHabitacion", "dbo.TipoHabitacion", "IdTipoHabitacion", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Habitacion", "IdTipoHabitacion", "dbo.TipoHabitacion");
            DropIndex("dbo.Habitacion", new[] { "IdTipoHabitacion" });
            DropColumn("dbo.Habitacion", "IdTipoHabitacion");
            DropColumn("dbo.Habitacion", "Numero");
            DropTable("dbo.TipoHabitacion");
        }
    }
}
