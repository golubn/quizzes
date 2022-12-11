using Microsoft.EntityFrameworkCore;
using Quiz.Models;


namespace Quiz
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
         : base(options)
        {
            //Database.EnsureDeleted();   
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(a => a.Games)
                .WithOne(b => b.User);
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasData(
        //           new User { UserId = 1, UserName = "Maksimush", Password = "1234" },
        //           new User { UserId = 2, UserName = "Maksim", Password = "123456" }
        //    );
        //    modelBuilder.Entity<Game>().HasData(
        //             new Game { UserId = 1, GameName = "Quiz1", Points = 10 },
        //             new Game { UserId = 2, GameName = "Oqiz2", Points = 15 }

        //     );

        //}
    }
}
