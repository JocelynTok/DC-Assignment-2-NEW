using DC_Assignment_2_NEW.Data;
using DC_Assignment_2_NEW.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DC_Assignment_2_NEW.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [HttpGet("defaultview")]
        public IActionResult GetDefaultView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("../Admin/AdminPanel");
                }
                else if(cookieValue== "2234567")
                {
                    return PartialView("../User/UserPanel");
                }

            }
            // Return the partial view as HTML
            return PartialView("LoginView");
        }

        [HttpGet("authview")]
        public IActionResult GetLoginAuthenticatedView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("../Admin/AdminPanel");
                }
                else if (cookieValue == "2234567")
                {
                    return PartialView("../User/UserPanel");
                }

            }
            // Return the partial view as HTML
            return PartialView("LoginFail");
        }

        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("LoginFail");
        }

        
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] UserProfile user)
        {
            // Return the partial view as HTML
            var response = new { login = false };


            UserProfile userProfile = DBManager.GetUserProfileByEmail(user.Email);

            if (userProfile!= null && userProfile.PasswordHash == user.PasswordHash)
            {
                if(userProfile.Roles.Equals("admin"))
                {
                    Response.Cookies.Append("SessionID", "1234567");
                    response = new { login = true };
                }
                else
                {
                    Response.Cookies.Append("SessionID", "2234567");
                    response = new { login = true };
                }
            }

            // Password doesn't match
           // return BadRequest("Invalid password");
        

            return Json(response);

        }
        
    }
}
