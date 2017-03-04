using App.Models;

namespace App.Services
{
    public interface IUserService
    {
        bool IsEmailRegistered(string email);
        bool Authenticate(string email, string password);
        void Register(RegisterViewModel model);
    }
}