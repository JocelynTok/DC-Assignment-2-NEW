using DC_Assignment_2_NEW.Models;
using System.Data.SQLite;
using System.Transactions;
using Transaction = DC_Assignment_2_NEW.Models.Transaction;

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
                    using(SQLiteCommand command = connection.CreateCommand())
                    {
                        //SQL Command to create a table named "Student Table"
                        command.CommandText = @"
                        CREATE TABLE TransactionTable (
                            TransactionID TEXT,
                            TransactionType TEXT,
                            Amount INTEGER,
                            AccountNo TEXT
                        )";

                       command.ExecuteNonQuery();
                        connection.Close();
                    }
                    Console.WriteLine("Table created successfully.");
                    return true;
                }
            }
            catch(Exception ex)
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
                using(SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using(SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO TransactionTable (TransactionID, TransactionType, Amount, AccountNo)
                        VALUES (@TransactionID, @TransactionType, @Amount, @AccountNo)
                        ";

                        //define transaction parameters
                        command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
                        command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@AccountNo", transaction.AccountNo);

                        int rowsInserted = command.ExecuteNonQuery();

                        //check if any rows were inserted
                        connection.Close();
                        if(rowsInserted > 0)
                        {
                            return true; //insertion was successful
                        }
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
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
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to delete data by ID
                        command.CommandText = $"DELETE FROM TransactionTable WHERE TransactionID = @TransactionID";
                        command.Parameters.AddWithValue("@TransactionID", transactionID);

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

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to update data by ID
                        command.CommandText = $"UPDATE TransactionTable SET AccountNo = @AccountNo, TransactionType = @TransactionType , Amount = @Amount WHERE TransactionID = @TransactionID";
                        command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
                        command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@AccountNo", transaction.AccountNo);

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
                Console.WriteLine("Error updating transaction: " + ex.Message);
                return false; // Update failed
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

        public static void DBInitialize()
        {
            if (CreateTable())
            {
                Account account = new Account();
                account.AccountNo = "123456789";
                account.Username = "Shu Man";
                account.Email = "shuman@gmail.com";
                account.Balance = 1300;

                Insert(account);

                account = new Account();
                account.AccountNo = "223456789";
                account.Username = "Jasmine";
                account.Email = "jasmine@gmail.com";
                account.Balance = 1400;

                Insert(account);

                account = new Account();
                account.AccountNo = "323456789";
                account.Username = "Jia Yi";
                account.Email = "jiayi@gmail.com";
                account.Balance = 1500;

                Insert(account);

            }

            if (CreateTransactionTable())
            {

                Transaction transaction = new Transaction();
                transaction.TransactionID = "1";
                transaction.TransactionType = "DEPOSIT";
                transaction.Amount = 10000000;
                transaction.AccountNo = "123456789";

                InsertTransaction(transaction);

                Transaction transaction1 = new Transaction();
                transaction1.TransactionID = "2";
                transaction1.TransactionType = "DEPOSIT";
                transaction1.Amount = 12452;
                transaction1.AccountNo = "223456789";

                InsertTransaction(transaction);

                transaction = new Transaction();
                transaction.TransactionID = "3";
                transaction.TransactionType = "WITHDRAW";
                transaction.Amount = 1400;
                transaction.AccountNo = "323456789";

                InsertTransaction(transaction);

                

            }

            Transaction transaction2 = new Transaction();
            transaction2.TransactionID = "4";
            transaction2.TransactionType = "DEPOSIT";
            transaction2.Amount = 100;
            transaction2.AccountNo = "323456789";
            //InsertTransaction(transaction2);

            //DeleteTransaction("4");
            transaction2.Amount = 500;
            UpdateTransaction(transaction2);
        }
    }
}
