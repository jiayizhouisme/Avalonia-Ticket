using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GetStartedApp.ViewModels
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Exhibition> Exhibition { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<UserInfos> UserInfos { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connStr = "Data Source=.;Initial Catalog=CommonTicket1;user id=sa;password=Aa123456;TrustServerCertificate=true";
            string connStr = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=CommonTicket2;user id=sa;password=Aa123456;TrustServerCertificate=true";
            optionsBuilder.UseSqlServer(connStr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //  modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Order>()
            .Property(e => e.Status)
            .HasConversion(
                v => (int)v,
                v => (OrderStatus)v);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Exhibition)
            .WithMany(e => e.Appointment)
            .HasForeignKey(a => a.objectId);
            //modelBuilder.Entity<Order>()
            //     .ToTable("Order")

            //   .Property(o => o.Trade_No).HasColumnName("Trade_No");
            //modelBuilder.Entity<Order>()

            //   .Property(o => o.AppointmentId).HasColumnName("AppointmentId");
            modelBuilder.Entity<UserInfos>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                   .ValueGeneratedOnAdd();
            });
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}

   
