using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newfactjo.Models;

namespace Newfactjo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<News> NewsItems { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<HiddenCategory> HiddenCategories { get; set; }

        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<NewsLog> NewsLogs { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<HeaderPlaylist> HeaderPlaylists { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }




        // ✅ تعديل العلاقة لتمنع الحذف التلقائي
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NewsLog>()
                .HasOne(log => log.News)
                .WithMany()
                .HasForeignKey(log => log.NewsId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
