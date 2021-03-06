using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Entities
{
    public class TodoEntity
    {
        [Key]
        public int TodoId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExecuted { get; set; }
        public bool Remove { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public TodoEntity()
        {
            CreatedAt = DateTime.Now;
            IsExecuted = false;
            Remove = false;
        }
    }
}
