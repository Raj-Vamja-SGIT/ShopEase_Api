using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Common.Helpers
{
    public class ErrorMessages
    {
        // General
        public const string Error = "An Error Occured";

        // User
        public const string InvalidCredential = "Invalid email id or password";
        public const string InvalidEmailId = "Email id is not valid.";
        public const string FirstNameRequired = "First name is required";
        public const string LastNameRequired = "Last name is required";

        //Login
        public const string LoginSuccess = "Logged In Successfully";
        public const string LoginError = "Invalid username or password";

    }
}
