using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace IC_ebilet.pl.Models
{
    public class SystemContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        //public DbSet<Event> Events { get; set; }
        public SystemContext() :base("SystemContext")
        {
            Database.SetInitializer(new SystemDbInit());
        }
    }

    public class SystemDbInit : CreateDatabaseIfNotExists<SystemContext>
    {
        protected override void Seed(SystemContext context)
        {
            base.Seed(context);
            context.SaveChanges();
        }

        public void InitDatabase(SystemContext context)
        {
        }
    }
}