using DC_Assignment_2_NEW.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DC_Assignment_2_NEW.Data
{
    public class DBGenerator
    {
        private int STARTING_BAL_LIMIT = 1000000;

        private Random random;
        private int accountNoCount;
        private int transactionIDcount;
        

        public DBGenerator()
        {
            random = new Random();
            accountNoCount = 99;
            transactionIDcount = 0;
        }

        // ---------------Account Use ------------------------

        private string NextAccountNo()
        {
            accountNoCount++;
            return accountNoCount.ToString();
        }

        private string RandomName()
        {
            string[] nameList = { "Jia Yi", "Shu Man", "Jasmine", "Lim", "Tok", "Chieng", "Anastasha", "Adam", "Ali", "Abu" , "Benjamin", "Annie", "Curtin", "Minho"};
            int randIndex = random.Next(0, nameList.Length);
            return nameList[randIndex];
        }

        private string RandomEmailExtension()
        {
            string[] extensionList = { "@hotmail.com", "@gmail.com", "@yahoo.com", "@student.curtin.edu.my", "@student.curtin.edu.au" };
            int randIndex = random.Next(0, extensionList.Length);
            return extensionList[randIndex];
        }

        private int RandomBalance()
        {
            return random.Next(0, STARTING_BAL_LIMIT);
        }

        public void GetNextAccount(out string accountNo, out string username, out string accountEmail, out int balance)
        {
            accountNo = NextAccountNo();
            username = RandomName();
            string tempEmail = username + RandomEmailExtension();
            accountEmail = String.Concat(tempEmail.Where(c => !Char.IsWhiteSpace(c)));
            balance = RandomBalance();
        }

        // -- user profile use
        private string GetAddress()
        {
            string[] addressList = { "Miri", "Sibu", "Bintulu", "Brunei", "Kuala Lumpur", "Sabah", "Marudi", "Sarikei", "Spaoh", "Betong" };
            int randIndex = random.Next(0, addressList.Length);
            return addressList[randIndex];
        }

        private string GetPhoneNumber()
        {
            int phoneNo = random.Next(10000000, 99999999);
            string phoneNumber = "601" + phoneNo.ToString();
            return phoneNumber;
        }

        private string GetPicture()
        {
            List<string> album = new List<string>();
            album.Add("https://www.pexels.com/photo/turned-on-bokeh-light-370799/");
            album.Add("https://www.pexels.com/photo/yellow-bokeh-photo-949587/");
            album.Add("https://www.pexels.com/photo/defocused-image-of-lights-255379/");
        
            int randIndex = random.Next(0, album.Count);
            return album[randIndex];
        }

        private string GetPassword()
        {
            int password = random.Next(0, 9999);
            string passwordString = password.ToString();
            passwordString = String.Format("{0:0000}", password);
            return passwordString;
        }

        private string GetRole()
        {
            string[] rolesList = { "admin", "user"};
            int randIndex = random.Next(0, rolesList.Length);
            return rolesList[randIndex];
        }

        //the reason we take a account is bc we want the user profile to be connected to the account
        public UserProfile GetNextUserProfile(Account account)
        {
            UserProfile user = new UserProfile();
            user.Username = account.Username;
            user.Email = account.Email;
            user.Address = GetAddress();
            user.Phone = GetPhoneNumber();
            user.PictureUrl = GetPicture();
            user.PasswordHash = GetPassword();
            user.AccountNo = account.AccountNo;
            user.Roles = GetRole();

            return user;
        }

        // ---------- for transactions random
        private string GetNextTransactionID()
        {
            transactionIDcount++;
            return transactionIDcount.ToString();
        }

        private string GetTransactionType()
        {
            string[] transactionList = { "DEPOSIT", "WITHDRAW" };
            int randIndex = random.Next(0, transactionList.Length);
            return transactionList[randIndex];
        }

        private int GetAmount()
        {
            return random.Next(0, 10000);
        }

        public void GetNextTransaction(out string transactionID, out string transactionType, out int amount)
        {
            transactionID = GetNextTransactionID();
            transactionType = GetTransactionType();
            amount = GetAmount();
        }

    }
}
