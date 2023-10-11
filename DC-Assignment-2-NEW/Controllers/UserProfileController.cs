using DC_Assignment_2_NEW.Models;
using Microsoft.AspNetCore.Mvc;
using DC_Assignment_2_NEW.Data;

namespace DC_Assignment_2_NEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<UserProfile> GetUserProfiles()
        {
            List<UserProfile> userProfiles = DBManager.GetAllUserProfiles();
            return userProfiles;
        }

        [HttpGet]
        [Route("{email}")]
        public IActionResult GetUserProfile(string email)
        {
            UserProfile userProfile = DBManager.GetUserProfileByEmail(email);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        [HttpPost]
        public IActionResult CreateUserProfile([FromBody] UserProfile userProfile)
        {
            if (DBManager.InsertUserProfile(userProfile))
            {
                return Ok("UserProfile created successfully");
            }
            return BadRequest("Error in UserProfile creation");
        }

        [HttpDelete]
        [Route("{username}")]
        public IActionResult DeleteUserProfile(string username)
        {
            if (DBManager.DeleteUserProfile(username))
            {
                return Ok("UserProfile deleted successfully");
            }
            return BadRequest("Could not delete UserProfile");
        }

        [HttpPut]
        public IActionResult UpdateUserProfile(UserProfile userProfile)
        {
            if (DBManager.UpdateUserProfile(userProfile))
            {
                return Ok("UserProfile updated successfully");
            }
            return BadRequest("Could not update UserProfile");
        }
    }

}
