using Microsoft.EntityFrameworkCore;
using Test.Entities;

namespace Test.Repo
{
   public class AppDbContext : DbContext //inherits from Dbcontext to manage Db operations 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } //used to map C# class to db table 
        public DbSet<UserDetails> UserDetailsList { get; set; }
        public DbSet<MissionTheme> MissionTheme {  get; set; }
        public DbSet<MissionSkill> MissionSkill {  get; set; }

    }
}
