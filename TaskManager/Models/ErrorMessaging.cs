namespace TaskManager.Models
{
    public class TaskManagerException : Exception
    {
        public TaskManagerException() { }

        public TaskManagerException(string message) : base(message) { }

        public TaskManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
