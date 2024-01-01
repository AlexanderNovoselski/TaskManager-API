using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Enums;

namespace TaskManager.Models
{
    public class TaskForUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public Importance ImportanceLevel { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
