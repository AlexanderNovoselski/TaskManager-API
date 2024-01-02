namespace TaskManager.Models.Requests.Task
{
    public class TaskIdOwnerIdRequest
    {
        public Guid Id { get; set; }

        public string OwnerId { get; set; }
    }
}
