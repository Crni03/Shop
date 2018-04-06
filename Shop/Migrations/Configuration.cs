namespace Shop.Migrations
{
    using Shop.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shop.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Shop.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Products.AddOrUpdate(p => p.Name,
                new Product { Name = "Smart Hub", Price = 49.99M },
                new Product { Name = "Motion Sensor", Price = 24.99M, QuantityForPrice = 3, QuantityPrice = 65M },
                new Product { Name = "Wireless Camera", Price = 99.99M },
                new Product { Name = "Smoke Sensor", Price = 19.99M, QuantityForPrice = 2, QuantityPrice = 35M },
                new Product { Name = "Water Leak Sensor", Price = 14.99M}
                );

            context.Discounts.AddOrUpdate(d => d.Name,
                new Discount { Name = "20% OFF", Amount = 20M, Percent = true, CanConjunc = false },
                new Discount { Name = "5% OFF", Amount = 5M, Percent = true, CanConjunc = true },
                new Discount { Name = "20£ OFF", Amount = 20M, Percent = false, CanConjunc = true });
        }


    }
}
