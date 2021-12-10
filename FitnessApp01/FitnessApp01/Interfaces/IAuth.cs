using System.Threading.Tasks;

namespace FitnessApp01.Interfaces
{
    public interface IAuth
    {
        Task<bool> RegisterUserAsync(string email, string password);
        Task<bool> LoginUserAsync(string email, string password);
        bool IsAuthenticated();
        string GetUserId();
        string GetUserEmail();
        bool SignOut();
    }
}
