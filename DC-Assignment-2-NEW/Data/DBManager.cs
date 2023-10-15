using DC_Assignment_2_NEW.Models;
using System.Data.SQLite;

namespace DC_Assignment_2_NEW.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=mydatabase.db;Version=3;";

        public static bool CreateTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // SQL command to create a table named "AccountTable"
                        command.CommandText = @"
                    CREATE TABLE AccountTable (
                        AccountNo TEXT,
                        Username TEXT,
                        Email TEXT,
                        Balance INTEGER
                    )";

                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("Table created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false; // Create table failed

        }

        public static bool CreateTransactionTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    //Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        //SQL Command to create a table named "Student Table"
                        command.CommandText = @"
                        CREATE TABLE TransactionTable (
                            TransactionID TEXT,
                            TransactionType TEXT,
                            Amount INTEGER,
                            AccountNo TEXT,
                            TransactionDate TEXT,
                            Description TEXT
                        )";

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    Console.WriteLine("Table created successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false; //create table failed
        }

        public static bool CreateUserProfileTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    //Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        //SQL Command to create a table named "UserProfileTable"
                        command.CommandText = @"
                        CREATE TABLE UserProfileTable (
                            Username TEXT,
                            Email TEXT,
                            Address TEXT,
                            Phone TEXT,
                            PictureUrl TEXT,
                            PasswordHash TEXT,
                            AccountNo TEXT,
                            Roles TEXT 
                        )";

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    Console.WriteLine("Table created successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return false; //create table failed
        }

        public static bool Insert(Account account)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // SQL command to insert data into the "StudentTable" table
                        command.CommandText = @"
                        INSERT INTO AccountTable (AccountNo, Username, Email, Balance)
                        VALUES (@AccountNo, @Username, @Email , @Balance)";

                        // Define parameters for the query
                        command.Parameters.AddWithValue("@AccountNo", account.AccountNo);
                        command.Parameters.AddWithValue("@Username", account.Username);
                        command.Parameters.AddWithValue("@Email", account.Email);
                        command.Parameters.AddWithValue("@Balance", account.Balance);

                        // Execute the SQL command to insert data
                        int rowsInserted = command.ExecuteNonQuery();

                        // Check if any rows were inserted
                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true; // Insertion was successful
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false; // Insertion failed
        }

        public static bool InsertTransaction(Transaction transaction)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO TransactionTable (TransactionID, TransactionType, Amount, AccountNo, TransactionDate, Description)
                        VALUES (@TransactionID, @TransactionType, @Amount, @AccountNo, @TransactionDate, @Description)
                        ";

                        //define transaction parameters
                        command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
                        command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@AccountNo", transaction.AccountNo);
                        //String formattedDate = transaction.TransactionDate.ToString("MM/dd/yyyy HH:mm:ss");
                        command.Parameters.AddWithValue("@TransactionDate", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

                        command.Parameters.AddWithValue("@Description", transaction.Description);

                        int rowsInserted = command.ExecuteNonQuery();

                        //check if any rows were inserted
                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            // Update the account's balance
                            UpdateAccountBalance(transaction.AccountNo, transaction.TransactionType, transaction.Amount);
                            return true; //insertion was successful
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error transaction: " + ex.Message);
            }
            return false; //insert transaction fail
        }

        public static bool Delete(string accountNo)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to delete data by ID
                        command.CommandText = $"DELETE FROM AccountTable WHERE AccountNo = @AccountNo";
                        command.Parameters.AddWithValue("@AccountNo", accountNo);

                        // Execute the SQL command to delete data
                        int rowsDeleted = command.ExecuteNonQuery();

                        // Check if any rows were deleted
                        connection.Close();
                        if (rowsDeleted > 0)
                        {

                            return true; // Deletion was successful
                        }
                    }
                    connection.Close();
                }

                return false; // No rows were deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Deletion failed
            }
        }

        public static bool DeleteTransaction(string transactionID)
        {
            try
            {
                // Retrieve the transaction details using the GetTransactionByID method
                Transaction transaction = GetTransactionByID(transactionID);

                if (transaction != null)
                {
                    string accountNo = transaction.AccountNo;
                    int amount = transaction.Amount;
                    string transactionType = transaction.TransactionType;

                    // Delete the transaction
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            // Build the SQL command to delete the transaction by ID
                            command.CommandText = $"DELETE FROM TransactionTable WHERE TransactionID = @TransactionID";
                            command.Parameters.AddWithValue("@TransactionID", transactionID);

                            // Execute the SQL command to delete the transaction
                            int rowsDeleted = command.ExecuteNonQuery();

                            if (rowsDeleted > 0)
                            {
                                // Update the account balance based on the deleted transaction type
                                if (transactionType == "DEPOSIT")
                                {
                                    // If it was a deposit, subtract the amount
                                    UpdateAccountBalance(accountNo, "DEPOSIT", -amount);
                                }
                                else if (transactionType == "WITHDRAW")
                                {
                                    // If it was a withdrawal, add back the amount
                                    UpdateAccountBalance(accountNo, "WITHDRAW", -amount);
                                }

                                return true; // Deletion was successful
                            }
                        }

                        connection.Close();
                    }
                }

                return false; // No rows were deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting transaction: " + ex.Message);
                return false; // Deletion failed
            }
        }

        public static bool Update(Account account)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to update data by ID
                        command.CommandText = $"UPDATE AccountTable SET Username = @Username, Email = @Email , Balance = @Balance WHERE AccountNo = @AccountNo";
                        command.Parameters.AddWithValue("@AccountNo", account.AccountNo);
                        command.Parameters.AddWithValue("@Username", account.Username);
                        command.Parameters.AddWithValue("@Email", account.Email);
                        command.Parameters.AddWithValue("@Balance", account.Balance);

                        // Execute the SQL command to update data
                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();
                        // Check if any rows were updated
                        if (rowsUpdated > 0)
                        {
                            return true; // Update was successful
                        }
                    }
                    connection.Close();
                }

                return false; // No rows were updated
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Update failed
            }
        }

        public static bool UpdateTransaction(Transaction transaction)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve the old transaction information to calculate the change in amount and type
                    Transaction oldTransaction = GetTransactionByID(transaction.TransactionID);

                    if (oldTransaction != null)
                    {
                        int oldAmount = oldTransaction.Amount;
                        string oldTransactionType = oldTransaction.TransactionType;

                        // Build the SQL command to update data by ID
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE TransactionTable SET TransactionType = @TransactionType, Amount = @Amount, TransactionDate = @TransactionDate, Description = @Description WHERE TransactionID = @TransactionID";
                            command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
                            command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                            command.Parameters.AddWithValue("@Amount", transaction.Amount);
                            command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
                            command.Parameters.AddWithValue("@Description", transaction.Description);
                           

                            // Execute the SQL command to update data
                            int rowsUpdated = command.ExecuteNonQuery();
                            connection.Close();

                            // Check if any rows were updated
                            if (rowsUpdated > 0)
                            {
                                // Calculate the change in amount
                                int amountChange = transaction.Amount - oldAmount;

                                // Calculate the change in the transaction type
                                if (transaction.TransactionType != oldTransactionType)
                                {
                                    if (transaction.TransactionType == "DEPOSIT")
                                    {
                                        Console.WriteLine("Old Amount :" + oldAmount);
                                        Console.WriteLine("New Amount :" + transaction.Amount);
                                        amountChange = oldAmount + transaction.Amount;
                                    }
                                    else if (transaction.TransactionType == "WITHDRAW")
                                    {
                                        amountChange -= oldAmount;
                                    }
                                }

                                // Update the account balance based on the amount change
                                UpdateAccountBalance(transaction.AccountNo, transaction.TransactionType, amountChange);

                                return true; // Update was successful
                            }
                        }
                    }
                }

                return false; // No rows were updated
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating transaction: " + ex.Message);
                return false; // Update failed
            }
        }

        public static int GetTransactionAmount(string transactionID)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select the transaction by ID
                        command.CommandText = "SELECT Amount FROM TransactionTable WHERE TransactionID = @TransactionID";
                        command.Parameters.AddWithValue("@TransactionID", transactionID);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve the amount from the transaction
                                return Convert.ToInt32(reader["Amount"]);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving transaction amount: " + ex.Message);
            }

            return 0; // Return 0 if the transaction doesn't exist or if there's an error
        }
        private static void UpdateAccountBalance(string accountNo, string transactionType, int amount)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        UPDATE AccountTable
                        SET Balance = Balance + @Amount
                        WHERE AccountNo = @AccountNo";

                        if (transactionType == "WITHDRAW")
                        {
                            // For withdrawals, subtract the amount from the balance
                            command.CommandText = @"
                            UPDATE AccountTable
                            SET Balance = Balance - @Amount
                            WHERE AccountNo = @AccountNo";
                        }

                        command.Parameters.AddWithValue("@AccountNo", accountNo);
                        command.Parameters.AddWithValue("@Amount", amount);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating account balance: " + ex.Message);
            }
        }

        public static List<Account> GetAll()
        {
            List<Account> accountList = new List<Account>();

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select all student data
                        command.CommandText = "SELECT * FROM AccountTable";

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account account = new Account();
                                account.AccountNo = reader["AccountNo"].ToString();
                                account.Username = reader["Username"].ToString();
                                account.Email = reader["Email"].ToString();
                                account.Balance = Convert.ToInt32(reader["Balance"]);

                                // Create a Student object and add it to the list
                                accountList.Add(account);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return accountList;
        }

        public static List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactionList = new List<Transaction>();

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select all student data
                        command.CommandText = "SELECT * FROM TransactionTable";

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction();
                                transaction.TransactionID = reader["TransactionID"].ToString();
                                transaction.TransactionType = reader["TransactionType"].ToString();
                                transaction.Amount = Convert.ToInt32(reader["Amount"]);
                                transaction.AccountNo = reader["AccountNo"].ToString();
                                transaction.TransactionDate = reader["TransactionDate"].ToString();
                                transaction.Description = reader["Description"].ToString();

                                // Create a Student object and add it to the list
                                transactionList.Add(transaction);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving transaction list: " + ex.Message);
            }

            return transactionList;
        }

        public static Account GetByNo(string accountNo)
        {
            Account account = null;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select a student by ID
                        command.CommandText = "SELECT * FROM AccountTable WHERE AccountNo = @AccountNo";
                        command.Parameters.AddWithValue("@AccountNo", accountNo);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account = new Account();
                                account.AccountNo = reader["AccountNo"].ToString();
                                account.Username = reader["Username"].ToString();
                                account.Email = reader["Email"].ToString();
                                account.Balance = Convert.ToInt32(reader["Balance"]);

                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return account;
        }

        public static Transaction GetTransactionByID(string transactionID)
        {
            Transaction transaction = null;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select a student by ID
                        command.CommandText = "SELECT * FROM TransactionTable WHERE TransactionID = @TransactionID";
                        command.Parameters.AddWithValue("@TransactionID", transactionID);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                transaction = new Transaction();
                                transaction.TransactionID = reader["TransactionID"].ToString();
                                transaction.TransactionType = reader["TransactionType"].ToString();
                                transaction.Amount = Convert.ToInt32(reader["Amount"]);
                                transaction.AccountNo = reader["AccountNo"].ToString();
                                transaction.TransactionDate = reader["TransactionDate"].ToString();
                                transaction.Description = reader["Description"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retriving transaction: " + ex.Message);
            }

            return transaction;
        }

        public static List<Transaction> GetTransactionsByNo(string accountNo)
        {
            List<Transaction> transactionList = new List<Transaction>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM TransactionTable WHERE AccountNo = @AccountNo";
                        command.Parameters.AddWithValue("@AccountNo", accountNo);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction
                                {
                                    AccountNo = reader["AccountNo"].ToString(),
                                    TransactionID = reader["TransactionID"].ToString(),
                                    TransactionType = reader["TransactionType"].ToString(),
                                    Amount = Convert.ToInt32(reader["Amount"]),
                                    TransactionDate = reader["TransactionDate"].ToString(),
                                    Description = reader["Description"].ToString()

                                };
                           

                                transactionList.Add(transaction);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving transactions: " + ex.Message);
            }

            return transactionList;
        }

        public static bool InsertUserProfile(UserProfile userProfile)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO UserProfileTable (Username, Email, Address, Phone, PictureUrl, PasswordHash, AccountNo, Roles)
                        VALUES (@Username, @Email, @Address, @Phone, @PictureUrl, @PasswordHash, @AccountNo, @Roles)
                        ";

                        //define user profile parameters
                        command.Parameters.AddWithValue("@Username", userProfile.Username);
                        command.Parameters.AddWithValue("@Email", userProfile.Email);
                        command.Parameters.AddWithValue("@Address", userProfile.Address);
                        command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                        command.Parameters.AddWithValue("@PictureUrl", userProfile.PictureUrl);
                        command.Parameters.AddWithValue("@PasswordHash", userProfile.PasswordHash);
                        command.Parameters.AddWithValue("@AccountNo", userProfile.AccountNo);
                        command.Parameters.AddWithValue("@Roles", userProfile.Roles);

                        int rowsInserted = command.ExecuteNonQuery();

                        //check if any rows were inserted
                        connection.Close();
                        if (rowsInserted > 0)
                        {
                            return true; //insertion was successful
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with user profile: " + ex.Message);
            }
            return false; //insert transaction fail
        }

        // Method to retrieve a user profile by email
        public static UserProfile GetUserProfileByEmail(string email)
        {
            UserProfile userProfile = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM UserProfileTable WHERE Email = @Email";
                        command.Parameters.AddWithValue("@Email", email);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userProfile = new UserProfile();
                                userProfile.Username = reader["Username"].ToString();
                                userProfile.Email = reader["Email"].ToString();
                                userProfile.Address = reader["Address"].ToString();
                                userProfile.Phone = reader["Phone"].ToString();
                                userProfile.PictureUrl = reader["PictureUrl"].ToString();
                                userProfile.PasswordHash = reader["PasswordHash"].ToString();
                                userProfile.AccountNo = reader["AccountNo"].ToString();
                                userProfile.Roles = reader["Roles"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving user profile: " + ex.Message);
            }

            return userProfile;
        }

        public static List<UserProfile> GetAllUserProfiles()
        {
            List<UserProfile> userProfiles = new List<UserProfile>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM UserProfileTable";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserProfile userProfile = new UserProfile
                                {
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    PictureUrl = reader["PictureUrl"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    AccountNo = reader["AccountNo"].ToString(),
                                    Roles = reader["Roles"].ToString()
                                };

                                userProfiles.Add(userProfile);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving user profiles: " + ex.Message);
            }

            return userProfiles;
        }

        public static bool DeleteUserProfile(string username)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM UserProfileTable WHERE Username = @Username";
                        command.Parameters.AddWithValue("@Username", username);

                        int rowsDeleted = command.ExecuteNonQuery();

                        connection.Close();

                        if (rowsDeleted > 0)
                        {
                            return true; // Deletion was successful
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting user profile: " + ex.Message);
            }

            return false; // Deletion failed
        }

        public static bool UpdateUserProfile(UserProfile userProfile)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                    UPDATE UserProfileTable
                    SET Email = @Email, Address = @Address, Phone = @Phone, PictureUrl = @PictureUrl, PasswordHash = @PasswordHash, AccountNo = @AccountNo, Roles = @Roles
                    WHERE Username = @Username
                ";

                        command.Parameters.AddWithValue("@Email", userProfile.Email);
                        command.Parameters.AddWithValue("@Address", userProfile.Address);
                        command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                        command.Parameters.AddWithValue("@PictureUrl", userProfile.PictureUrl);
                        command.Parameters.AddWithValue("@PasswordHash", userProfile.PasswordHash);
                        command.Parameters.AddWithValue("@AccountNo", userProfile.AccountNo);
                        command.Parameters.AddWithValue("@Roles", userProfile.Roles);
                        command.Parameters.AddWithValue("@Username", userProfile.Username);

                        int rowsUpdated = command.ExecuteNonQuery();

                        connection.Close();

                        if (rowsUpdated > 0)
                        {
                            return true; // Update was successful
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user profile: " + ex.Message);
            }

            return false; // Update failed
        }

        public static string GetCurrentAdminByRole()
        {
            // Replace this logic with your actual implementation
            // Retrieve the current admin's ID based on their role
            string currentAdminUsername = "";

            // Example implementation:
            // Assuming you have a list of UserProfile objects representing admins
            // and each UserProfile object has a "Roles" property indicating their role
            // You can iterate through the list and find the admin with the desired role

            // Replace "adminList" with your actual list of UserProfile objects representing admins
            List<UserProfile> adminList = GetAllUserProfiles();

            foreach (var admin in adminList)
            {
                if (admin.Roles == "admin")
                {
                    currentAdminUsername = admin.Username; // You can change this to the appropriate ID property of your UserProfile model
                    break;
                }
            }

            return currentAdminUsername;
        }

        public static void DBInitialize()
        {
            DBGenerator generator = new DBGenerator();
            int initialAccountNo = 20;
            //change this to change the amount of initial accounts created

            if (CreateTable())
            {
                int balance = 0;
                string accountNumber = "", username = "", email = "";

                //setting admin accounts
                Account adminAccount = new Account();
                generator.GetNextAccount(out accountNumber, out username, out email, out balance);
                adminAccount.AccountNo = accountNumber;
                adminAccount.Username = "Shu Man";
                adminAccount.Email = "shuman_admin@gmail.com";
                adminAccount.Balance = balance;
                Insert(adminAccount);

                adminAccount = new Account();
                generator.GetNextAccount(out accountNumber, out username, out email, out balance);
                adminAccount.AccountNo = accountNumber;
                adminAccount.Username = "JasmineC";
                adminAccount.Email = "jasmine_admin@gmail.com";
                adminAccount.Balance = balance;
                Insert(adminAccount);

                adminAccount = new Account();
                generator.GetNextAccount(out accountNumber, out username, out email, out balance);
                adminAccount.AccountNo = accountNumber;
                adminAccount.Username = "Jia Yi";
                adminAccount.Email = "jiayi_admin@gmail.com";
                adminAccount.Balance = balance;
                Insert(adminAccount);


                // Inserting random user accounts
                Account account;
                for (int i = 0; i < initialAccountNo; i++)
                {
                    account = new Account();
                    generator.GetNextAccount(out accountNumber, out username, out email, out balance);
                    account.AccountNo = accountNumber;
                    account.Username = username;
                    account.Email = email;
                    account.Balance = balance;

                    //Console.WriteLine(account.AccountToString());
                    Insert(account);
                }



            }


            if (CreateUserProfileTable())
            {
                UserProfile userProfile = new UserProfile();

                List<Account> accounts = GetAll();
                foreach (Account account in accounts)
                {
                    string accountNo = account.AccountNo;
                    //we set the 3 initial accounts of admin to admin
                    if (accountNo.Equals("100000001") || accountNo.Equals("100000002") || accountNo.Equals("100000003"))
                    {
                        UserProfile user = generator.GetNextUserProfile(account);
                        user.PasswordHash = "1234";
                        user.Roles = "admin";
                        InsertUserProfile(user);
                    }
                    else
                    {

                        //for each account, we create the rest of the profile based on that account and insert it into the db
                        UserProfile user = generator.GetNextUserProfile(account);
                        //Console.WriteLine(user.ProfileToString());
                        InsertUserProfile(user);
                    }
                }

                
 
            }

            
            if (CreateTransactionTable())
            {
                Transaction transaction = new Transaction();

                List<Account> accounts = GetAll();
                foreach (Account account in accounts)
                {
                    //create a transaction for each account
                    string transactionID = "", transactionType = "", accountNo = "";
                    int amount = 0;
                    
                    //for each account, we create the rest of the profile based on that account and insert it into the db
                    generator.GetNextTransaction(out transactionID, out transactionType, out amount);

                    transaction.TransactionID = transactionID;
                    transaction.TransactionType = transactionType;
                    transaction.Amount = amount;
                    transaction.AccountNo = account.AccountNo;
                    transaction.TransactionDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    transaction.Description = "Pre-generated Transaction Example";

                    //Console.WriteLine(user.ProfileToString());
                    InsertTransaction(transaction);
                }

            }
            

        }

    }
}
