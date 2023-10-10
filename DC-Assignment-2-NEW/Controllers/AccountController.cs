using DC_Assignment_2_NEW.Data;
using DC_Assignment_2_NEW.Models;
using Microsoft.AspNetCore.Mvc;

namespace DC_Assignment_2_NEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Account> getAccounts()
        {
            List<Account> account = DBManager.GetAll();
            return account;
        }
        [HttpGet]
        [Route("{accountNo}")]
        public IActionResult Get(String accountNo)
        {
            Account account = DBManager.GetByNo(accountNo);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Account account)
        {
            if (DBManager.Insert(account))
            {
                return Ok("Successfully inserted");
            }
            return BadRequest("Error in data insertion");
        }
        [HttpDelete]
        [Route("{accountNo}")]
        public IActionResult Delete(String accountNo)
        {
            if (DBManager.Delete(accountNo))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }
        [HttpPut]
        public IActionResult Update(Account account)
        {
            if (DBManager.Update(account))
            {
                return Ok("Successfully updated");
            }
            return BadRequest("Could not update");
        }
    }
}
