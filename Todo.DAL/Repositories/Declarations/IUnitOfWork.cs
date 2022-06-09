namespace Todo.DAL.Repositories.Declarations
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoRepository TodoRepository { get; }
        Task<int> ExecuteCommandAsync();
    }
}
