using App.Models;

namespace App.Services
{
    public interface IUserService
    {
        bool IsEmailRegistered(string email);
        void Register(RegisterViewModel model);
    }
}