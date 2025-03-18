using GetStartedApp.Context.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Context
{
    public class AppEFContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=EFTest;User ID=admin;Password=Aa123456;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 针对Blog实体添加种子数据
            modelBuilder.Entity<Blog>().HasData(
                new Blog()
                {
                    // Id字段要赋值，否则会报错
                    id = 1
                },
                new Blog()
                {
                    id = 2
                },
                new Blog()
                {
                    id = 3
                }
                );
            base.OnModelCreating(modelBuilder);
        }
        // DbSet属性
        public DbSet<Blog> Blogs { get; set; }
    }
}
