namespace Shop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dva : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Adress", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.AspNetUsers", "CardNumber", c => c.String(nullable: false, maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "CardNumber", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "Adress");
        }
    }
}
