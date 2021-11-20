using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp01.Interfaces
{
    public interface IAuth
    {
        Task<bool> RegisterUser(string email, string password);
        Task<bool> LoginUser(string email, string password);
        bool IsAuthenticated();
        string GetUserId();
        string GetUserEmail();
        bool SignOut();
    }
}
