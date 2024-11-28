using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Common.Helpers
{
    public class Messages
    {
        // General
        public const string Error = "An Error Occured";

        // User
        public const string InvalidCredential = "Invalid email id or password";
        public const string InvalidEmailId = "Email id is not valid.";
        public const string FirstNameRequired = "First name is required";
        public const string LastNameRequired = "Last name is required";
        public const string GetUserProfileSuccess = "Fetch user profile derails successfully";
        public const string GetUserProfileError = "There is an error occured while fetching user profile details.";
        public const string UpdateUserProfileSuccess = "User profile updated successfully.";
        public const string UpdateUserProfileError = "There is an error occured while updating an user profile!";
        public const string GetUsersSuccess = "Fetch users successfully";
        public const string AddUserSuccess = "User added successfully";
        public const string AddUserError = "There is an error occured while adding the user!";
        public const string UserExist = "User alredy exist with given email, Please try with another email.";
        public const string UpdateUserSuccess = "User updated successfully";
        public const string DeleteUserSuccess = "User deleted successfully";
        public const string DeleteMultiUserSuccess = "Selected users deleted successfully";
        public const string DeleteUserError = "There is an error occured while deleting the user";

        //Login
        public const string LoginSuccess = "Logged in successfully";
        public const string LoginError = "Invalid username or password";
        public const string LoginIssue = "Something went wrong while login with given user!";

        //Reset Passwors
        public const string ResetPasswordMailSuccess = "Reset password mail send successfully to your register email.";
        public const string UserNotFound = "There is no user found with procided data.";
        public const string ChangePasswordSuccess = "User's password change successfully";
        public const string ChangePasswordError = "There is an error occured while update an user password!";

        //Dashboard
        public const string DashbodrdDataSuccess = "Fetch dashboard data sccessfully";
        public const string DashbodrdDataError = "There is an error while fetching the dashboard data!";

        //Product
        public const string AddProductSuccess = "Product saved successfully";
        public const string AddProductError = "There is an error occured while adding the product.";
        public const string UpdateProductSuccess = "Product updated successfully";
        public const string UpdateProductError = "There is an error occured while updating the product.";
        public const string GetProductsSuccess = "Fetch products successfully.";
        public const string GetCategoriesSuccess = "Fetch categories successfully.";
        public const string GetBrandsSuccess = "Fetch brands successfully.";
        public const string GetProductSuccess = "Fetch product details successfully.";
        public const string AddProductImageSuccess = "Product image added successfully";
        public const string AddProductImageError = "There is an error occured while adding the product image.";
        public const string DeleteProductImageSuccess = "Product image deleted successfully.";
        public const string DeleteProductImageError = "There is an error occured while deleting the product image.";
        public const string NoDataForUpdateOrder = "There is no data provided for update image ordering.";
        public const string UpdateImageOrderSuccess = "Image orderig updated successfully.";
        public const string UpdateImageOrderError = "There is an error occures while update image orderig!";
    }
}
