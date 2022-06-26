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
        public DbSet<YorumCevap> YorumCevaplar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(connectionString: @"server=.;database=YORUMUNUYAZDB;user id=sa;password=122333;multipleactiveresultsets=true;"); // ev
            optionsBuilder.UseSqlServer(connectionString: @"Server=(localdb)\mssqllocaldb; initial catalog  =YorumunuYaz; integrated security = true"); // iş
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>()
                .ToTable("Kullanicilar")
                .HasOne(kullanici => kullanici.Rol)
                .WithMany(rol => rol.Kullanicilar)
                .HasForeignKey(rol=>rol.RolId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Rol>()
                .ToTable("Roller")
                .HasMany(rol => rol.Kullanicilar);



            modelBuilder.Entity<Yorum>()
                .ToTable("Yorumlar")
                .HasOne(yorum => yorum.Kategori)
                .WithMany(kategori => kategori.Yorumlar)
                .HasForeignKey(kategori => kategori.KategoriId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Kategori>()
                .ToTable("Kategoriler")
                .HasMany(kategori => kategori.Yorumlar);

            modelBuilder.Entity<YorumCevap>()
                .ToTable("YorumCevaplar")
                .HasOne(yorumcevap => yorumcevap.Yorum)
                .WithMany(yorum => yorum.YorumCevaplar)
                .HasForeignKey(yorum => yorum.YorumId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
