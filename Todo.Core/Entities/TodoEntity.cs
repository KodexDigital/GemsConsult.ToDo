namespace Todo.Core.Entities
{
    public class TodoEntity
    {
        public int TodoId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExecuted { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public TodoEntity()
        {
            ExecutionDate = DateTime.Now;
            IsExecuted = false;
        }
    }
}
