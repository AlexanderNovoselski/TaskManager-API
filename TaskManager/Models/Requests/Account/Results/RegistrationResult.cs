namespace TaskManager.Models.Requests.Account.Results
{
    public class RegistrationResult
    {
        public bool Succeeded { get; set; }
        public string UserId { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }
        public string[] Errors { get; set; }
    }
}
