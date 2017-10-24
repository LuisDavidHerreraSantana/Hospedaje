namespace App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model_refactoring : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        Dni = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(nullable: false),
                        ApellidoPaterno = c.String(nullable: false),
                        ApellidoMaterno = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        IdUsuario = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Dni)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Reserva",
                c => new
                    {
                        IdReserva = c.String(nullable: false, maxLength: 128),
                        Dni = c.String(nullable: false, maxLength: 128),
                        Fecha = c.DateTime(nullable: false),
                        createdAt = c.DateTime(nullable: false),
                        Estado = c.String(),
                    })
                .PrimaryKey(t => t.IdReserva)
                .ForeignKey("dbo.Cliente", t => t.Dni, cascadeDelete: true)
                .Index(t => t.Dni);
            
            CreateTable(
                "dbo.ReservaDetalle",
                c => new
                    {
                        IdReservaDetalle = c.Int(nullable: false, identity: true),
                        IdHabitacion = c.Int(nullable: false),
                        IdReserva = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdReservaDetalle)
                .ForeignKey("dbo.Habitacion", t => t.IdHabitacion, cascadeDelete: true)
                .ForeignKey("dbo.Reserva", t => t.IdReserva)
                .Index(t => t.IdHabitacion)
                .Index(t => t.IdReserva);
            
            CreateTable(
                "dbo.Habitacion",
                c => new
                    {
                        IdHabitacion = c.Int(nullable: false, identity: true),
                        Numero = c.String(nullable: false),
                        Disponible = c.Boolean(nullable: false),
                        IdHospedaje = c.Int(nullable: false),
                        IdTipoHabitacion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdHabitacion)
                .ForeignKey("dbo.Hospedaje", t => t.IdHospedaje, cascadeDelete: true)
                .ForeignKey("dbo.TipoHabitacion", t => t.IdTipoHabitacion, cascadeDelete: true)
                .Index(t => t.IdHospedaje)
                .Index(t => t.IdTipoHabitacion);
            
            CreateTable(
                "dbo.Hospedaje",
                c => new
                    {
                        IdHospedaje = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.IdHospedaje);
            
            CreateTable(
                "dbo.TipoHabitacion",
                c => new
                    {
                        IdTipoHabitacion = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.IdTipoHabitacion);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Cliente", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ReservaDetalle", "IdReserva", "dbo.Reserva");
            DropForeignKey("dbo.ReservaDetalle", "IdHabitacion", "dbo.Habitacion");
            DropForeignKey("dbo.Habitacion", "IdTipoHabitacion", "dbo.TipoHabitacion");
            DropForeignKey("dbo.Habitacion", "IdHospedaje", "dbo.Hospedaje");
            DropForeignKey("dbo.Reserva", "Dni", "dbo.Cliente");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Habitacion", new[] { "IdTipoHabitacion" });
            DropIndex("dbo.Habitacion", new[] { "IdHospedaje" });
            DropIndex("dbo.ReservaDetalle", new[] { "IdReserva" });
            DropIndex("dbo.ReservaDetalle", new[] { "IdHabitacion" });
            DropIndex("dbo.Reserva", new[] { "Dni" });
            DropIndex("dbo.Cliente", new[] { "IdUsuario" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TipoHabitacion");
            DropTable("dbo.Hospedaje");
            DropTable("dbo.Habitacion");
            DropTable("dbo.ReservaDetalle");
            DropTable("dbo.Reserva");
            DropTable("dbo.Cliente");
        }
    }
}
