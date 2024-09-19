using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Common.Helpers
{
    public static class Utility
    {

        /// <summary>
        /// Check if valid email address or not
        /// </summary>
        /// <param name="emailaddress"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if valid mobile number or not (As per Norway standards)
        /// </summary>
        /// <param name="mobileNumber">Mobile number</param>
        /// <returns></returns>
        public static bool IsValidMobilNo(string mobileNumber)
        {
            bool isValid = false;

            if (mobileNumber.Length > 0 && mobileNumber.All(char.IsNumber))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// Generate random password
        /// </summary>
        /// <returns></returns>
        public static string GeneratePassword()
        {
            // Password should be 8 caharacters - first 6 lower case then 2 didgits
            var stringLower = "abcdefghijklmnopqrstuvwxyz";
            var numeric = "0123456789";
            var password = "";
            var character = "";
            var characters = 0;
            var numerics = 0;
            Random random = new Random();
            while (characters < 6) // 6 characters lower case
            {
                var entity1 = random.Next(0, stringLower.Length - 1);
                characters++;
                character += stringLower.ToCharArray()[entity1];
            }

            while (numerics < 2) // 2 digits
            {
                var entity3 = random.Next(0, numeric.Length - 1);
                numerics++;

                character += numeric.ToCharArray()[entity3]; 
            }
            password = character;

            return password;
        }

        //public static string ReplaceTokens(string template, List<ReplaceTokenDto> replaceTokens)
        //{
        //    foreach (ReplaceTokenDto pair in replaceTokens)
        //    {
        //        template = template.Replace(pair.Key, pair.Value);
        //    }
        //    return template;
        //}

        public static DateTime ConvertFromUTC(DateTime utcDateTime, string timeZone)
        {
            try
            {
                TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                DateTime dateTimeAsTimeZone = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, nzTimeZone);
                return dateTimeAsTimeZone;
            }
            catch
            {
                throw;
            }
        }

        public static DateTime ConvertToUTC(DateTime dateTime, string timeZone)
        {
            try
            {
                TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, nzTimeZone);
                return utcDateTime;
            }
            catch
            {
                throw;
            }
        }

        public static int[] ConvertEnumToIntArray(Type enums)
        {
            return System.Enum.GetValues(enums)
           .Cast<int>()
           .Select(x => x)
           .ToArray();
        }

        public static string ApplicationPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        
    }
}