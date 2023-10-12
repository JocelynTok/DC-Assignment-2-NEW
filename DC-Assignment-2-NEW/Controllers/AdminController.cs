using Microsoft.AspNetCore.Mvc;

namespace DC_Assignment_2_NEW.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        [HttpGet]
        [Route("profile")]
        public IActionResult GetProfileView()
        {
            return PartialView("../Admin/AdminProfile");
        }

        [Route("transactionManagement")]
        public IActionResult GetTransactionView()
        {
            return PartialView("../Admin/TransactionManagementView");
        }

        [Route("userManagement")]
        public IActionResult GetUserView()
        {
            return PartialView("../Admin/UserManagementView");
        }

    }
}
