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
        public DbSet<Ad> Ads { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<HelpQuestion> HelpQuestions { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Qusi> Qusis { get; set; } = null!;
        public DbSet<QusiImage> QusiImages { get; set; } = null!;
        public DbSet<FavoriteStory> FavoriteStories { get; set; } = null!;
        public DbSet<StoryBookmark> StoryBookmarks { get; set; } = null!;
        public DbSet<Drawing> Drawings { get; set; } = null!;
        public DbSet<Template> Templates { get; set; } = null!;
        public DbSet<QuizQuestion> QuizQuestions { get; set; } = null!;
        public DbSet<QuizOption> QuizOptions { get; set; } = null!;
        public DbSet<QuizResult> QuizResults { get; set; } = null!;
        public DbSet<Semester> Semesters { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<ExerciseQuestion> ExerciseQuestions { get; set; } = null!;
        public DbSet<UserLesson> UserLessons { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserPal>().ToTable("UsersPal");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Admin>().ToTable("Admins");

            // منع حذف تلقائي بين UsersPal و Semesters لتجنب مشاكل Multiple Cascade Paths
            modelBuilder.Entity<UserPal>()
                .HasOne(u => u.Semester)
                .WithMany(s => s.UsersPal)
                .HasForeignKey(u => u.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقة UserLesson مع UserPal و Lesson
            modelBuilder.Entity<UserLesson>()
                .HasOne(ul => ul.UserPal)
                .WithMany(u => u.UserLessons)
                .HasForeignKey(ul => ul.UserPalId);

            modelBuilder.Entity<UserLesson>()
                .HasOne(ul => ul.Lesson)
                .WithMany(l => l.UserLessons)
                .HasForeignKey(ul => ul.LessonId);

            // علاقة QuizOption مع QuizQuestion
            modelBuilder.Entity<QuizOption>()
                .HasOne(o => o.QuizQuestion)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة QuizResult مع UserPal
            modelBuilder.Entity<QuizResult>()
                .HasOne(r => r.User)
                .WithMany(u => u.QuizResults)
                .HasForeignKey(r => r.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة QusiImage مع Qusi
            modelBuilder.Entity<QusiImage>()
                .HasOne(img => img.Qusi)
                .WithMany(q => q.Images)
                .HasForeignKey(img => img.QusiId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة FavoriteStory مع UserPal و Qusi
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

            // علاقة StoryBookmark مع UserPal و Qusi
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

            // علاقة Drawing مع UserPal و Template
            modelBuilder.Entity<Drawing>()
                .HasOne(d => d.User)
                .WithMany(u => u.Drawings)
                .HasForeignKey(d => d.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Drawing>()
                .HasOne(d => d.Template)
                .WithMany(t => t.Drawings)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.SetNull); // للحفاظ على الرسم إذا حذف القالب

            // علاقة Book مع Semester
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Semester)
                .WithMany(s => s.Books)
                .HasForeignKey(b => b.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة Lesson مع Book
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Lessons)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة ExerciseQuestion مع Lesson
            modelBuilder.Entity<ExerciseQuestion>()
                .HasOne(eq => eq.Lesson)
                .WithMany(l => l.ExerciseQuestions)
                .HasForeignKey(eq => eq.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
