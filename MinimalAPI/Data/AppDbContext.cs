using Microsoft.EntityFrameworkCore;

namespace MinimalAPI.Data
{
    internal sealed class AppDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder) => dbContextOptionsBuilder.UseSqlite("Data Source =./Data/MinimalApi.db");
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Post[] postsToSeed = new Post[6];

            for (int i = 0; i < postsToSeed.Length; i++)
            {
                postsToSeed[i] = new Post
                {
                    PostId = i+1,
                    Title = $"Post {i+1}",
                    Content = $"This is post {i+1} and it has some very interesting content.",
                };
            }

            modelBuilder.Entity<Post>().HasData(postsToSeed);
        }
    }
}
