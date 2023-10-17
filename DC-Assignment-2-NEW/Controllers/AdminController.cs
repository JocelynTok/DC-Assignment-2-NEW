using DC_Assignment_2_NEW.Logging;
using Microsoft.AspNetCore.Mvc;
using DC_Assignment_2_NEW.Data;
using DC_Assignment_2_NEW.Models;

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
        [Route("adminManagement")]
        public IActionResult GetAdminView()
        {
            return PartialView("../Admin/AdminManagementView");
        }

        [Route("activityLogs")]
        public IActionResult GetLogView()
        {
            return PartialView("../Admin/ActivityLogView");
        }

        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("admincreateUser")]
        public IActionResult CreateUser(UserProfile newUser)
        {
            // Perform create user logic

            // Log the admin activity
            string adminUsername = DBManager.GetCurrentAdminByRole();
            AuditLogger.LogActivity(adminUsername, $"Created user {newUser.Username}");

            return Ok();
        }

        [HttpPost]
        [Route("adminupdateUser")]
        public IActionResult UpdateUser(UserProfile updatedUser)
        {
            // Perform update user logic

            // Log the admin activity
            string adminUsername = DBManager.GetCurrentAdminByRole();
            AuditLogger.LogActivity(adminUsername, $"Updated user {updatedUser.Username}");

            return Ok();
        }

        [HttpPost]
        [Route("admindeleteUser")]
        public IActionResult DeleteUser(string username)
        {
            // Perform delete user logic

            // Log the admin activity
            string adminUsername = DBManager.GetCurrentAdminByRole();
            AuditLogger.LogActivity(adminUsername, $"Deleted user {username}");

            return Ok();
        }

        [HttpPost]
        [Route("adminupdateTransaction")]
        public IActionResult updateTransaction(string transactionID)
        {
            // Perform create user logic

            // Log the admin activity
            string adminUsername = DBManager.GetCurrentAdminByRole();
            AuditLogger.LogActivity(adminUsername, $"Updated Transaction ID {transactionID}");

            return Ok();
        }

        [HttpPost]
        [Route("admindeleteTransaction")]
        public IActionResult deleteTransaction(string transactionID)
        {
            // Perform update user logic

            // Log the admin activity
            string adminUsername = DBManager.GetCurrentAdminByRole();
            AuditLogger.LogActivity(adminUsername, $"Deleted transaction {transactionID}");

            return Ok();
        }
    }
}
