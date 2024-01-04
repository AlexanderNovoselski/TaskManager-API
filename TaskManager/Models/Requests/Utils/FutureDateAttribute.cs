using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Requests.Utils
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.Date >= DateTime.Now.Date;
            }
            return false; // If it's not a DateTime, consider it invalid

        }
    }
}