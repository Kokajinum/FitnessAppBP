using System.Threading.Tasks;

namespace FitnessApp01.Interfaces
{
    public interface IAuth
    {
        Task RegisterUserAsync(string email, string password);
        Task LoginUserAsync(string email, string password);
        bool IsAuthenticated();
        string GetUserId();
        string GetUserEmail();
        bool SignOut();
        Task UpdatePassword(string oldPassword, string oldPasswordConfirm, string password);
    }
}
