﻿using System;
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

        //Login
        public const string LoginSuccess = "Logged in successfully";
        public const string LoginError = "Invalid username or password";
        public const string LoginIssue = "Something went wrong while login with given user!";

        //Reset Passwors
        public const string ResetPasswordMailSuccess = "Reset password mail send successfully to your register email.";
        public const string UserNotFound = "There is no user found with procided email.";
        public const string ChangePasswordSuccess = "User's password change successfully";
        public const string ChangePasswordError = "There is an error occured while update an user password!";


    }
}
