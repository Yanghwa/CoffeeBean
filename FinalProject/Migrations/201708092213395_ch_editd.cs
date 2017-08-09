namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ch_editd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ratings", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Beans", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CoffeeBeans", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Coffees", "EditDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Coffees", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CoffeeBeans", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Beans", "EditDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Ratings", "EditDate", c => c.DateTime(nullable: false));
        }
    }
}
