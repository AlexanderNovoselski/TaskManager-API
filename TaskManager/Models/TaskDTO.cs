using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Enums;

namespace TaskManager.Models
{
    public class TaskDTO
    {
        [Required]
        public Guid Id { get; set; }

        public string OwnerId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public Importance ImportanceLevel { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime AddedDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
