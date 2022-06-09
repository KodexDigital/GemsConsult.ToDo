using Microsoft.AspNetCore.Identity;

namespace Todo.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public virtual ICollection<TodoEntity> Todos { get; set; }
        public ApplicationUser()
        {
            Todos = new HashSet<TodoEntity>();
        }
    }
}
