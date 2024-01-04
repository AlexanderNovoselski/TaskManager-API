using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Enums;
using TaskManager.Models.Requests.Utils;

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
        [FutureDate(ErrorMessage = "Due date must be a future date.")]
        public DateTime DueDate { get; set; }
    }
}
