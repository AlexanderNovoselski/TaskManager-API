using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Enums;

namespace TaskManager.Models.Requests
{
    public class TaskForUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string ImportanceLevel { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
