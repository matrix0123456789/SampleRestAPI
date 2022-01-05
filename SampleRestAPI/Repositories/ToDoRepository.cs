using SampleRestAPI.Database;
using SampleRestAPI.Database.Entity;
using static SampleRestAPI.Services.ToDoService;

namespace SampleRestAPI.Repositories
{
    public class ToDoRepository : AbstractRepository<ToDo>
    {
        public ToDoRepository(AppDbContext dbContext):base(dbContext, dbContext.ToDos)
        {
        }
        internal IEnumerable<ToDo> GetIncoming(ToDoWhen when)
        {
            var today = DateTime.Today;//in case run over midnight copy to variable
            var weekStart = today.AddDays(-(int)today.DayOfWeek + 1);
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                weekStart = weekStart.AddDays(-7);
            }
            return when switch
            {
                ToDoWhen.today => dbContext.ToDos.Where(x => x.Expiration.Value.Date == today).ToArray(),
                ToDoWhen.tommorow => dbContext.ToDos.Where(x => x.Expiration.Value.Date == today.AddDays(1)).ToArray(),
                ToDoWhen.thisWeek => dbContext.ToDos.Where(x => x.Expiration >= weekStart && x.Expiration < weekStart.AddDays(7)).ToArray(),
            };
        }

    }
}
