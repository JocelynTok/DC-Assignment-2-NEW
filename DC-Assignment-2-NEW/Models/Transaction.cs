namespace DC_Assignment_2_NEW.Models
{
    public class Transaction
    {
        public string TransactionID { get; set; }
        public string TransactionType { get; set; } //type: deposit or withdrawal
        public int Amount { get; set; }
        public string AccountNo { get; set; }
    }
}
