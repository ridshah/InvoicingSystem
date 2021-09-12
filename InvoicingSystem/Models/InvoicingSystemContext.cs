using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InvoicingSystem.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class InvoicingSystemContext : DbContext
    {
        //this is the name of the connection string defined in Web.config file
        public InvoicingSystemContext() : base("name = dbConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(15, 5));
        }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Invoice_Item> Invoice_Items { get; set; }
    }
}