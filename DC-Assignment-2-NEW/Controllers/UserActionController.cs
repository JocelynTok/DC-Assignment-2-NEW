using Microsoft.AspNetCore.Mvc;

namespace DC_Assignment_2_NEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActionController:Controller
    {
        [HttpGet]
        [Route("profile")]
        public IActionResult GetProfileView()
        {
            return PartialView("../User/Profile");
        }

        [HttpGet]
        [Route("history")]
        public IActionResult GetHistoryView()
        {
            return PartialView("../User/History");
        }
    }
}
