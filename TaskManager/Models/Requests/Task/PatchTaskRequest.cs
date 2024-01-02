namespace TaskManager.Models.Requests
{
    public class PatchTaskRequest
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
