namespace DC_Assignment_2_NEW.Models
{
    public class UserProfile
    {
        public string Username { get; set; } // User's username
        public string Email { get; set; } // User's email address
        public string Address { get; set; } // User's home address
        public string Phone { get; set; } // User's phone number
        public string PictureUrl { get; set; } // URL or path to the user's profile picture
        public string PasswordHash { get; set; } // Hashed password
        public string AccountNo { get; set; } // User's bank account number
        public string Roles { get; set; } // User's role (admin or employee)

    }
}
