using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Enums;

namespace TaskManager.Data.Models
{
    public class ToDoTask
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }


        public Importance ImportanceLevel { get; set; } = Importance.Medium;
        public bool IsCompleted { get; set; } = false;

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
