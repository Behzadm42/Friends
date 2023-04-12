using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
namespace WebApplication3.Data
{
    public class FrindDbContext:DbContext
    {
        public FrindDbContext(DbContextOptions<FrindDbContext> options) :base(options)
        {

        }
        public DbSet<WebApplication3.Models.Friend>? Friend { get; set; }
    }
}
