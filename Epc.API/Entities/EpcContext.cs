using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Entities
{
    public class EpcContext : DbContext
    {

        #region Private Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region Public Properties

        //public DbSet<EpcType> EpcTypes { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        #endregion

        #region Public Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EpcContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public EpcContext(
            DbContextOptions<EpcContext> options,
            IConfiguration configuration) 
            : base(options)
        {
            _configuration = configuration; 
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
            optionsBuilder.UseMySql(_configuration.GetValue<string>("connectionStrings:epcDBConnectionString"));
        }

        #endregion



    }
}
