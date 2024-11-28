using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Model.ViewModels.User
{
    public class Users
    {
        public List<UserListModel> UserList { get; set; }
    }

    public class UserListModel
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Password { get; set; }

        public DateTime? DOB { get; set; }
        public string Role { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
    }

    public class UsersModel
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Password { get; set; }
        public long Role { get; set; }
        public DateTime? DOB { get; set; }
        public string? Gender { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Avatar { get; set; }
        public IFormFile? AvatarFile { get; set; }
    }

    public class DashboardData
    {
        public int Customers { get; set; }
        public int NewlyRegistered { get; set; }
        public int SuperAdminCountOnJan { get; set; }
        public int SuperAdminCountOnFeb { get; set; }
        public int SuperAdminCountOnMarch { get; set; }
        public int SuperAdminCountOnApril { get; set; }
        public int SuperAdminCountOnMay { get; set; }
        public int SuperAdminCountOnJune { get; set; }
        public int SuperAdminCountOnJuly { get; set; }
        public int SuperAdminCountOnAug { get; set; }
        public int SuperAdminCountOnSep { get; set; }
        public int SuperAdminCountOnOct { get; set; }
        public int SuperAdminCountOnNov { get; set; }
        public int SuperAdminCountOnDec { get; set; }
        public int AdminCountOnJan { get; set; }
        public int AdminCountOnFeb { get; set; }
        public int AdminCountOnMarch { get; set; }
        public int AdminCountOnApril { get; set; }
        public int AdminCountOnMay { get; set; }
        public int AdminCountOnJune { get; set; }
        public int AdminCountOnJuly { get; set; }
        public int AdminCountOnAug { get; set; }
        public int AdminCountOnSep { get; set; }
        public int AdminCountOnOct { get; set; }
        public int AdminCountOnNov { get; set; }
        public int AdminCountOnDec { get; set; }
        public int UserCountOnJan { get; set; }
        public int UserCountOnFeb { get; set; }
        public int UserCountOnMarch { get; set; }
        public int UserCountOnApril { get; set; }
        public int UserCountOnMay { get; set; }
        public int UserCountOnJune { get; set; }
        public int UserCountOnJuly { get; set; }
        public int UserCountOnAug { get; set; }
        public int UserCountOnSep { get; set; }
        public int UserCountOnOct { get; set; }
        public int UserCountOnNov { get; set; }
        public int UserCountOnDec { get; set; }
    }
}