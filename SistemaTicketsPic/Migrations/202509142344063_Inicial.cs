namespace SistemaTicketsPic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Usuarios", "Clave", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Usuarios", "Clave");
        }
    }
}
