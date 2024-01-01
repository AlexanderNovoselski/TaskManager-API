namespace TaskManager.Models.Requests
{
    public class TaskIdOwnerIdRequest
    {
        public Guid Id { get; set; }

        public string OwnerId { get; set; }
    }
}
