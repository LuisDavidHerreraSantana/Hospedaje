namespace App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columnnamehospedaje : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hospedaje", "Nombre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Hospedaje", "Nombre");
        }
    }
}
