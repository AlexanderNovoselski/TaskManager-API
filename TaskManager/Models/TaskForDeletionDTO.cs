using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskForDeletionDTO
    {
        [Required]
        public Guid Id { get; set; }
    }
}
