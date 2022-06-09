using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities;

namespace Todo.DAL.Data
{
    public class TodoDbContext : IdentityDbContext<ApplicationUser>
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
        public DbSet<TodoEntity> Todos { get; set; }
    }
}
