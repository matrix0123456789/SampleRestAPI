using SampleRestAPI.Database.Entity;
using SampleRestAPI.Repositories;

namespace SampleRestAPI.Services
{
    public class ToDoService
    {
        private readonly ToDoRepository toDoRepository;

        public ToDoService(ToDoRepository toDoRepository)
        {
            this.toDoRepository = toDoRepository;
        }
        internal IEnumerable<ToDo> GetAll()
        {
            return toDoRepository.GetAll();
        }

        internal ToDo Insert(ToDo item)
        {
            return toDoRepository.Insert(item);
        }

        internal void Delete(int id)
        {
            toDoRepository.Delete(id);
        }

        internal ToDo? GetOne(int id)
        {
            return toDoRepository.GetOne(id);
        }

        internal IEnumerable<ToDo> GetIncoming(ToDoWhen when)
        {
            return toDoRepository.GetIncoming(when);
        }

        public enum ToDoWhen
        {
            today = 1, tommorow = 2, thisWeek = 3
        }

        internal void Update(int id, ToDo item)
        {
            item.Id = id;
            toDoRepository.Update(id, item);
        }

        internal void SetAsDone(int id)
        {
            toDoRepository.Update(id, new Dictionary<string, object> { { "Status", ToDo.ToDoStatus.Done } });
        }
        internal void SetCompletionPercentage(int id, decimal percentage)
        {
            toDoRepository.Update(id, new Dictionary<string, object> { { "Completion", percentage } });
        }
    }
}
