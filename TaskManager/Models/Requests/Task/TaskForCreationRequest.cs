using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Enums;

namespace TaskManager.Models.Requests
{
    public class TaskForCreationRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = "-";

        public string ImportanceLevel { get; set; } = "Medium";

        [Required]
        public DateTime DueDate { get; set; }
    }
}
