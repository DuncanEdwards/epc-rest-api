using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Entities
{
    public class EpcContext : DbContext
    {

        #region Public Properties

        public DbSet<EpcType> EpcTypes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        #endregion

        #region Public Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EpcContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public EpcContext(DbContextOptions<EpcContext> options) 
            : base(options)
        {
            Database.Migrate();
        }
        public EpcContext() {}

        #endregion

        #region Protected Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(user => user.Email )
            .IsUnique(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString = Configuration["connectionStrings:epcDBConnectionString"];
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = Epc; Trusted_Connection = True");
        }

        #endregion



    }
}
