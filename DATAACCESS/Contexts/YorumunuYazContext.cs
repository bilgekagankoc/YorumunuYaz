using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class YorumunuYazContext : DbContext
    {
        public DbSet<Rol> Roller { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(connectionString: @"server=.;database=YORUMUNUYAZDB;user id=sa;password=122333;multipleactiveresultsets=true;"); // ev
            optionsBuilder.UseSqlServer(connectionString: @"Server=(localdb)\mssqllocaldb; initial catalog  =YorumunuYaz; integrated security = true"); // iş
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>()
                .ToTable("Kullanicilar");

            modelBuilder.Entity<Rol>()
                .ToTable("Roller");

            modelBuilder.Entity<Yorum>()
                .ToTable("Yorumlar");

            modelBuilder.Entity<Kategori>()
                .ToTable("Kategoriler");
        }
    }
}
