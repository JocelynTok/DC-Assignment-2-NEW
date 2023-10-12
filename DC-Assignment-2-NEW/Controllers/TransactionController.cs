using DC_Assignment_2_NEW.Data;
using DC_Assignment_2_NEW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DC_Assignment_2_NEW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Transaction> getTransactions()
        {
            List<Transaction> transactions = DBManager.GetAllTransactions();
            return transactions;
        }
        [HttpGet("{transactionID}")]
        public IActionResult GetTransaction(string transactionID)
        {
            Transaction transaction = DBManager.GetTransactionByID(transactionID);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpGet("account/{accountNo}")]
        public IEnumerable<Transaction> GetTransactionByNo(string accountNo)
        {
            List <Transaction> transactionList= DBManager.GetTransactionsByNo(accountNo);
       
            return transactionList;
        }
        [HttpPost]
        public IActionResult PostTransaction([FromBody] Transaction transaction)
        {
            if (DBManager.InsertTransaction(transaction))
            {
                return Ok("Successfully inserted");
            }
            return BadRequest("Error in data insertion");
        }
        [HttpDelete]
        [Route("{transactionID}")]
        public IActionResult DeleteTransaction(String transactionID)
        {
            if (DBManager.DeleteTransaction(transactionID))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }
        [HttpPut]
        public IActionResult UpdateTransaction(Transaction transaction)
        {
            if (DBManager.UpdateTransaction(transaction))
            {
                return Ok("Successfully updated");
            }
            return BadRequest("Could not update");
        }
    }
}
