using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SKS.ParcelLogistics.DataAccess.Entities;

namespace SKS.ParcelLogistics.DataAccess.SQL
{
    public class ParcelLogisticsContext : DbContext
    {

        public ParcelLogisticsContext(DbContextOptions<ParcelLogisticsContext> options) : base(options) { }

        public DbSet<ParcelDTO> Parcels { get; set; }
        public DbSet<TruckDTO> Trucks { get; set; }
        public DbSet<RecipientDTO> Recipients { get; set; }
        public DbSet<WarehouseDTO> Warehouses { get; set; }
        public DbSet<HopArrivalDTO> HopArrivals { get; set; }

       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("" +
                "Server=tcp:vaiaphraim.database.windows.net,1433;" +
                "Initial Catalog=parcellogistics;" +
                "Persist Security Info=False;" +
                "User ID=boss;" +
                "Password=Parcel123;" +
                "MultipleActiveResultSets=False;" +
                "Encrypt=True;" +
                "TrustServerCertificate=False;" +
                "Connection Timeout=30;");
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParcelDTO>().ToTable("Parcels");
            modelBuilder.Entity<TruckDTO>().ToTable("Trucks");
            modelBuilder.Entity<RecipientDTO>().ToTable("Recipients");
            modelBuilder.Entity<WarehouseDTO>().ToTable("Warehouses");
            modelBuilder.Entity<HopArrivalDTO>().ToTable("Hops");
        }
    }
}