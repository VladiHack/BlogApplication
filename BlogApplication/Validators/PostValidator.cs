using BlogApplication.Models;

namespace BlogApplication.Validators
{
    public static class PostValidator
    {

        public static bool HasAllAttributes(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Content) || post.CategoryId<=0)
            {
                return false;
            }
            return true;
        }

        public static string ReturnErrorsCreate(List<Post> posts, Post post)
        {
            string msg = "";
            if (!HasAllAttributes(post))
            {
                msg += "Fill all fields!\n";
            }
            
            if (post.Title.Length < 6)
            {
                msg += "Title should be at least 6 symbols";
            }
            return msg;
        }
        public static string ReturnErrorsEdit(List<Post> posts, Post post, int id)
        {
            //Changed user.Id 
            string msg = "";
            if (!HasAllAttributes(post))
            {
                msg += "Fill all fields!\n";
            }
            if (post.Title.Length < 6)
            {
                msg += "Title should be at least 6 symbols";
            }
            return msg;
        }
    }
}
