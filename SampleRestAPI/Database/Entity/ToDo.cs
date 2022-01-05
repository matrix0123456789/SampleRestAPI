using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SampleRestAPI.Database.Entity
{
    public class ToDo : IIdEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Expiration { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Completion { get; set; }
        public ToDoStatus Status { get; set; }
        public enum ToDoStatus
        {
            Waiting = 0, Done = 1
        }
    }
}
