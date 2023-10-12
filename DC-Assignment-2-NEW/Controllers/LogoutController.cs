using Microsoft.AspNetCore.Mvc;

namespace DC_Assignment_2_NEW.Controllers
{

    [Route("api/[controller]")]
    public class LogoutController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            Response.Cookies.Delete("SessionID");
            Response.Cookies.Delete("userEmail");
            return PartialView("LogoutView");
        }
    }
}
