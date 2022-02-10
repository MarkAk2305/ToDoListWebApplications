namespace ToDoListWebApplications.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirdmigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ToDoes", "CreationDate", c => c.DateTime());
            AlterColumn("dbo.ToDoes", "DueDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ToDoes", "DueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ToDoes", "CreationDate", c => c.DateTime(nullable: false));
        }
    }
}
