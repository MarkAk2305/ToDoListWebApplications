namespace ToDoListWebApplications.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ToDoListWebApplications.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<ToDoListWebApplications.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ToDoListWebApplications.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            AddUsers(context);
        }

        void AddUsers(ToDoListWebApplications.Models.ApplicationDbContext context)
        {
            var user = new ApplicationUser { UserName = "admin@email.com" }; // this is the user we defined in the todo.cs
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            userManager.Create(user, "password");
        }

    }
}
