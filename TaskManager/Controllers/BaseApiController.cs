using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskManager.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected string OwnerId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
