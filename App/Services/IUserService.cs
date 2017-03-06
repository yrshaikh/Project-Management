using App.Models;
using App.Models.User;

namespace App.Services
{
    public interface IUserService
    {
        bool IsEmailRegistered(string email);
        bool Authenticate(string email, string password);
        void Register(RegisterViewModel model);
        AuthenticatedUserModel GetUserDetails(string email);
    }
}