using Microsoft.EntityFrameworkCore;
using Palesteeny_Project.Models;

namespace Palesteeny_Project.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserPal> UsersPal { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Qusi> Qusis { get; set; } = null!;
        public DbSet<QusiImage> QusiImages { get; set; } = null!;
        public DbSet<FavoriteStory> FavoriteStories { get; set; } = null!;
        public DbSet<StoryBookmark> StoryBookmarks { get; set; } = null!;
        public DbSet<Drawing> Drawings { get; set; } = null!;
        public DbSet<Template> Templates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // علاقة القصة مع الصور (1 إلى كثير)
            modelBuilder.Entity<QusiImage>()
                .HasOne(img => img.Qusi)
                .WithMany(q => q.Images)
                .HasForeignKey(img => img.QusiId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة المستخدم مع المفضلة
            modelBuilder.Entity<FavoriteStory>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteStories)
                .HasForeignKey(f => f.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteStory>()
                .HasOne(f => f.Qusi)
                .WithMany()
                .HasForeignKey(f => f.QusiId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة المستخدم مع البوك مارك
            modelBuilder.Entity<StoryBookmark>()
                .HasOne(b => b.User)
                .WithMany(u => u.StoryBookmarks)
                .HasForeignKey(b => b.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StoryBookmark>()
                .HasOne(b => b.Qusi)
                .WithMany()
                .HasForeignKey(b => b.QusiId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ علاقة المستخدم مع الرسم
            modelBuilder.Entity<Drawing>()
                .HasOne(d => d.User)
                .WithMany(u => u.Drawings)
                .HasForeignKey(d => d.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ علاقة الرسم مع القالب (template)
            modelBuilder.Entity<Drawing>()
                .HasOne(d => d.Template)
                .WithMany(t => t.Drawings)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.SetNull); // إن أردت الاحتفاظ بالرسم إذا حذف القالب
        }
    }
}
