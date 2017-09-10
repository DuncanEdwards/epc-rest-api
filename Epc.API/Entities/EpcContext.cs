using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Entities
{
    public class EpcContext : DbContext
    {

        #region Public Properties

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

        #endregion

        #region Protected Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(user => user.Email )
            .IsUnique(true);
        }

        #endregion



    }
}
