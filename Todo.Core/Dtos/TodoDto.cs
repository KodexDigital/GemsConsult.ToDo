namespace Todo.Core.Dtos
{
    public class TodoDto
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string UserId { get; set; }
    }
    
    public class EditTodoDto
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public DateTime ExecutionDate { get; set; }
        public int ItemId { get; set; }
    }
}
