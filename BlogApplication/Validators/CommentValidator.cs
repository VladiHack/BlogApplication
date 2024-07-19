using BlogApplication.Models;

namespace BlogApplication.Validators
{
    public class CommentValidator
    {
 
        public static bool HasAllAttributes(Comment comment)
        {
            if (String.IsNullOrEmpty(comment.Content) )
            {
                return false;
            }
            return true;
        }

        public static string ReturnErrorsCreate(List<Comment> comments, Comment comment)
        {
            string msg = "";
            if (!HasAllAttributes(comment))
            {
                msg += "Fill all fields!\n";
            }
            else if (comment.Content.Length < 6)
            {
                msg += "Content should be at least 6 symbols";
            }
 
            return msg;
        }
        public static string ReturnErrorsEdit(List<Comment> comments, Comment comment, int id)
        {
            //Changed user.Id 
            string msg = "";
            if (!HasAllAttributes(comment))
            {
                msg += "Fill all fields!\n";
            }
            if (comment.Content.Length < 6)
            {
                msg += "Content should be at least 6 symbols";
            }
            return msg;
        }
    }
}
