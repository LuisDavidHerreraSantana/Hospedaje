namespace App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_estado_reserva : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reserva", "Estado", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reserva", "Estado");
        }
    }
}
