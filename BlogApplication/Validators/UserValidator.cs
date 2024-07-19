using BlogApplication.Models;
namespace BlogApplication.Validators
{
    public static class UserValidator
    {
        public static bool HasAllAttributes(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                return false;
            }
            return true;
        }

        public static string ReturnErrorsCreate(List<User> users, User user)
        {
            string msg = "";
            if (!HasAllAttributes(user))
            {
                msg += "Fill all fields!\n";
            }
            if (user.Password.Length < 6)
            {
                msg += "Password should be at least 6 symbols";
            }
            if (users.FirstOrDefault(p => p.Username == user.Username) != null)
            {
                msg += "This username already exists in the database!\n";
            }
            if (users.FirstOrDefault(p => p.Email == user.Email) != null)
            {
                msg += "This email already exists in the database!";
            }
            return msg;
        }
        public static string ReturnErrorsEdit(List<User> users, User user, int id)
        {
            //Changed user.Id 
            string msg = "";
            if (!HasAllAttributes(user))
            {
                msg += "Fill all fields!\n";
            }
            if (user.Password.Length < 6)
            {
                msg += "Password should be at least 6 symbols";
            }
            if (users.FirstOrDefault(p => p.UserId != id && p.Username == user.Username) != null)
            {
                msg += "This username already exists in the database!\n";
            }
            return msg;
        }

   
    }
}
