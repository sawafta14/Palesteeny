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
     
        public DbSet<SearchableContent> SearchableContents { get; set; } = null!;
        public DbSet<ChatLog> ChatLogs { get; set; } = null!;
        public DbSet<Semester> Semesters { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<ExerciseQuestion> ExerciseQuestions { get; set; } = null!;
        public DbSet<ExerciseOption> ExerciseOptions { get; set; } = null!;
        public DbSet<UserExerciseAnswer> UserExerciseAnswers { get; set; } = null!;
        public DbSet<BookProgress> BookProgresses { get; set; } = null!;
        public DbSet<UserLesson> UserLessons { get; set; } = null!;
        public DbSet<AIAssistant> AIAssistant { get; set; } = null!;
        public DbSet<QuestionGroup> QuestionGroups { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserPal>().ToTable("UsersPal");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Admin>().ToTable("Admins");

         
            modelBuilder.Entity<UserPal>()
                .HasOne(u => u.Semester)
                .WithMany(s => s.UsersPal)
                .HasForeignKey(u => u.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<UserLesson>()
             .HasOne(ul => ul.UserPal)
             .WithMany(u => u.UserLessons)
             .HasForeignKey(ul => ul.UserPalId);

            modelBuilder.Entity<UserLesson>()
                .HasOne(ul => ul.Lesson)
                .WithMany(l => l.UserLessons)
                .HasForeignKey(ul => ul.LessonId);


            modelBuilder.Entity<QuizOption>()
                .HasOne(o => o.QuizQuestion)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<QuizResult>()
                .HasOne(r => r.User)
                .WithMany(u => u.QuizResults)
                .HasForeignKey(r => r.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<QusiImage>()
                .HasOne(img => img.Qusi)
                .WithMany(q => q.Images)
                .HasForeignKey(img => img.QusId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<FavoriteStory>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteStories)
                .HasForeignKey(f => f.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteStory>()
                .HasOne(f => f.Qusi)
                .WithMany()
                .HasForeignKey(f => f.QusId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<StoryBookmark>()
                .HasOne(b => b.User)
                .WithMany(u => u.StoryBookmarks)
                .HasForeignKey(b => b.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StoryBookmark>()
                .HasOne(b => b.Qusi)
                .WithMany()
                .HasForeignKey(b => b.QusId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Drawing>()
                .HasOne(d => d.User)
                .WithMany(u => u.Drawings)
                .HasForeignKey(d => d.UserPalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Drawing>()
                .HasOne(d => d.Template)
                .WithMany(t => t.Drawings)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.SetNull); 

           
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Semester)
                .WithMany(s => s.Books)
                .HasForeignKey(b => b.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            
          

            
          


            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Lessons)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة ExerciseQuestion مع Lesson
            modelBuilder.Entity<QuestionGroup>()
                .HasOne(qg => qg.Lesson)
                .WithMany(l => l.QuestionGroups) // Change if needed
                .HasForeignKey(qg => qg.LessonId)

                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<Palesteeny_Project.Models.ExerciseMatch> ExerciseMatch { get; set; } = default!;
    }
}
