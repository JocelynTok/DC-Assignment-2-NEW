using Microsoft.AspNetCore.Mvc;

namespace DC_Assignment_2_NEW.Controllers
{
    [Route("api/logfile")]
    [ApiController]
    public class LogFileController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLogFileContents()
        {
            try
            {
                // Read the log file contents
                string logFilePath = "audit_log.txt"; // Replace with your log file path
                string logText = System.IO.File.ReadAllText(logFilePath);

                return Ok(logText);
            }
            catch (Exception ex)
            {
                return BadRequest("Error reading log file: " + ex.Message);
            }
        }
    }
}
