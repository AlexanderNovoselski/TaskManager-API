using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Enums;

namespace TaskManager.Models
{
    public class TaskForCreationDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public Importance ImportanceLevel { get; set; } = Importance.Medium;

        [Required]
        public DateTime DueDate { get; set; }
    }
}
