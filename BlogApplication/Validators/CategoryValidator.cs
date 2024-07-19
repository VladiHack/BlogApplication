using BlogApplication.Models;

namespace BlogApplication.Validators
{
    public static class CategoryValidator
    {

        public static string ReturnErrorsCreate(List<Category> categories, Category category)
        {
            string msg = "";
            if (String.IsNullOrWhiteSpace(category.Name))
            {
                msg += "Fill all fields!\n";
            }
            return msg;
        }
        public static string ReturnErrorsEdit(List<Category> categories, Category category, int id)
        {
            //Changed user.Id 
            string msg = "";
            if (String.IsNullOrWhiteSpace(category.Name))
            {
                msg += "Fill all fields!\n";
            }
            if (categories.FirstOrDefault(c => c.CategoryId != id && c.Name == category.Name) != null)
            {
                msg += "This category name already exists in the database!\n";
            }
            return msg;
        }
    }
}
