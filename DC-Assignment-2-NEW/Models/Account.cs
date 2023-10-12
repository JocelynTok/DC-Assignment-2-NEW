namespace DC_Assignment_2_NEW.Models
{
    public class Account
    {
        public string AccountNo { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Balance { get; set; }

        public Account()
        {
            AccountNo = "";
            Username = "";
            Email = "";
            Balance = 0;
        }

        public String AccountToString()
        {
            string prompt = "Account Number: " + AccountNo +
                "\nUsername: " + Username +
                "\nEmail: " + Email +
                "\nBalance: " + Balance + "\n";
            return prompt;
        }

    }


}
