using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;
using System.Text;
using Todo.DAL.Data;
using Todo.DAL.Repositories.Declarations;

namespace Todo.DAL.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoDbContext context;
        public UnitOfWork(TodoDbContext context) { this.context = context; }

        public ITodoRepository TodoRepository => new TodoRepository(context);

        public async Task<int> ExecuteCommandAsync()
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                int result = 0;
                var strategy = context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    result = await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                });

                return result;
            }
            catch (DbEntityValidationException dbvex)
            {
                var outputLines = new StringBuilder();

                foreach (var eve in dbvex.EntityValidationErrors)
                {
                    outputLines.AppendLine(
                        $"{DateTime.Now}: Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors: ");

                    foreach (var ve in eve.ValidationErrors)
                        outputLines.AppendLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                }

                transaction.Rollback();
                throw new DbEntityValidationException(outputLines.ToString(), dbvex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
