using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.ViewModel
{




    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
      : base("Priemka_db") { }

        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Receipts> Receipts { get; set; }

    }
}