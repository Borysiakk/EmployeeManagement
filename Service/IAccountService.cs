using System.Threading.Tasks;
using EmployeeManagement.Contracts.Request;
using EmployeeManagement.Contracts.Result;
using EmployeeManagement.ViewModel;

namespace EmployeeManagement.Service
{
    public interface IAccountService
    {
        public Task LogoutAsync();
        public Task<UniversalResult> LoginAsync(UserLoginRequests request);
        public Task<UniversalResult> RegisterAsync(UserRegistrationRequest request);
    }
}