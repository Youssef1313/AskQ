using AskQ.Core.Entities;
using AskQ.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AskQ.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Reply> Replies => Set<Reply>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new ReplyConfiguration());
        }
    }
}
