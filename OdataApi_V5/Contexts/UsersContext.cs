using Microsoft.EntityFrameworkCore;
using OdataApi_V5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdataApi_V5.Contexts
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {

        }

        /// <summary>
        /// The table in our database as reference
        /// </summary>
        public DbSet<Users> Users { get; set; }


        /// <summary>
        /// Uses the Entity framework
        /// </summary>
        /// <param name="modelBuilder"></param>
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Users>().HasKey(c => c.Userid);
        //}
    }
}
