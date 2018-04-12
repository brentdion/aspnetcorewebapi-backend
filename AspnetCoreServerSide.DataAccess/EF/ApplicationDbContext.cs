using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspnetCoreServerSide.DataAccess.EF
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        // ========== Define DbSets ====================
        //public DbSet<ExampleModel> Examples { get; set; }


        // ========== Define table mappings =============
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.EntityExampleModel>().ToTable("ExampleTable");
           
        }
    }
}
